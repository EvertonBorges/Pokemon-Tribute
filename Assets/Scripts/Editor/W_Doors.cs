using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class W_Doors : EditorWindow
{

    private static GUIStyle boxStyle;
    private static GUIStyle separatorStyle;

    private static bool m_isSetuped = false;

    private static string ResourcesDoor => $"Assets/Resources/Doors";
    private static string ResourcesDoorTemp => $"{ResourcesDoor}/Door_Temp.asset";

    public static Generic_Tree<string, SO_Door> m_doorsTree = null;
    private static Dictionary<string, SO_Door> Doors => InspectorDoor.Extensions.Doors;

    private Vector2 m_scroolPosition;

    [MenuItem("Tools/Doors")]
    private static void ShowWindow()
    {
        var window = GetWindow<W_Doors>("Doors Window");

        var size = new Vector2(800, 400);

        // var teste = EditorGUIUtility.GetMainWindowPosition().center;

        var position = new Vector2(
            (Screen.currentResolution.width - size.x) / 2f,
            (Screen.currentResolution.height - size.y) / 2f
        );

        var rect = new Rect(position, size);

        window.minSize = size;

        window.maxSize = size * 1.25f;

        window.position = rect;

        m_isSetuped = false;

        window.Show();
    }

    public void Setup()
    {
        SetupStyles();

        PopulateDoorsTree();
    }

    private void SetupStyles()
    {
        boxStyle = new(GUI.skin.box)
        {
            alignment = TextAnchor.MiddleCenter
        };

        separatorStyle = new(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter
        };

        separatorStyle.normal.textColor = Color.grey;

        m_isSetuped = true;
    }

    private static void PopulateDoorsTree()
    {
        m_doorsTree = new("");

        foreach (var item in Doors)
        {
            string[] paths;

            if (item.Key.Contains("/"))
                paths = item.Key[..item.Key.LastIndexOf("/")].Split("/");
            else
                paths = new string[] { "" };

            var path = "";

            var actualNode = m_doorsTree;

            for (int i = 0; i < paths.Length; i++)
            {
                path += $"{(path.IsEmpty() ? "" : "/")}{paths[i]}";

                var node = actualNode.FindNode(path);

                if (node == null)
                {
                    node = new Generic_Tree<string, SO_Door>(path);

                    actualNode.AddNode(node);
                }

                actualNode = node;

                if (i == paths.Length - 1)
                    node.AddObj(item.Value);
            }
        }

        // m_doorsTree.PrintTree();
    }

    void OnGUI()
    {
        if (!m_isSetuped)
            Setup();

        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        DrawTableBody();

        DrawButtons();

        GUILayout.EndVertical();
    }

    private void DrawButtons()
    {
        GUILayout.BeginHorizontal(GUILayout.Height(40), GUILayout.ExpandWidth(true));

        GUILayout.FlexibleSpace();

        var disabledPlus = m_doorsTree.FindObj("Door_Temp") != null;

        EditorGUI.BeginDisabledGroup(disabledPlus);

        if (GUILayout.Button("+", GUILayout.ExpandHeight(true), GUILayout.Width(40)))
        {
            var door = CreateInstance<SO_Door>();

            AssetDatabase.CreateAsset(door, ResourcesDoorTemp);

            Save();
        }

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
    }

    private void Save()
    {
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

        Setup();

        Repaint();
    }

    private void DrawTableBody()
    {
        m_scroolPosition = GUILayout.BeginScrollView(m_scroolPosition, false, false, null, GUI.skin.verticalScrollbar, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        DrawNode(m_doorsTree);

        GUILayout.EndScrollView();
    }

    private void DrawNode(Generic_Tree<string, SO_Door> node)
    {
        if (node == null)
            return;

        if (node != m_doorsTree)
        {
            var path = node.path;

            if (!path.IsEmpty() && path.Contains("/"))
                path = path[(path.LastIndexOf("/") + 1)..];

            GUILayout.BeginHorizontal(boxStyle, GUILayout.ExpandWidth(true));

            var rect = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));

            if (GUI.Button(rect, "", boxStyle))
                node.isExpanded = !node.isExpanded;

            node.isExpanded = EditorGUI.Foldout(rect, node.isExpanded, path);

            GUILayout.EndHorizontal();

            if (!node.isExpanded)
                return;

            EditorGUI.indentLevel++;
        }

        foreach (var obj in node.objs)
        {
            var serializedObject = obj.Value;

            if (serializedObject == null)
                continue;

            var from = serializedObject.FindProperty("_from");

            var to = serializedObject.FindProperty("_to");

            from.FindPropertyRelative("_visibleLabel").boolValue = false;

            to.FindPropertyRelative("_visibleLabel").boolValue = false;

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(25));

            GUILayout.Space(EditorGUI.indentLevel * 16);

            GUILayout.BeginHorizontal(boxStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            EditorGUILayout.PropertyField(from, GUILayout.MinWidth(350), GUILayout.ExpandWidth(true));

            DrawHorizontalSeparator("->");

            EditorGUILayout.PropertyField(to, GUILayout.Width(250));

            DrawHorizontalSeparator();

            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("D", GUILayout.ExpandHeight(true), GUILayout.Width(25)))
            {
                var door = obj.Key;

                var path = door.From.Path;

                if (path.IsEmpty())
                    path = "Door Temporary";

                if (EditorUtility.DisplayDialog("DELETE", $"Delete asset: {path}?", "CONFIRM", "CANCEL"))
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(door));

                    Save();
                }
            }

            GUILayout.EndHorizontal();

            if (EditorGUI.indentLevel > 0)
                GUILayout.Space(20);

            GUILayout.EndHorizontal();
        }

        foreach (var item in node.nodes)
            DrawNode(item);

        if (node != m_doorsTree)
            EditorGUI.indentLevel--;
    }

    private void DrawHorizontalSeparator(string value = "|")
    {
        GUILayout.Label(value, separatorStyle, GUILayout.Width(20));
    }

}

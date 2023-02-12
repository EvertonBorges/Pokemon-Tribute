using System;
using UnityEditor;
using UnityEngine;

public class W_Doors : EditorWindow
{

    private static GUIStyle boxStyle;
    private static GUIStyle separatorStyle;

    private static bool m_isSetuped = false;

    private static string ResourcesDoor => $"Assets/Resources/Doors";
    private static string ResourcesDoorTemp => $"{ResourcesDoor}/Door_Temp.asset";

    private static Generic_Tree<SO_Door> Doors => D_Doors.DoorsTree;

    private static W_Doors m_window = null;

    private Vector2 m_scroolPosition;

    [MenuItem("Tools/Doors")]
    private static void ShowWindow()
    {
        var size = new Vector2(800, 400);

        if (m_window == null)
        {
            m_window = GetWindow<W_Doors>("Doors Windows");

            m_window.minSize = size;

            m_window.maxSize = size * 1.25f;
        }

        var position = new Vector2(
            (Screen.currentResolution.width - size.x) / 2f,
            (Screen.currentResolution.height - size.y) / 2f
        );

        var rect = new Rect(position, size);

        m_window.position = rect;

        m_isSetuped = false;

        m_window.Show();
    }

    public void Setup()
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

        D_Doors.Setup();
    }

    void OnGUI()
    {
        if (!m_isSetuped)
            Setup();

        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        DrawTableBody();

        GUILayout.EndVertical();
    }

    private void Save()
    {
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

        D_Doors.Setup();
    }

    private void DrawTableBody()
    {
        m_scroolPosition = GUILayout.BeginScrollView(m_scroolPosition, false, false, null, GUI.skin.verticalScrollbar, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        DrawNode(Doors);

        GUILayout.EndScrollView();
    }

    private void DrawNode(Generic_Tree<SO_Door> node)
    {
        if (node == null)
            return;

        if (node != Doors)
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

            var door = obj.Key;

            var path = door.From.Path;

            if (!path.IsEmpty())
            {
                DrawHorizontalSeparator();

                if (GUILayout.Button("D", GUILayout.ExpandHeight(true), GUILayout.Width(25)))
                {
                    if (EditorUtility.DisplayDialog("DELETE", $"Delete asset: {path}?", "CONFIRM", "CANCEL"))
                    {
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(door));

                        Save();
                    }
                }
            }

            GUILayout.EndHorizontal();

            if (EditorGUI.indentLevel > 0)
                GUILayout.Space(20);

            GUILayout.EndHorizontal();
        }

        try
        {
            foreach (var item in node.nodes)
            {
                if (item == null)
                    continue;

                DrawNode(item);
            }
        }
        catch (InvalidOperationException)
        {

        }

        if (node != Doors)
            EditorGUI.indentLevel--;
    }

    private void DrawHorizontalSeparator(string value = "|")
    {
        GUILayout.Label(value, separatorStyle, GUILayout.Width(20));
    }

}

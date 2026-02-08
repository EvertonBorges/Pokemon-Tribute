using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

public class W_Pokemon : EditorWindow
{

    private static GUIStyle boxStyle;
    private static GUIStyle separatorStyle;

    private static bool m_isSetuped = false;

    private static Generic_Tree<SO_Pokemon> Pokemons => D_Pokemons.PokemonsTree;

    private static W_Pokemon m_window = null;

    private Vector2 m_scroolPosition;

    [MenuItem("Tools/Pokemons")]
    private static void ShowWindow()
    {
        var size = new Vector2(800, 400);

        if (m_window == null)
        {
            m_window = GetWindow<W_Pokemon>("Pokemon's Windows");

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

        D_Pokemons.Setup();
    }

    void OnGUI()
    {
        if (!m_isSetuped)
            Setup();

        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        DrawTableBody();

        GUILayout.EndVertical();
    }

    private void DrawTableBody()
    {
        m_scroolPosition = GUILayout.BeginScrollView(m_scroolPosition, false, false, null, GUI.skin.verticalScrollbar, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        GUILayout.BeginHorizontal(boxStyle, GUILayout.ExpandWidth(true));

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Refresh", GUILayout.ExpandHeight(true), GUILayout.Width(100)))
            ND_Pokemon.GetPokemonDatabase();

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        DrawNode(Pokemons);

        GUILayout.EndScrollView();
    }

    private void DrawNode(Generic_Tree<SO_Pokemon> node)
    {
        if (node == null)
            return;

        if (node != Pokemons)
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

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(25));

            GUILayout.Space(EditorGUI.indentLevel * 16);

            GUILayout.BeginHorizontal(boxStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            GUILayout.Label(serializedObject.FindProperty("name").stringValue, GUILayout.ExpandWidth(true));

            GUILayout.EndHorizontal();

            if (EditorGUI.indentLevel > 0)
                GUILayout.Space(20);

            GUILayout.EndHorizontal();
        }

        foreach (var item in node.nodes)
        {
            if (item == null)
                continue;

            DrawNode(item);
        }

        if (node != Pokemons)
            EditorGUI.indentLevel--;
    }

    private void DrawHorizontalSeparator(string value = "|")
    {
        GUILayout.Label(value, separatorStyle, GUILayout.Width(20));
    }

}

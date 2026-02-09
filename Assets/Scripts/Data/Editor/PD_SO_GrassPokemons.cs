using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_GrassPokemons))]
public class PD_SO_GrassPokemons : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var obj = (SO_GrassPokemons) target;

        float total = 0f;

        foreach (var pokemon in obj.Pokemons.Values)
            total += pokemon.rate;

        var style = new GUIStyle();
        style.normal.textColor = total != 100f ? Color.red : GUI.color;
        style.alignment = TextAnchor.MiddleRight;

        GUILayout.Space(EditorGUIUtility.standardVerticalSpacing * 10f);
        GUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight));
        GUILayout.Label("Total: ");
        GUILayout.Label($"{total}", style, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();
    }

}

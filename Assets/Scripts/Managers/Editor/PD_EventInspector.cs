using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EventInspector))]
public class PD_EventInspector : PropertyDrawer
{

    private const string variableName = "_event";

    private readonly Color missingEnumColor = new(1, 0, 0, 0.2f);

    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        return false;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var eventProperty = property.FindPropertyRelative(variableName);

        var rectButton = EditorGUI.PrefixLabel(position, label);

        var eventName = eventProperty.stringValue;
        var tooltip = "Select an Event";

        bool hasName = !eventName.IsEmpty();

        if (eventName.IsEmpty())
            eventName = "NO VALUE";
        else
        {
            tooltip = eventName.Replace("/", " > ");

            var paths = eventName.Split("/");
            eventName = "";

            for (int i = Mathf.Max(paths.Length - 1, 0); i < paths.Length; i++)
                eventName += $"{paths[i]}";
        }

        if (GUI.Button(rectButton, new GUIContent(eventName, tooltip)))
            DrawMenu(property, eventProperty);

        if (!hasName) EditorGUI.DrawRect(position, missingEnumColor);

        property.serializedObject.ApplyModifiedProperties();

#if !UNITY_EDITOR
        eventProperty.Dispose();
#endif
    }

    private void DrawMenu(SerializedProperty property, SerializedProperty eventProperty)
    {
        GenericMenu menu = new();

        menu.AddItem(new GUIContent("NO VALUE"), false, () =>
        {
            eventProperty.stringValue = "";
            EditorUtility.SetDirty(property.serializedObject.targetObject);
            property.serializedObject.ApplyModifiedProperties();
        });

        foreach (var item in Manager_Events.EventsExtensions.Fields)
        {
            menu.AddItem(new GUIContent(item.Key, item.Key), false, () =>
            {
                eventProperty.stringValue = item.Key;
                EditorUtility.SetDirty(property.serializedObject.targetObject);
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        menu.ShowAsContext();
    }

}

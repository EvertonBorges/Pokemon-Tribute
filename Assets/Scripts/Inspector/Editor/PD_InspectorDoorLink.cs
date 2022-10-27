using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InspectorDoorLink))]
public class PD_InspectorDoorLink : PropertyDrawer
{

    private const string pathVariableName = "_path";
    private const string visibleLabelVariableName = "_visibleLabel";
    private const string emptyValue = "Null Door";

    private readonly Color missingEnumColor = new(1, 0, 0, 0.2f);

    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        return false;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var pathProperty = property.FindPropertyRelative(pathVariableName);

        var visibleLabelProperty = property.FindPropertyRelative(visibleLabelVariableName);

        var rectButton = position;

        if (visibleLabelProperty.boolValue)
            rectButton = EditorGUI.PrefixLabel(rectButton, label);

        var name = pathProperty.stringValue;

        var tooltip = "Select a Door";

        bool hasName = !name.IsEmpty();

        if (name.IsEmpty())
            name = emptyValue;
        else
        {
            tooltip = name.Replace("/", " > ");

            name = tooltip;
        }

        if (GUI.Button(rectButton, new GUIContent(name, tooltip)))
            DrawMenu(property.serializedObject, pathProperty);

        if (!hasName) EditorGUI.DrawRect(position, missingEnumColor);

        property.serializedObject.ApplyModifiedProperties();

#if !UNITY_EDITOR
        pathProperty.Dispose();
        
        sceneVisibleLabelProperty.Dispose();

        visibleLabelProperty.Dispose();

        sceneProperty.Dispose();

        property.Dispose();
#endif
    }

    private void DrawMenu(SerializedObject obj, SerializedProperty relativeProperty)
    {
        GenericMenu menu = new();

        menu.AddItem(new GUIContent(emptyValue), false, () =>
        {
            relativeProperty.stringValue = "";

            EditorUtility.SetDirty(obj.targetObject);

            obj.ApplyModifiedProperties();
        });

        foreach (var item in InspectorDoor.Extensions.Doors)
        {
            if (item.Key.EndsWith("Door_Temp"))
                continue;
                
            menu.AddItem(new GUIContent(item.Key), false, () =>
            {
                relativeProperty.stringValue = item.Key;

                EditorUtility.SetDirty(obj.targetObject);

                obj.ApplyModifiedProperties();
            });
        }

        menu.ShowAsContext();
    }

}

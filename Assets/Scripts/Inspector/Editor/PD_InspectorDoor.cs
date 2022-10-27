using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InspectorDoor))]
public class PD_InspectorDoor : PropertyDrawer
{

    private const string nameVariableName = "name";
    private const string pathVariableName = "_path";
    private const string sceneVariableName = "_scene";
    private const string visibleLabelVariableName = "_visibleLabel";
    private const string emptyValue = "Null Door";

    private const string focusField = "PATH_FOCUS_TEXTFIELD";

    private bool m_changing = false;

    private readonly Color missingEnumColor = new(1, 0, 0, 0.2f);

    private static string ResourcesDoor => $"Assets/Resources/Doors";
    private static string ResourcesDoorFilename(string value) => $"{ResourcesDoor}/{value}.asset";

    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        return false;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var nameProperty = property.FindPropertyRelative(nameVariableName);

        var pathProperty = property.FindPropertyRelative(pathVariableName);

        var visibleLabelProperty = property.FindPropertyRelative(visibleLabelVariableName);

        var sceneProperty = property.FindPropertyRelative(sceneVariableName);

        var sceneVisibleLabelProperty = sceneProperty.FindPropertyRelative(visibleLabelVariableName);

        sceneVisibleLabelProperty.boolValue = false;

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

        var rectScene = rectButton;

        rectScene.width = 150;

        EditorGUI.PropertyField(rectScene, sceneProperty);

        rectButton.x = rectScene.x + rectScene.width + 2;

        rectButton.width = rectButton.width - rectScene.width - 2;

        if (m_changing)
        {
            GUI.SetNextControlName(focusField);

            pathProperty.stringValue = EditorGUI.TextField(rectButton, pathProperty.stringValue, GUI.skin.textField);

            GUI.FocusControl(focusField);

            HandleKeyEvents(pathProperty);
        }
        else if (GUI.Button(rectButton, new GUIContent(name, tooltip)))
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

    private void HandleKeyEvents(SerializedProperty relativeProperty)
    {
        Event _event = Event.current;

        KeyCode keyCode = _event.keyCode;

        if (_event.type != EventType.KeyDown)
            return;

        if (keyCode == KeyCode.KeypadEnter || keyCode == KeyCode.Return)
        {
            m_changing = false;

            var filename = relativeProperty.stringValue;

            var newPath = ResourcesDoorFilename(filename);

            var folder = newPath[..newPath.LastIndexOf("/")];

            CreateFolder(folder);

            var oldPath = $"{ResourcesDoor}/Door_Temp.asset";

            var door = AssetDatabase.LoadAssetAtPath<SO_Door>(oldPath);

            AssetDatabase.MoveAsset(oldPath, newPath);

            door.SetPath(InspectorDoor.Extensions.GetReducedPath(newPath));

            Save();

            SceneView.RepaintAll();

            GUIUtility.hotControl = 0;

            Event.current.Use();
        }
        else if (keyCode == KeyCode.Escape)
        {
            m_changing = false;

            relativeProperty.stringValue = "";

            SceneView.RepaintAll();

            GUIUtility.hotControl = 0;

            Event.current.Use();
        }
    }

    private void DrawMenu(SerializedObject obj, SerializedProperty relativeProperty)
    {
        GenericMenu menu = new();

        menu.AddItem(new GUIContent("New Value"), false, () =>
        {
            m_changing = true;

            relativeProperty.stringValue = "";

            EditorUtility.SetDirty(obj.targetObject);

            obj.ApplyModifiedProperties();
        });

        menu.AddItem(new GUIContent(emptyValue), false, () =>
        {
            m_changing = false;

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
                m_changing = false;

                relativeProperty.stringValue = item.Key;

                EditorUtility.SetDirty(obj.targetObject);

                obj.ApplyModifiedProperties();
            });
        }

        menu.ShowAsContext();
    }

    private void CreateFolder(string folder)
    {
        var paths = folder.Split("/");

        var path = "";

        for (int i = 0; i < paths.Length; i++)
        {
            if (i == 0)
                path += paths[i];
            else
                path += $"/{paths[i]}";

            if(!AssetDatabase.IsValidFolder(path))
            {
                var parentDir = path[..path.LastIndexOf("/")];

                var folderDir = path[(path.LastIndexOf("/") + 1)..];

                AssetDatabase.CreateFolder(parentDir, folderDir);
            }
        }

        Save();
    }

    private void Save()
    {
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

        var window = EditorWindow.GetWindow<W_Doors>();

        if (window != null)
            window.Setup();
    }

}

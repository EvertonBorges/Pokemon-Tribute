using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class D_Doors
{

    private static readonly Generic_Tree<SO_Door> m_doorsTree = new("");
    public static Generic_Tree<SO_Door> DoorsTree => m_doorsTree;

    private static Dictionary<string, SO_Door> Doors => Manager_Resources.Door_Resources.Doors;

    private static string ResourcesDoor => $"Assets/Resources/Doors";
    private static string ResourcesDoorFilename(string value)
    {
        var filename = "Door_Temp";

        if (!value.IsEmpty())
            filename = value;

        return $"{ResourcesDoor}/{filename}.asset";
    }

    public static void Setup()
    {
        CreateEmptyDoor();

        m_doorsTree.Clear();

        foreach (var item in Doors)
            m_doorsTree.AddNode(item);

        // m_doorsTree.PrintTree();
    }

    public static void CreateEmptyDoor()
    {
        var emptyDoors = Doors.Values.ToList().FindAll(x => x.From.Path.IsEmpty());

        if (emptyDoors.IsEmpty())
            CreateDoor();
    }

    public static void CreateDoor(string filename = "")
    {
        var door = ScriptableObject.CreateInstance<SO_Door>();

        var path = filename.IsEmpty() ? "" : StringUtils.GetReducedPath(ResourcesDoorFilename(filename), ".asset", 3);

        door.SetPath(path);

        var assetPath = ResourcesDoorFilename(filename);

        var folder = assetPath[..assetPath.LastIndexOf("/")];

        folder.CreateResourcesFolder();

        AssetDatabase.CreateAsset(door, assetPath);

        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

        m_doorsTree.AddNode(new(path, door), true);

        m_doorsTree.PrintTree();
    }

}

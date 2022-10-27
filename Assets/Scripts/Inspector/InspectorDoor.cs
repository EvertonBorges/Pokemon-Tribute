using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class InspectorDoor
{

    [SerializeField] private bool _visibleLabel = true;
    [SerializeField] private string _path;
    [SerializeField] private InspectorScene _scene;

    public string Path => _path;
    public InspectorScene Scene => _scene;

    public void SetPath(string value) => _path = value;

    public static class Extensions
    {
        public static Dictionary<string, SO_Door> Doors
        {
            get
            {
                var doorsResource = Resources.LoadAll<SO_Door>("").ToList();

                Dictionary<string, SO_Door> doors = new();

                foreach (var door in doorsResource)
                    doors.Add(GetReducedPath(AssetDatabase.GetAssetPath(door)), door);

                return doors;
            }
        }

        public static string GetReducedPath(string path)
        {
            for (int i = 0; i < 3; i++)
                path = path.Remove(0, path.IndexOf("/") + 1);

            return path.Replace(".asset", "");
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class Manager_Resources
{

    public static class Door_Resources
    {

        private static Dictionary<string, SO_Door> m_doors;

        public static Dictionary<string, SO_Door> Doors
        {
            get
            {
                if (!Application.isPlaying)
                    Setup();
                else if (m_doors.IsEmpty())
                    Setup();

                return m_doors;
            }
        }

        private static void Setup()
        {
            var doorsResource = Resources.LoadAll<SO_Door>("").ToList();

            m_doors = new();

            foreach (var door in doorsResource)
                m_doors.Add(StringUtils.GetReducedPath(AssetDatabase.GetAssetPath(door), ".asset", 3), door);
        }
    }

}

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

    public static class Pokemon_Resourcers
    {
        private static Dictionary<string, SO_Type> m_types;

        public static Dictionary<string, SO_Type> Types
        {
            get
            {
                if (!Application.isPlaying)
                    Setup();
                else if (m_types.IsEmpty())
                    Setup();

                return m_types;
            }
        }

        private static Dictionary<string, SO_Pokemon> m_pokemons;

        public static Dictionary<string, SO_Pokemon> Pokemons
        {
            get
            {
                if (!Application.isPlaying)
                    Setup();
                else if (m_pokemons.IsEmpty())
                    Setup();

                return m_pokemons;
            }
        }

        private static void Setup()
        {
            var typeResources = Resources.LoadAll<SO_Type>("").ToList();

            var pokemonResources = Resources.LoadAll<SO_Pokemon>("").ToList();

            m_types = new();

            m_pokemons = new();

            foreach (var type in typeResources.OrderBy(x => x.id))
                m_types.Add(type.name, type);

            foreach (var pokemon in pokemonResources.OrderBy(x => x.id))
                m_pokemons.Add(pokemon.name, pokemon);
        }
    }

}

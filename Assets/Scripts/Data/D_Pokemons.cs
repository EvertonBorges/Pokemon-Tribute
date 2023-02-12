using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class D_Pokemons
{

    private static readonly Generic_Tree<SO_Pokemon> m_pokemonsTree = new("");
    public static Generic_Tree<SO_Pokemon> PokemonsTree => m_pokemonsTree;

    private static Dictionary<string, SO_Pokemon> Pokemons => Manager_Resources.Pokemon_Resourcers.Pokemons;

    private static string ResourcesPokemon => $"Assets/Resources/Database/Pokemon";

    private static string ResourcesPokemonFilename(string value)
    {
        var filename = "Pokemon_Temp";

        if (!value.IsEmpty())
            filename = value;

        return $"{ResourcesPokemon}/{filename}.asset";
    }

    public static void Setup()
    {
        m_pokemonsTree.isRootable = true;

        m_pokemonsTree.Clear();

        foreach (var pokemon in Pokemons)
            m_pokemonsTree.AddNode(pokemon);
    }

    public static void CreatePokemon(string filename = "")
    {
        var pokemon = ScriptableObject.CreateInstance<SO_Pokemon>();

        var path = filename.IsEmpty() ? "" : StringUtils.GetReducedPath(ResourcesPokemonFilename(filename), ".asset", 3);

        var assetPath = ResourcesPokemonFilename(filename);

        var folder = assetPath[..assetPath.LastIndexOf("/")];

        folder.CreateResourcesFolder();

        AssetDatabase.CreateAsset(pokemon, assetPath);

        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

        m_pokemonsTree.AddNode(new(path, pokemon), true);

        // m_pokemonsTree.PrintTree();
    }

}

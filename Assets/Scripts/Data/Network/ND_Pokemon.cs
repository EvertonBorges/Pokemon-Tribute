using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class ND_Pokemon
{

    private const string LANGUAGE = "en"; // ja-Hrkt, ko, zh-Hant, fr, de, es, it, en, ja, zh-Hans
    private const string VERSION_GROUP = "red-blue";
    private const string VERSION = "red";

    private static readonly List<string> versionGroups = new()
    {
        "red-green", "red-blue", "yellow",
        "gold-silver", "crystal",
        "ruby-sapphire", "emerald", "firered-leafgreen",
        "diamond-pearl", "platinum", "heartgold-soulsilver",
        "black-white", "black-2-white-2",
        "x-y", "omega-ruby-alpha-sapphire",
        "sun-moon", "ultra-sun-ultra-moon", "lets-go-pikachu-lets-go-eevee",
        "sword-shield", "brilliant-diamond-shining-pearl", "legends-arceus",
        "scarlet-violet"
    };

    private static readonly List<string> versions = new()
    {
        "red", "blue", "green", "yellow",
        "gold", "silver", "crystal",
        "ruby", "sapphire", "emerald", "firered", "leafgreen",
        "diamond", "pearl", "platinum", "heartgold", "soulsilver",
        "black", "white", "black-2", "white-2",
        "x", "y", "omega-ruby", "alpha-sapphire",
        "sun", "moon", "ultra-sun", "ultra-moon", "lets-go-pikachu", "lets-go-eevee",
        "sword", "shield", "brilliant-diamond", "shining-pearl", "legends-arceus",
        "scarlet", "violet"
    };

    private static string Resources => $"Assets/Resources/Database";
    private static string ResourcesPokemon => $"{Resources}/Pokemon";
    private static string ResourcesType => $"{Resources}/Type";
    private static string ResourcesMove => $"{Resources}/Move";
    private static string ResourcesMoveTarget => $"{Resources}/MoveTarget";
    private static string ResourcesSprite => $"{Resources}/Sprite";

    private static string ResourcesPokemonFilename(string value) => $"{ResourcesPokemon}/{value}.asset";
    private static string ResourcesTypeFilename(string value) => $"{ResourcesType}/{value}.asset";
    private static string ResourcesMoveFilename(string value) => $"{ResourcesMove}/{value}.asset";
    private static string ResourcesMoveTargetFilename(string value) => $"{ResourcesMoveTarget}/{value}.asset";
    private static string ResourcesSpriteFilename(int id, string value) => $"{ResourcesSprite}/{id}/{value}.png";

    private const string URL_MOVE_TARGET = "https://pokeapi.co/api/v2/move-target?limit=10000";
    private const string URL_GENERATION = "https://pokeapi.co/api/v2/generation";

    private static readonly Dictionary<string, SO_Type> m_types_so = new();

    private static readonly Dictionary<string, PokeAPI_MoveTarget> m_moveTargets = new();
    private static readonly Dictionary<string, SO_MoveTarget> m_moveTargets_so = new();
    private static readonly Dictionary<string, SO_Move> moves_so = new();
    private static readonly Dictionary<string, SO_Pokemon> pokemons_so = new();


    private static readonly Dictionary<string, PokeAPI_Pokemon> pokemons_api = new();

    public static void GetPokemonDatabase()
    {
        Populate();
    }

    private async static void Populate()
    {
        ClearDictionaries();

        var generation = 1;

        var url = $"{URL_GENERATION}/{generation}";

        await PopulateAllMoveTargets();

        var generationApi = await NetworkTask<PokeAPI_Generation>.Get(url);

        Debug.Log("Populating Types");

        await PopulateTypes(generation, generationApi.types);

        Debug.Log("Types Populated");

        Debug.Log("Populating Moves");

        await PopulateMoves(generationApi.moves);

        Debug.Log("Moves Populated");

        Debug.Log("Populating Pokemons");

        await PopulatePokemons(generation, generationApi.pokemon_species);

        Debug.Log("Pokemons Populated");

        Debug.Log("Populating Pokemons Sprites");

        await PopulatePokemonsSprites();

        Debug.Log("Pokemons Sprites Populated");

        AssetDatabase.SaveAssets();
    }

    private static void ClearDictionaries()
    {
        moves_so.Clear();

        m_moveTargets_so.Clear();

        m_moveTargets.Clear();

        m_types_so.Clear();
    }

    private async static Task PopulateTypes(int generation, List<PokeAPI_NameUrl> types)
    {
        Dictionary<string, PokeAPI_Type> typesApi = new();

        ClearLog();

        foreach (var type in types)
        {
            var typeApi = await NetworkTask<PokeAPI_Type>.Get(type.url);

            typesApi.Add(typeApi.name, typeApi);

            Debug.Log($"Types (API): {typesApi.Count}/{types.Count}");
        }

        foreach (var type in typesApi)
        {
            var so_type = ScriptableObject.CreateInstance<SO_Type>();

            m_types_so.Add(type.Key, so_type);
        }

        foreach (var type_so in m_types_so)
        {
            var type = type_so.Value;

            var typeApi = typesApi[type_so.Key];

            type.id = typeApi.id;

            type.name = typeApi.name;

            var path = ResourcesTypeFilename($"{type.id}. {type.name}");

            path.CreateResourcesFolder();

            AssetDatabase.CreateAsset(type, path);
        }

        foreach (var type_so in m_types_so)
        {
            var type = type_so.Value;

            var typeApi = typesApi[type_so.Key];

            var damage_relations = typeApi.damage_relations;

            if (!typeApi.past_damage_relations.IsEmpty())
            {
                var past_damage_relation = typeApi.past_damage_relations.Find(x => generation <= x.generation.name.Remove(0, "generation-".Length).RomanToDecimal());

                if (!past_damage_relation.generation.name.IsEmpty())
                    damage_relations = past_damage_relation.damage_relations;
            }

            MakeRelation(damage_relations.double_damage_from, type.double_damage_from);

            MakeRelation(damage_relations.double_damage_to, type.double_damage_to);

            MakeRelation(damage_relations.half_damage_from, type.half_damage_from);

            MakeRelation(damage_relations.half_damage_to, type.half_damage_to);

            MakeRelation(damage_relations.no_damage_from, type.no_damage_from);

            MakeRelation(damage_relations.no_damage_to, type.no_damage_to);

            EditorUtility.SetDirty(type);

            AssetDatabase.SaveAssetIfDirty(type);

            void MakeRelation(List<PokeAPI_NameUrl> relation, List<SO_Type> types)
            {
                types.Clear();

                relation
                    .FindAll(x => m_types_so.ContainsKey(x.name))
                    .ForEach(x => types.Add(m_types_so[x.name]));
            }
        }

        AssetDatabase.Refresh();
    }

    private async static Task PopulateMoves(List<PokeAPI_NameUrl> moves)
    {
        Dictionary<string, PokeAPI_Move> movesApi = new();

        ClearLog();

        foreach (var move in moves)
        {
            var moveApi = await NetworkTask<PokeAPI_Move>.Get(move.url);

            movesApi.Add(moveApi.name, moveApi);

            Debug.Log($"Moves (API): {movesApi.Count}/{moves.Count}");
        }

        moves_so.Clear();

        foreach (var moveApi in movesApi)
        {
            var so_move = ScriptableObject.CreateInstance<SO_Move>();

            moves_so.Add(moveApi.Key, so_move);
        }

        foreach (var move_so in moves_so)
        {
            var so_move = move_so.Value;

            var moveApi = movesApi[move_so.Key];

            so_move.id = moveApi.id;

            so_move.accuracy = moveApi.accuracy;

            so_move.name = moveApi.name;

            var index = versionGroups.FindIndex(x => x == VERSION_GROUP);

            var version =
                moveApi.flavor_text_entries
                    .FindAll(x => x.language.name == LANGUAGE)
                    .Select(x => x.version_group.name)
                    .ToList()
                    .Find(x => index <= versionGroups.FindIndex(y => y == x));

            so_move.description =
                moveApi.flavor_text_entries
                    .Find(x => x.language.name == LANGUAGE && x.version_group.name == version)
                    .flavor_text
                    .Replace("\n", " ");

            so_move.power = moveApi.power;

            so_move.pp = moveApi.pp;

            so_move.priority = moveApi.priority;

            if (m_types_so.ContainsKey(moveApi.type.name))
                so_move.type = m_types_so[moveApi.type.name];

            so_move.moveTarget = m_moveTargets_so[moveApi.target.name];

            if (!moveApi.past_values.IsEmpty())
            {
                var pastVersionGroupIndex =
                    moveApi.past_values
                        .Select(x => versionGroups.FindIndex(y => y == x.version_group.name))
                        .OrderBy(x => x)
                        .ToList()
                        .Find(x => index <= x);

                var pastVersionGroup = versionGroups[pastVersionGroupIndex];

                var pastValue = moveApi.past_values.Find(x => x.version_group.name == pastVersionGroup);

                if (pastValue != null)
                {
                    so_move.accuracy = pastValue.accuracy ?? so_move.accuracy;

                    so_move.power = pastValue.power ?? so_move.power;

                    so_move.pp = pastValue.pp ?? so_move.pp;

                    if (pastValue.type != null && m_types_so.ContainsKey(pastValue.type.name))
                        so_move.type = m_types_so[pastValue.type.name];
                }
            }
        }

        foreach (var move_so in moves_so)
        {
            var so_move = move_so.Value;

            var path = ResourcesMoveFilename($"{so_move.id}. {so_move.name}");

            path.CreateResourcesFolder();

            AssetDatabase.CreateAsset(so_move, path);
        }
    }

    private async static Task PopulatePokemons(int generation, List<PokeAPI_NameUrl> pokemonSpecies)
    {
        Dictionary<string, PokeAPI_PokemonSpecies> pokemonSpeciesApi = new();

        Dictionary<string, PokeAPI_EvolutionChain> pokemonsEvolutionChainApi = new();

        ClearLog();

        foreach (var pokemonSpecie in pokemonSpecies)
        {
            var pokemonSpecieApi = await NetworkTask<PokeAPI_PokemonSpecies>.Get(pokemonSpecie.url);

            var pokemonApi = await NetworkTask<PokeAPI_Pokemon>.Get(pokemonSpecieApi.varieties.Find(x => x.is_default).pokemon.url);

            pokemonSpeciesApi.Add(pokemonSpecieApi.name, pokemonSpecieApi);

            pokemons_api.Add(pokemonApi.name, pokemonApi);

            if (!pokemonsEvolutionChainApi.ContainsKey(pokemonSpecieApi.evolution_chain.url))
            {
                var pokemonEvolutionChainApi = await NetworkTask<PokeAPI_EvolutionChain>.Get(pokemonSpecieApi.evolution_chain.url);

                pokemonsEvolutionChainApi.Add(pokemonSpecieApi.evolution_chain.url, pokemonEvolutionChainApi);
            }

            Debug.Log($"Pokemons (API): {pokemonSpeciesApi.Count}/{pokemonSpecies.Count}");
        }

        foreach (var pokemonSpecieApi in pokemonSpeciesApi)
        {
            var so_pokemon = ScriptableObject.CreateInstance<SO_Pokemon>();

            pokemons_so.Add(pokemonSpecieApi.Key, so_pokemon);
        }

        foreach (var pokemon_so in pokemons_so)
        {
            var so_pokemon = pokemon_so.Value;

            var pokemonSpecieApi = pokemonSpeciesApi[pokemon_so.Key];

            var pokemonApi = pokemons_api[pokemon_so.Key];

            so_pokemon.id = pokemonApi.id;

            so_pokemon.name = pokemonApi.name;

            so_pokemon.description =
                pokemonSpecieApi
                    .flavor_text_entries
                    .Find(x => x.language.name == LANGUAGE && x.version.name == VERSION)
                    .flavor_text
                    .Replace("\n", " ");

            so_pokemon.base_stats.hp = pokemonApi.stats.Find(x => x.stat.name == "hp").base_stat;
            so_pokemon.base_stats.attack = pokemonApi.stats.Find(x => x.stat.name == "attack").base_stat;
            so_pokemon.base_stats.defense = pokemonApi.stats.Find(x => x.stat.name == "defense").base_stat;
            so_pokemon.base_stats.special_attack = pokemonApi.stats.Find(x => x.stat.name == "special-attack").base_stat;
            so_pokemon.base_stats.special_defense = pokemonApi.stats.Find(x => x.stat.name == "special-defense").base_stat;
            so_pokemon.base_stats.speed = pokemonApi.stats.Find(x => x.stat.name == "speed").base_stat;

            var movesVersionGroup =
                pokemonApi.moves
                    .FindAll(x => x.version_group_details.Any(y => y.version_group.name == VERSION_GROUP && y.move_learn_method.name == "level-up"))
                    .OrderBy(x => x.move.name)
                    .OrderBy(x => x.version_group_details.Find(y => y.version_group.name == VERSION_GROUP).level_learned_at)
                    .ToList();

            so_pokemon.moves = movesVersionGroup.Select(ToMove).ToList();

            so_pokemon.types = pokemonApi.types.FindAll(x => m_types_so.ContainsKey(x.type.name)).Select(x => m_types_so[x.type.name]).ToList();

            if (!pokemonApi.past_types.IsEmpty())
            {
                var generationTypes = pokemonApi.past_types.Find(x => generation <= x.generation.name.Remove(0, "generation-".Length).RomanToDecimal());

                if (generationTypes != null && !generationTypes.types.IsEmpty())
                    so_pokemon.types = generationTypes.types.Select(x => m_types_so[x.type.name]).ToList();
            }

            var path = ResourcesPokemonFilename($"{so_pokemon.id}. {so_pokemon.name}");

            path.CreateResourcesFolder();

            AssetDatabase.CreateAsset(so_pokemon, path);
        }

        foreach (var pokemon_so in pokemons_so)
        {
            var so_pokemon = pokemon_so.Value;

            var pokemonSpecieApi = pokemonSpeciesApi[pokemon_so.Key];

            if (pokemonsEvolutionChainApi.ContainsKey(pokemonSpecieApi.evolution_chain.url))
            {
                var pokemonsEvolutionChain = new List<PokeAPI_EvolutionChain.PokeAPI_Chain>() { pokemonsEvolutionChainApi[pokemonSpecieApi.evolution_chain.url].chain };

                var speciesName = so_pokemon.name;

                while (!pokemonsEvolutionChain.Any(x => x.species.name == speciesName))
                    pokemonsEvolutionChain =
                        pokemonsEvolutionChain
                            .SelectMany(x => x.evolves_to)
                            .Distinct()
                            .Where(x => pokemons_so.ContainsKey(x.species.name))
                            .ToList();

                foreach (var pokemonEvolutionChain in pokemonsEvolutionChain)
                {
                    foreach (var evolutionApi in pokemonEvolutionChain.evolves_to)
                    {
                        if (!pokemons_so.ContainsKey(evolutionApi.species.name))
                            continue;

                        SO_Pokemon.Evolution evolution = new() { pokemon = pokemons_so[evolutionApi.species.name] };

                        evolutionApi.evolution_details.ForEach(y =>
                            evolution.evolutionConditions.Add(
                                new() { trigger = y.trigger.name, min_level = y.min_level ?? -1 }));

                        so_pokemon.evolve_to.Add(evolution);
                    }
                }
            }

            EditorUtility.SetDirty(so_pokemon);

            AssetDatabase.SaveAssetIfDirty(so_pokemon);

            AssetDatabase.Refresh();
        }

        static void UpdateSpritesReference(SO_Pokemon so_pokemon)
        {
            var id = so_pokemon.id;

            var backDefault = AssetDatabase.LoadAssetAtPath<Sprite>(ResourcesSpriteFilename(id, "backDefault"));

            var frontDefault = AssetDatabase.LoadAssetAtPath<Sprite>(ResourcesSpriteFilename(id, "frontDefault"));

            so_pokemon.sprites.backDefault = backDefault;

            so_pokemon.sprites.frontDefault = frontDefault;
        }
    }

    private async static Task PopulatePokemonsSprites()
    {
        ClearLog();

        foreach (var pokemon_so in pokemons_so.OrderBy(x => x.Value.id))
        {
            var so_pokemon = pokemon_so.Value;

            var pokemonApi = pokemons_api[pokemon_so.Key];

            await SaveTextures(pokemonApi);

            UpdateSpritesReference(so_pokemon);

            Debug.Log($"Pokemon Sprites: {so_pokemon.id}/{pokemons_so.Count}");

            EditorUtility.SetDirty(so_pokemon);

            AssetDatabase.SaveAssetIfDirty(so_pokemon);

            AssetDatabase.Refresh();
        }

        static void UpdateSpritesReference(SO_Pokemon so_pokemon)
        {
            var id = so_pokemon.id;

            var backDefault = AssetDatabase.LoadAssetAtPath<Sprite>(ResourcesSpriteFilename(id, "backDefault"));

            var frontDefault = AssetDatabase.LoadAssetAtPath<Sprite>(ResourcesSpriteFilename(id, "frontDefault"));

            so_pokemon.sprites.backDefault = backDefault;

            so_pokemon.sprites.frontDefault = frontDefault;
        }
    }

    private static SO_Pokemon.Move ToMove(PokeAPI_Pokemon.PokeAPI_Moves moves)
    {
        var move = moves.version_group_details.Find(y => y.version_group.name == VERSION_GROUP);

        var name = moves.move.name;

        return new SO_Pokemon.Move() { learn_level = move.level_learned_at, name = moves_so[name] };
    }

    private static async Task PopulateAllMoveTargets()
    {
        var result = await NetworkTask<PokeAPI_Result>.Get(URL_MOVE_TARGET);

        foreach (var item in result.results)
        {
            var moveTargetApi = await NetworkTask<PokeAPI_MoveTarget>.Get(item.url);

            m_moveTargets.Add(item.name, moveTargetApi);
        }

        while (m_moveTargets.Count != result.results.Count)
            await Task.Delay(1);

        m_moveTargets_so.Clear();

        foreach (var item in m_moveTargets)
        {
            var so_moveTarget = ScriptableObject.CreateInstance<SO_MoveTarget>();

            m_moveTargets_so.Add(item.Key, so_moveTarget);
        }

        foreach (var moveTarget_so in m_moveTargets_so)
        {
            var so_moveTarget = moveTarget_so.Value;

            var moveTargetApi = m_moveTargets[moveTarget_so.Key];

            so_moveTarget.id = moveTargetApi.id;

            so_moveTarget.name = moveTargetApi.name;

            so_moveTarget.description = moveTargetApi.descriptions.Find(x => x.language.name == LANGUAGE)?.description;

            var path = ResourcesMoveTargetFilename($"{so_moveTarget.id}. {so_moveTarget.name}");

            path.CreateResourcesFolder();

            AssetDatabase.CreateAsset(so_moveTarget, path);
        }
    }

    public static void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(Editor));

        var type = assembly.GetType("UnityEditor.LogEntries");

        var method = type.GetMethod("Clear");

        method.Invoke(new object(), null);
    }

    private static async Task SaveTextures(PokeAPI_Pokemon pokemon)
    {
        var id = pokemon.id;

        var sprites = pokemon.sprites.versions.generation_i.red_blue;

        await Task.WhenAll(
            SaveTexture(id, sprites.back_default, "backDefault"),
            SaveTexture(id, sprites.front_default, "frontDefault")
        );
    }

    private static async Task SaveTexture(int id, string url, string filename)
    {
        if (url.IsEmpty())
            return;

        var texture = await NetworkTask<Texture2D>.GetTexture(url);

        SaveTexture(ResourcesSpriteFilename(id, filename), texture);
    }

    private static void SaveTexture(string path, Texture2D texture)
    {
        path.CreateResourcesFolder();

        File.WriteAllBytes(path, texture.EncodeToPNG());

        AssetDatabase.ImportAsset(path);

        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);

        importer.isReadable = true;
        importer.textureType = TextureImporterType.Sprite;

        TextureImporterSettings importerSettings = new();
        importer.ReadTextureSettings(importerSettings);
        importerSettings.spriteExtrude = 0;
        importerSettings.spriteGenerateFallbackPhysicsShape = false;
        importerSettings.spriteMeshType = SpriteMeshType.Tight;
        importerSettings.spriteMode = (int)SpriteImportMode.Single;
        importer.SetTextureSettings(importerSettings);

        importer.spriteImportMode = SpriteImportMode.Single;
        importer.maxTextureSize = 2048;
        importer.alphaIsTransparency = true;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.alphaSource = TextureImporterAlphaSource.FromInput;
        importer.wrapMode = TextureWrapMode.Repeat;
        importer.filterMode = FilterMode.Point;

        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}

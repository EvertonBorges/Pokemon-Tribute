using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

[Serializable]
public class PokeAPI_Pokemon
{
    public int id;
    public int height;
    public List<PokeAPI_Moves> moves;
    public string name;
    public List<PokeAPI_PastTypes> past_types;
    public PokeAPI_Sprites sprites;
    public PokeAPI_NameUrl species;
    public List<PokeAPI_Stats> stats;
    public List<PokeAPI_Types> types;
    public int weight;

    public class PokeAPI_Moves
    {
        public PokeAPI_NameUrl move;
        public List<PokeAPI_VersionGroupDetails> version_group_details;

        public class PokeAPI_VersionGroupDetails
        {
            public int level_learned_at;
            public PokeAPI_NameUrl move_learn_method;
            public PokeAPI_NameUrl version_group;
        }
    }

    public class PokeAPI_Sprites
    {
        public string back_default;
        public string back_female;
        public string back_shiny;
        public string back_shiny_female;
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
        public PokeAPI_Versions versions;

        public class PokeAPI_Versions
        {
            [JsonProperty("generation-i")] public PokeAPI_SpriteGenerationI generation_i;

            public class PokeAPI_SpriteGenerationI
            {
                [JsonProperty("red-blue")] public PokeAPI_SpriteGeneration red_blue;
                public PokeAPI_SpriteGeneration yellow;
            }

            public class PokeAPI_SpriteGeneration
            {
                public string back_default;
                public string back_gray;
                public string back_transparent;
                public string front_default;
                public string front_gray;
                public string front_transparent;
            }
        }
    }

    public class PokeAPI_Stats
    {
        public int base_stat;
        public int effort;
        public PokeAPI_NameUrl stat;
    }

    public class PokeAPI_Types
    {
        public int slot;
        public PokeAPI_NameUrl type;
    }

    public class PokeAPI_PastTypes
    {
        public PokeAPI_NameUrl generation;
        public List<PokeAPI_Types> types;
    }

    public override string ToString()
    {
        StringBuilder result = new();

        var moves = this.moves.Select(x => x.move.name).ToList().ToSingleString();
        var stats = this.stats.Select(x => x.stat.name).ToList().ToSingleString();
        var types = this.types.Select(x => x.type.name).ToList().ToSingleString();

        var movesRedBlue = 
            this.moves
                .FindAll(x => x.version_group_details.Any(y => y.version_group.name == "red-blue" && y.move_learn_method.name == "level-up"))
                .OrderBy(x => x.move.name)
                .OrderBy(x => x.version_group_details.Find(y => y.version_group.name == "red-blue").level_learned_at)
                .Select(x => $"{x.version_group_details.Find(y => y.version_group.name == "red-blue").level_learned_at}. {x.move.name}")
                .ToList()
                .ToSingleString();

        result.AppendLine($"{id}: {name}");
        result.AppendLine($"Height: {height} / Weight: {weight}");
        result.AppendLine($"Moves: {moves}");
        result.AppendLine($"Moves Red-Blue: {movesRedBlue}");
        result.AppendLine($"Stats: {stats}");
        result.AppendLine($"Types: {types}");
        result.AppendLine($"Sprites (front_default): {sprites.front_default}");
        result.AppendLine($"Sprites (generation-i red-blue front_default): {sprites.versions.generation_i.red_blue.front_default}");

        return result.ToString();
    }
}
using System;
using System.Collections.Generic;

[Serializable]
public class PokeAPI_PokemonSpecies
{

    public List<PokeAPI_FlavorTextEntrie> flavor_text_entries;
    public string name;
    public PokeAPI_Url evolution_chain;
    public PokeAPI_NameUrl evolves_from_species;
    public List<PokeAPI_Variety> varieties;

    public class PokeAPI_FlavorTextEntrie
    {
        public string flavor_text;
        public PokeAPI_NameUrl language;
        public PokeAPI_NameUrl version;
    }

    public class PokeAPI_Variety
    {
        public bool is_default;
        public PokeAPI_NameUrl pokemon;
    }

}
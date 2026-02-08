using System;
using System.Collections.Generic;

[Serializable]
public class PokeAPI_EvolutionChain
{
    
    public int id;
    public PokeAPI_Chain chain;

    public class PokeAPI_Chain
    {
        public List<PokeAPI_EvolutionDetails> evolution_details;
        public List<PokeAPI_Chain> evolves_to;
        public bool is_baby;
        public PokeAPI_NameUrl species;
    }

    public class PokeAPI_EvolutionDetails
    {
        public string know_move;
        public int? min_level;

        public PokeAPI_NameUrl trigger;
    }

}

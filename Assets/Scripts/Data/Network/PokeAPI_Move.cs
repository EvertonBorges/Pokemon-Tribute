using System;
using System.Collections.Generic;

[Serializable]
public class PokeAPI_Move
{

    public int id;
    public int? accuracy;
    public List<PokeAPI_FlavorTextEntrie> flavor_text_entries;
    public string name;
    public List<PokeAPI_PastValues> past_values;
    public int? power;
    public int pp;
    public int priority;
    public PokeAPI_NameUrl target;
    public PokeAPI_NameUrl type;
    
    public class PokeAPI_FlavorTextEntrie
    {
        public string flavor_text;
        public PokeAPI_NameUrl language;
        public PokeAPI_NameUrl version_group;
    }

    public class PokeAPI_PastValues
    {
        public int? accuracy;
        public int? power;
        public int? pp;
        public PokeAPI_NameUrl type;
        public PokeAPI_NameUrl version_group;
    }

}

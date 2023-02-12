using System;
using System.Collections.Generic;

[Serializable]
public struct PokeAPI_Type
{

    public int id;
    public string name;
    public PokeAPI_DamageRelation damage_relations;
    public List<PokeAPI_PastDamageRelation> past_damage_relations;

    public struct PokeAPI_DamageRelation
    {
        public List<PokeAPI_NameUrl> double_damage_from;
        public List<PokeAPI_NameUrl> double_damage_to;
        public List<PokeAPI_NameUrl> half_damage_from;
        public List<PokeAPI_NameUrl> half_damage_to;
        public List<PokeAPI_NameUrl> no_damage_from;
        public List<PokeAPI_NameUrl> no_damage_to;
    }

    public struct PokeAPI_PastDamageRelation
    {
        public PokeAPI_DamageRelation damage_relations;
        public PokeAPI_NameUrl generation;
    }
}
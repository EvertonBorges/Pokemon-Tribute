using System;
using System.Collections.Generic;
using UnityEngine;

public class SO_Pokemon : ScriptableObject
{
    
    public int id;
    public new string name;
    public string description;
    public Stats base_stats = new();
    public List<Move> moves = new();
    public List<SO_Type> types = new();
    public List<Evolution> evolve_to = new();

    [Serializable]
    public class Stats
    {
        public int hp;
        public int attack;
        public int defense;
        public int special_attack;
        public int special_defense;
        public int speed;
    }

    [Serializable]
    public class Move
    {
        public int learn_level;
        public SO_Move name;
    }

    [Serializable]
    public class Evolution
    {
        public List<EvolutionCondition> evolutionConditions = new();
        public SO_Pokemon pokemon = null;
    }

    [Serializable]
    public class EvolutionCondition
    {
        public string trigger = null;
        public int min_level = -1;
    }

    public new string ToString => $"{id}. {name}";

}

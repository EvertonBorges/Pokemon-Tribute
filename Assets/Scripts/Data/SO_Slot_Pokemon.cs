using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Tests/Create Pokemon to Test")]
public class SO_Slot_Pokemon : ScriptableObject
{

    [SerializeField] private SO_Pokemon so_pokemon;
    public SO_Pokemon Pokemon => so_pokemon;
    [SerializeField] private int level;
    public int Level => level;
    [SerializeField] private SO_Move[] so_moves;
    public IList<SO_Move> Moves => so_moves.ToList().AsReadOnly();
    public SO_Move RandomMove => Moves[Random.Range(0, Moves.Count)];

    [SerializeField] private int stateExp;
    [SerializeField] private DeterminantValues dv;
    [SerializeField] private bool randomizeDv = false;

    public int lastUp;
    public int hp;

    public int HP => HpCalc();
    public int Attack => StatCalc(so_pokemon.base_stats.attack, dv.Attack);
    public int Defense => StatCalc(so_pokemon.base_stats.defense, dv.Defense);
    public int Special => StatCalc(Mathf.Min(so_pokemon.base_stats.special_attack, so_pokemon.base_stats.special_defense), dv.Special);
    public int Speed => StatCalc(so_pokemon.base_stats.speed, dv.Speed);

    public void Setup()
    {
        dv.SetupValues(randomizeDv);

        hp = HP;

        lastUp = hp;
    }

    public void LoseHp(int value)
    {
        lastUp = hp;

        hp -= value;

        Manager_Events.Battle.UpdateHp.Notify(this);

        if (hp <= 0)
        {
            Manager_Events.Battle.PokemonDefeated.Notify(this);

            hp = 0;
        }
    }

    public void RecoveryHp(int value = int.MaxValue)
    {
        hp += value;

        if (hp > HP)
            hp = HP;
    }

    public void ShowLogStats()
    {
        Debug.Log($"==== Start Stats Pokemon: {so_pokemon.name} ====");
        Debug.Log($"HP: {HP}");
        Debug.Log($"Attack: {Attack}");
        Debug.Log($"Defense: {Defense}");
        Debug.Log($"Special: {Special}");
        Debug.Log($"Speed: {Speed}");
        Debug.Log($"==== End Stats Pokemon: {so_pokemon.name} ====");
    }

    public int HpCalc()
    {
        var hp = (so_pokemon.base_stats.hp + dv.HP) * 2;
        hp += Mathf.FloorToInt(Mathf.FloorToInt(Mathf.Sqrt(stateExp)) / 4f);
        hp = Mathf.FloorToInt(hp * Level / 100f);
        hp += Level + 10;

        return hp;
    }

    public int StatCalc(int baseStat, int dv)
    {
        var stat = (baseStat + dv) * 2;
        stat += Mathf.FloorToInt(Mathf.FloorToInt(Mathf.Sqrt(stateExp)) / 4);
        stat = Mathf.FloorToInt(stat * Level / 100);
        stat += 5;

        return stat;
    }

    public static List<AttackDatas> AttackOrder(params AttackDatas[] attackers)
    {
        return attackers.OrderByDescending(x => x.attacker.Speed).OrderByDescending(x => x.move.priority).ToList();
    }

    public DamageDatas DamageDataCalc(SO_Slot_Pokemon defender, SO_Move move)
    {
        var level = (float)Level;
        var critical = Random.Range(0, 100) == 0 ? 2f : 1f;

        var power = move.power <= 1 ? 1f : move.power;
        float A = 1f;
        float D = 1f;
        var STAB = Pokemon.types.Contains(move.type) ? 1.5f : 1f;

        var dType1 = defender.Pokemon.types[0];
        var dType2 = defender.Pokemon.types.Count > 1 ? defender.Pokemon.types[1] : null;

        var T1 = move.type.double_damage_to.Contains(dType1) ? 2f : (move.type.half_damage_to.Contains(dType1) ? 0.5f : (move.type.no_damage_to.Contains(dType1) ? 0f : 1f));
        var T2 = dType2 == null ? 1f : (move.type.double_damage_to.Contains(dType2) ? 2f : (move.type.half_damage_to.Contains(dType2) ? 0.5f : (move.type.no_damage_to.Contains(dType2) ? 0f : 1f)));
        var random = Random.Range(217, 256) / 255f;

        switch (move.damageClass)
        {
            case "physical":
                A = Attack;
                D = defender.Defense;
                break;
            case "special":
                A = Special;
                D = defender.Special;
                break;
        }

        // Debug.Log($"Level: {level}");
        // Debug.Log($"Critical: {critical}");
        // Debug.Log($"Power: {power}");
        // Debug.Log($"A: {A}");
        // Debug.Log($"D: {D}");
        // Debug.Log($"STAB: {STAB}");
        // Debug.Log($"T1: {T1}");
        // Debug.Log($"T2: {T2}");
        // Debug.Log($"Random: {random}");

        var dd = new DamageDatas
        {
            damage = Mathf.FloorToInt((((2 * level * critical / 5) + 2) * power * A / D / 50 + 2) * STAB * T1 * T2 * random),

            critical = critical == 2f
        };

        return dd;
    }

}

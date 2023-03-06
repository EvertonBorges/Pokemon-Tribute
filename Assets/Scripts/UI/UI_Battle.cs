using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Battle : Singleton<UI_Battle>
{

    [SerializeField] private Image CTN_Options;
    [SerializeField] private Image CTN_Attacks;
    [SerializeField] private UI_Battle_Attack[] UBA_items;
    [SerializeField] private Image CTN_AttackType;
    [SerializeField] private Image CTN_Confirm;

    private int m_moveIndex = -1;

    protected override void Init()
    {
        base.Init();

        Setup();
    }

    private void Setup()
    {
        m_moveIndex = -1;

        ShowOptions();
    }

    private void ShowOptions()
    {
        DisableContainers();

        CTN_Options.gameObject.SetActive(true);
    }

    private void ShowAttacks()
    {
        DisableContainers();

        CTN_Attacks.gameObject.SetActive(true);

        CTN_AttackType.gameObject.SetActive(true);
    }

    private void ShowConfirm()
    {
        DisableContainers();

        CTN_Confirm.gameObject.SetActive(true);
    }

    private void DisableContainers()
    {
        CTN_Options.gameObject.SetActive(false);

        CTN_Attacks.gameObject.SetActive(false);

        CTN_AttackType.gameObject.SetActive(false);

        CTN_Confirm.gameObject.SetActive(false);
    }

    public void BTN_Fight()
    {
        LoadAttackInfos();

        ShowAttacks();
    }

    private void LoadAttackInfos()
    {
        var pokemon = Manager_Battle.Instance.MyPokemon;

        for (int i = 0; i < UBA_items.Length; i++)
        {
            var item = UBA_items[i];

            var moves = pokemon.Moves;

            var move = i >= moves.Count ? null : moves[i];

            item.Setup(move);
        }
    }

    public void BTN_Pkmn()
    {
        Debug.Log($"BTN_Pkmn");
    }

    public void BTN_Item()
    {
        Debug.Log($"BTN_Item");
    }

    public void BTN_Run()
    {
        Debug.Log($"BTN_Run");
    }

    public void BTN_Attack(int index)
    {
        m_moveIndex = index;

        ShowConfirm();
    }

    public void BTN_Confirm()
    {
        var moves = Manager_Battle.Instance.MyPokemon.Moves;

        if (m_moveIndex >= moves.Count)
            return;

        var myPokemon = Manager_Battle.Instance.MyPokemon;

        var myMove = moves[m_moveIndex];

        var theirPokemon = Manager_Battle.Instance.TheirPokemon;

        var theirMove = Manager_Battle.Instance.TheirPokemon.RandomMove;

        var myAttacker = new AttackDatas(myPokemon, theirPokemon, myMove);

        var theirAttacker = new AttackDatas(theirPokemon, myPokemon, theirMove);

        var attackOrder = SO_Slot_Pokemon.AttackOrder(myAttacker, theirAttacker);

        MonoBehaviourHelper.StartCoroutine(WaitAttacks(attackOrder));
    }

    private IEnumerator WaitAttacks(List<AttackDatas> attackersData)
    {
        DisableContainers();

        foreach (var attackerData in attackersData)
        {
            var attacker = attackerData.attacker;

            var defenser = attackerData.defenser;

            var move = attackerData.move;

            var dd = attacker.DamageDataCalc(defenser, move);

            defenser.LoseHp(dd.damage);

            yield return new WaitForSeconds(1f);
        }

        ShowOptions();
    }

    public void BTN_Cancel()
    {
        ShowOptions();
    }

}

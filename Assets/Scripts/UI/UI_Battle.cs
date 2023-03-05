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

        var move = moves[m_moveIndex];

        var dd = Manager_Battle.Instance.MyPokemon.DamageDataCalc(Manager_Battle.Instance.TheirPokemon, move);

        Debug.Log($"BTN_Confirm: {dd.damage} - {dd.critical}");

        ShowOptions();
    }

    public void BTN_Cancel()
    {
        ShowOptions();
    }

}

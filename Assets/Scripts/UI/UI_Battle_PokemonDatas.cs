using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UI_Battle_PokemonDatas : MonoBehaviour
{

    [SerializeField] private Image IMG_Pokemon;
    [SerializeField] private TextMeshProUGUI TXT_Name;
    [SerializeField] private TextMeshProUGUI TXT_Level;
    [SerializeField] private Slider SLD_Hp;
    [SerializeField] private Image IMG_Hp;
    [SerializeField] private TextMeshProUGUI TXT_Hp;

    [SerializeField] private Color CLR_green;
    [SerializeField] private Color CLR_yellow;
    [SerializeField] private Color CLR_red;

    private SO_Slot_Pokemon m_slotPokemon = null;

    public void Setup(bool myPokemon, SO_Slot_Pokemon slotPokemon)
    {
        m_slotPokemon = slotPokemon;

        slotPokemon.Setup();

        IMG_Pokemon.sprite = myPokemon ? slotPokemon.Pokemon.sprites.backDefault : slotPokemon.Pokemon.sprites.frontDefault;
        TXT_Name.SetText(slotPokemon.Pokemon.name.ToUpper());
        TXT_Level.SetText($"<size=4>:L</size><size=6>{slotPokemon.Level}</size>");

        SLD_Hp.minValue = 0;
        SLD_Hp.maxValue = slotPokemon.HP;
        SLD_Hp.wholeNumbers = false;

        UpdateHp(m_slotPokemon);

        // slotPokemon.ShowLogStats();
    }

    private void UpdateHp(SO_Slot_Pokemon slotPokemon)
    {
        if (m_slotPokemon != slotPokemon)
            return;

        MonoBehaviourHelper.StartCoroutine(WaitUpdateHp(slotPokemon));
    }

    private IEnumerator WaitUpdateHp(SO_Slot_Pokemon slotPokemon)
    {
        float duration = 0.25f;

        float current = 0f;

        while (current <= duration)
        {
            current += Time.deltaTime;

            var proportion = current / duration;

            var hp = Mathf.Lerp(slotPokemon.lastUp, slotPokemon.hp, proportion);

            SLD_Hp.SetValueWithoutNotify(hp);

            IMG_Hp.color = GetImgHpColor(hp, slotPokemon.HP);

            TXT_Hp?.SetText($"{Mathf.FloorToInt(hp)}/ {m_slotPokemon.HP}");

            yield return null;
        }

        SLD_Hp.SetValueWithoutNotify(slotPokemon.hp);

        IMG_Hp.color = GetImgHpColor(slotPokemon.hp, slotPokemon.HP);

        TXT_Hp?.SetText($"{slotPokemon.hp}/ {m_slotPokemon.HP}");
    }

    private Color GetImgHpColor(float hp, int totalHp)
    {
        var totalHpFloat = (float) totalHp;

        if (hp > 27f / 48f * totalHpFloat)
            return CLR_green;
        else if (hp > 10f / 48f * totalHp)
            return CLR_yellow;
        else
            return CLR_red;
    }

    private void OnEnable()
    {
        Manager_Events.Battle.UpdateHp += UpdateHp;
    }

    private void OnDisable()
    {
        Manager_Events.Battle.UpdateHp -= UpdateHp;
    }

}

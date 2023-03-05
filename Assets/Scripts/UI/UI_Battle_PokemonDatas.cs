using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Battle_PokemonDatas : MonoBehaviour
{
    
    [SerializeField] private Image IMG_Pokemon;
    [SerializeField] private TextMeshProUGUI TXT_Name;
    [SerializeField] private TextMeshProUGUI TXT_Level;
    [SerializeField] private Slider SLD_Hp;
    [SerializeField] private TextMeshProUGUI TXT_Hp;

    public void Setup(bool myPokemon, SO_Slot_Pokemon slotPokemon)
    {
        slotPokemon.Setup();

        IMG_Pokemon.sprite = myPokemon ? slotPokemon.Pokemon.sprites.backDefault : slotPokemon.Pokemon.sprites.frontDefault;
        TXT_Name.SetText(slotPokemon.Pokemon.name.ToUpper());
        TXT_Level.SetText($"<size=4>:L</size><size=6>{slotPokemon.Level}</size>");

        var hp = slotPokemon.HP;

        SLD_Hp.minValue = 0;
        SLD_Hp.maxValue = hp;
        SLD_Hp.SetValueWithoutNotify(hp);

        TXT_Hp?.SetText($"{hp}/ {hp}");

        slotPokemon.ShowLogStats();
    }

}

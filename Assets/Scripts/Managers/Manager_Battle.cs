using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class Manager_Battle : Singleton<Manager_Battle>
{

    [SerializeField] private LocalizedString wildPokemon;

    [SerializeField] private SO_Slot_Pokemon myPokemon;
    public SO_Slot_Pokemon MyPokemon => myPokemon;
    [SerializeField] private SO_Slot_Pokemon theirPokemon;
    public SO_Slot_Pokemon TheirPokemon => theirPokemon;

    [SerializeField] private UI_Battle_PokemonDatas myPokemonData;
    [SerializeField] private UI_Battle_PokemonDatas theirPokemonData;

    protected override void Init()
    {
        base.Init();

        myPokemonData.gameObject.SetActive(false);

        theirPokemonData.gameObject.SetActive(false);

        var enemyPokemonName = theirPokemon.Pokemon.name;

        var text = wildPokemon.GetLocalizedString(enemyPokemonName);

        Manager_Dialog.Instance.Setup(text, false, Setup);
    }

    private void Setup()
    {
        UI_Battle.Instance.Setup();

        myPokemonData.gameObject.SetActive(true);

        theirPokemonData.gameObject.SetActive(true);

        theirPokemonData.Setup(false, theirPokemon);

        myPokemonData.Setup(true, myPokemon);
    }

}
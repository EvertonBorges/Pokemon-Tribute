using UnityEngine;

public class Manager_Battle : Singleton<Manager_Battle>
{

    [SerializeField] private SO_Slot_Pokemon myPokemon;
    public SO_Slot_Pokemon MyPokemon => myPokemon;
    [SerializeField] private SO_Slot_Pokemon theirPokemon;
    public SO_Slot_Pokemon TheirPokemon => theirPokemon;

    [SerializeField] private UI_Battle_PokemonDatas myPokemonData;
    [SerializeField] private UI_Battle_PokemonDatas theirPokemonData;

    protected override void Init()
    {
        base.Init();

        Manager_Dialog.Instance.Setup();

        myPokemonData.Setup(true, myPokemon);
        
        theirPokemonData.Setup(false, theirPokemon);
    }

}
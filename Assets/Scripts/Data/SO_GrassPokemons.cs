using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GrassPokemons", menuName = "Data/Grass Pokemons", order = 1)]
public class SO_GrassPokemons : ScriptableObject
{
    
    [field: SerializeField] public SerializedDictionary<SO_Pokemon, S_GrassPokemonInfo> Pokemons { get; private set; }

}

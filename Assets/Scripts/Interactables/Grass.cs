using UnityEngine;

public class Grass : MonoBehaviour
{

    [SerializeField] private SO_GrassPokemons _pokemons;
    [SerializeField][Range(0f, 100f)] private float _encounterRate = 35f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerController player))
            return;

        Manager_Events.Player.Events.OnMove += OnPlayerMove;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerController player))
            return;

        Manager_Events.Player.Events.OnMove -= OnPlayerMove;
    }

    private void OnPlayerMove()
    {
        float sort = Random.Range(0f, 100f);

        if (sort <= _encounterRate)
        {
            float counter = 0f;
            sort = Random.Range(0f, 100f);

            foreach (var pokemon in _pokemons.Pokemons)
            {
                counter += pokemon.Value.rate;

                if (sort <= counter)
                {
                    var level = Random.Range(pokemon.Value.minMaxLevel.x, pokemon.Value.minMaxLevel.y + 1);

                    Debug.Log($"Battle with {pokemon.Key.name} on level {level} | (sort={sort}, counter={counter})");

                    return;
                }
            }
        }
    }

}

using UnityEngine;
using AYellowpaper.SerializedCollections;

public class CharacterAnimationController : MonoBehaviour
{
    
    [SerializeField] private SpriteRenderer _sprite;

    [field: SerializeField] public SerializedDictionary<CharacterEnumState, CharacterSpriteController> Animations { get; private set; }

    private DirectionsEnum m_direction = DirectionsEnum.NONE;
    private CharacterEnumState m_state = CharacterEnumState.IDLE;


    public void SetDirection(DirectionsEnum direction)
    {
        if (m_direction == direction)
            return;

        m_direction = direction;

        SetAnimation();
    }

    public void SetState(CharacterEnumState state)
    {
        if (m_state == state)
            return;

        m_state = state;

        SetAnimation();
    }

    public void SetAnimation()
    {
        var animations = Animations[m_state];
        _sprite.sprite = animations.GetSprite(m_direction);
    }

}

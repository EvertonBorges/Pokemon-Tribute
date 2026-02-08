using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSpriteController", menuName = "Data/Character Sprite", order = 0)]
public class CharacterSpriteController : ScriptableObject
{

    [field: SerializeField] public Sprite[] DownSprites { get; private set; }
    [field: SerializeField] public Sprite[] LeftSprites { get; private set; }
    [field: SerializeField] public Sprite[] UpSprites { get; private set; }
    [field: SerializeField] public Sprite[] RightSprites { get; private set; }

    private int m_index = 0;

    public Sprite GetSprite(DirectionsEnum direction)
    {
        m_index++;
        Sprite sprite = null;

        switch (direction)
        {
            case DirectionsEnum.DOWN: sprite = DownSprites[m_index % DownSprites.Length]; break;
            case DirectionsEnum.LEFT: sprite = LeftSprites[m_index % LeftSprites.Length]; break;
            case DirectionsEnum.UP: sprite = UpSprites[m_index % UpSprites.Length]; break;
            case DirectionsEnum.RIGHT: sprite = RightSprites[m_index % RightSprites.Length]; break;
        }

        return sprite;
    }

}

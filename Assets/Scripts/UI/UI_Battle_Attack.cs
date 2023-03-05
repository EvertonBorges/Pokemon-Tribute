using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Battle_Attack : MonoBehaviour
{

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        if (_button == null) _button = GetComponent<Button>();

        if (_text == null) _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Setup(SO_Move move)
    {
        var isMove = move != null;

        _button.interactable = isMove;

        _text.characterSpacing = isMove ? 0f : -40f;

        _text.SetText(isMove ? move.name.Replace("-", " ").ToUpper() : "---");
    }

}

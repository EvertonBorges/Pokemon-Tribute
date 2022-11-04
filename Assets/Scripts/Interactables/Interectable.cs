using UnityEngine;
using UnityEngine.Localization;

public class Interectable : MonoBehaviour, IInteractable
{

    [SerializeField] private LocalizedString[] _keys;

    public void Interact()
    {
        Manager_Dialog.Instance.Setup(_keys[Random.Range(0, _keys.Length)]);
    }
    
}

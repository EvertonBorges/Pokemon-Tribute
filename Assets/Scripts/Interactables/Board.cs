using UnityEngine;
using UnityEngine.Localization;

public class Board : MonoBehaviour, IInteractable
{

    [SerializeField] private LocalizedString _key;

    public void Interact()
    {
        Manager_Dialog.Instance.Setup(_key.GetLocalizedString());
    }
    
}

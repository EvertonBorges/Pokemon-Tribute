using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    [SerializeField] private ScenesEnum _scene = ScenesEnum.SCN_Game;

    public virtual void Interact()
    {
        _scene.LoadSceneAsync();

        // TODO Transite smootly to another scene
    }
    
}

using UnityEngine;

public class DoorExit : MonoBehaviour, IInteractable
{

    [SerializeField] private DirectionsEnum _exitDirection;
    public DirectionsEnum ExitDirection => _exitDirection;

    [SerializeField] private ScenesEnum _scene = ScenesEnum.SCN_Game;

    public virtual void Interact()
    {
        _scene.LoadSceneAsync();

        // TODO Transite smootly to another scene
    }

}

using UnityEngine;

public class DoorExit : MonoBehaviour, IInteractable
{

    [SerializeField] private DirectionsEnum _exitDirection;
    public DirectionsEnum ExitDirection => _exitDirection;

    [SerializeField] private InspectorScene _scene;

    public virtual void Interact()
    {
        _scene.LoadSceneAsync();

        // TODO Transite smootly to another scene
    }

}

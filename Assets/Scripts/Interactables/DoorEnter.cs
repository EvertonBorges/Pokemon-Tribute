using UnityEngine;

public class DoorEnter : MonoBehaviour, IInteractable
{

    [SerializeField] private InspectorScene _scene;

    public virtual void Interact()
    {
        _scene.LoadSceneAsync();

        // TODO Transite smootly to another scene
    }
    
}

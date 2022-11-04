using System.Collections;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    [SerializeField] private InspectorDoorSelector _door;
    [SerializeField] private DirectionsEnum _enterDirection;
    [SerializeField] private DirectionsEnum _exitDirection;
    public InspectorDoorSelector GetDoor => _door;
    public DirectionsEnum EnterDirection => _enterDirection;
    public DirectionsEnum ExitDirection => _exitDirection;

    public virtual void Interact()
    {
        var door = Manager_Resources.Door_Resources.Doors[_door.Path];

        if (door == null)
            return;

        var doorTransite = Manager_Resources.Door_Resources.Doors[door.To.Path];

        if (doorTransite == null)
        {
            Debug.LogError($"Scene path error: {door.To.Path}");

            return;
        }

        MonoBehaviourHelper.StartCoroutine(Transite(doorTransite));
    }

    private IEnumerator Transite(SO_Door door)
    {
        var asyncOperation = door.From.Scene.LoadSceneAsync();

        asyncOperation.allowSceneActivation = false;

        Manager_Events.GameManager.OnTransite.Notify(true, null);

        Manager_Events.GameManager.Pause.Notify();

        var time = Manager_Transitions.Instance.Blocker.FadeInDuration;

        yield return new WaitForSecondsRealtime(time);

        while(asyncOperation.progress < 0.9f)
            yield return null;

        asyncOperation.allowSceneActivation = true;

        Manager_Events.GameManager.Pause.Notify();

        while(asyncOperation.progress < 1f)
            yield return null;

        Manager_Events.GameManager.Pause.Notify();

        var nextDoor = 
            FindObjectsOfType<Door>()
                .ToList()
                .FindAll(x => x.GetDoor.Path == door.From.Path)
                .OrderBy(x => x.transform.position.x)
                .First();

        var positionTeleport = nextDoor.transform.position + (Vector3.down + Vector3.right) * 0.5f;

        var exitDirection = nextDoor.ExitDirection.DirectionToVector3();

        positionTeleport += exitDirection;

        if (exitDirection != Vector3.zero)
            Manager_Events.Player.OnRotate.Notify(exitDirection);

        Manager_Events.Player.OnTeleport.Notify(positionTeleport);

        Manager_Events.GameManager.OnTransite.Notify(false, () => Manager_Events.GameManager.Unpause.Notify());
    }
    
}

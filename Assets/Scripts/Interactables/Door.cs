using System.Collections;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    [SerializeField] private SO_Door _door;
    [SerializeField] private DirectionsEnum _enterDirection;
    [SerializeField] private DirectionsEnum _exitDirection;
    public SO_Door GetDoor => _door;
    public DirectionsEnum EnterDirection => _enterDirection;
    public DirectionsEnum ExitDirection => _exitDirection;

    public virtual void Interact()
    {
        var doorTransite = InspectorDoor.Extensions.Doors[_door.To.Path];

        if (doorTransite == null)
        {
            Debug.LogError($"Scene path error: {_door.To.Path}");

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

        while(asyncOperation.progress < 1f)
            yield return null;

        Manager_Events.GameManager.Pause.Notify();

        var nextDoor = FindObjectsOfType<Door>().First(x => x.GetDoor == door);

        var positionTeleport = nextDoor.transform.position + (Vector3.down + Vector3.right) * 0.5f;

        positionTeleport += nextDoor.ExitDirection.DirectionToVector3();

        Manager_Events.Player.OnTeleport.Notify(positionTeleport);

        Manager_Events.GameManager.OnTransite.Notify(false, () => Manager_Events.GameManager.Unpause.Notify());
    }
    
}

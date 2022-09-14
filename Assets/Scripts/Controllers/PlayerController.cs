using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private void OnMovement(Vector2 value)
    {
        Debug.Log($"Movement: {value}");
    }

    private void OnButtonA()
    {
        Debug.Log("OnButtonA");
    }

    private void OnButtonB()
    {
        Debug.Log("OnButtonB");
    }

    private void OnButtonPause()
    {
        Debug.Log("OnButtonPause");
    }

    private void OnButtonSelect()
    {
        Debug.Log("OnButtonSelect");
    }

    void OnEnable()
    {
        Manager_Events.Player.OnMovement += OnMovement;

        Manager_Events.Player.OnButtonA += OnButtonA;
        Manager_Events.Player.OnButtonB += OnButtonB;
        Manager_Events.Player.OnButtonPause += OnButtonPause;
        Manager_Events.Player.OnButtonSelect += OnButtonSelect;
    }

    void OnDisable()
    {
        Manager_Events.Player.OnMovement -= OnMovement;

        Manager_Events.Player.OnButtonA -= OnButtonA;
        Manager_Events.Player.OnButtonB -= OnButtonB;
        Manager_Events.Player.OnButtonPause -= OnButtonPause;
        Manager_Events.Player.OnButtonSelect -= OnButtonSelect;
    }

}

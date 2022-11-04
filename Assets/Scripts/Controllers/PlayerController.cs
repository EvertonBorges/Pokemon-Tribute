using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _speed = 5f;
    [SerializeField] private Transform _body;
    [SerializeField] private LayerMask _notWalkableLayerMask;
    [SerializeField] private LayerMask _interactableLayerMask;
    [SerializeField] private LayerMask _doorLayerMask;

    private Vector2 m_move = Vector2.zero;

    private Coroutine m_moveCoroutine = null;

    private Vector2 nextPosition = Vector2.zero;
    private Vector2 m_boxCheckSize = new(-0.1f, 0.1f);

    private bool IsBlocked => GameManager.Instance.IsBlocked;

    private void Update()
    {
        if (IsBlocked)
            return;

        if (m_move == Vector2.zero)
            return;

        if (m_moveCoroutine != null)
            return;

        Move();
    }

    private void Move()
    {
        Vector2 move = MoveDirection();

        if (move == Vector2.zero)
            return;

        Vector2 position = transform.position;

        var pointA = position + m_boxCheckSize;

        var pointB = position - m_boxCheckSize;

        Rotate(move);

        var collider = Physics2D.OverlapArea(pointA, pointB, _doorLayerMask);

        if (collider != null && collider.TryGetComponent(out Door door) && 
            door.EnterDirection != DirectionsEnum.NONE && move.Vector2ToDirection() == door.EnterDirection)
        {
            door.Interact();

            return;
        }

        nextPosition = position + move * 0.5f;

        pointA = nextPosition + m_boxCheckSize;

        pointB = nextPosition - m_boxCheckSize;

        if (Physics2D.OverlapArea(pointA, pointB, _notWalkableLayerMask))
            return;

        m_moveCoroutine = StartCoroutine(Move(move));
    }

    private Vector2 MoveDirection()
    {
        Vector2 move = Vector2.zero;

        if (m_move.y != 0f)
            if (m_move.y < 0f)
                move = Vector2.down;
            else
                move = Vector2.up;
        else if (m_move.x != 0f)
            if (m_move.x > 0f)
                move = Vector2.right;
            else
                move = Vector2.left;

        return move;
    }

    private IEnumerator Move(Vector2 direction, float duration = 1f)
    {
        Vector2 from = transform.position;

        Vector2 to = from + direction;

        transform.position = from;

        float currentTime = 0f;

        float realDuration = duration / _speed;

        while (currentTime <= realDuration)
        {
            currentTime += Time.deltaTime;

            float proportion = currentTime / realDuration;

            Vector2 position = Vector2.Lerp(from, to, proportion);

            transform.position = position;

            yield return null;
        }

        transform.position = to;

        var collider = Physics2D.OverlapPoint(to, _doorLayerMask);

        if (collider && collider.TryGetComponent(out Door door) && door.EnterDirection == DirectionsEnum.NONE)
            door.Interact();

        m_moveCoroutine = null;
    }

    private void Teleport(Vector2 value)
    {
        transform.position = value;
    }

    private void Rotate(Vector2 direction)
    {
        var angle = Vector2.SignedAngle(Vector2.up, direction);

        _body.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnMovement(Vector2 value)
    {
        m_move = value;
    }

    private void OnButtonA()
    {
        if (IsBlocked)
        {
            if (Manager_Dialog.Instance.IsShowing)
                Manager_Dialog.Instance.NextLine();

            return;
        }

        var collider = Physics2D.OverlapArea(nextPosition + m_boxCheckSize, nextPosition - m_boxCheckSize, _interactableLayerMask);

        if (collider == null || !collider.TryGetComponent(out IInteractable interactable))
            return;

        interactable.Interact();
    }

    private void OnButtonB()
    {
        if (IsBlocked)
        {
            if (Manager_Dialog.Instance.IsShowing)
                Manager_Dialog.Instance.NextLine();

            return;
        }
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
        Manager_Events.Player.OnTeleport += Teleport;
        Manager_Events.Player.OnRotate += Rotate;

        Manager_Events.Player.OnMovement += OnMovement;
        Manager_Events.Player.OnButtonA += OnButtonA;
        Manager_Events.Player.OnButtonB += OnButtonB;
        Manager_Events.Player.OnButtonPause += OnButtonPause;
        Manager_Events.Player.OnButtonSelect += OnButtonSelect;
    }

    void OnDisable()
    {
        Manager_Events.Player.OnTeleport -= Teleport;
        Manager_Events.Player.OnRotate -= Rotate;

        Manager_Events.Player.OnMovement -= OnMovement;
        Manager_Events.Player.OnButtonA -= OnButtonA;
        Manager_Events.Player.OnButtonB -= OnButtonB;
        Manager_Events.Player.OnButtonPause -= OnButtonPause;
        Manager_Events.Player.OnButtonSelect -= OnButtonSelect;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawCube(nextPosition, Vector3.one * 0.25f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _speed = 5f;
    [SerializeField] private Transform _body;
    [SerializeField] private LayerMask _notWalkableLayerMask;
    [SerializeField] private LayerMask _interactableLayerMask;

    private Vector2 m_move = Vector2.zero;

    private Coroutine m_moveCoroutine = null;

    private Vector2 nextPosition = Vector2.zero;
    private Vector2 m_boxCheckSize = new(-0.1f, 0.1f);

    private void Update()
    {
        if (m_move == Vector2.zero)
            return;

        if (m_moveCoroutine != null)
            return;

        Move();
    }

    private void Move()
    {
        Vector2 move = Vector2.zero;

        if (m_move.x > 0f)
            move = Vector2.right;
        else if (m_move.y < 0f)
            move = Vector2.down;
        else if (m_move.x < 0f)
            move = Vector2.left;
        else if (m_move.y > 0f)
            move = Vector2.up;

        if (move == Vector2.zero)
            return;

        Vector2 position = transform.position;

        nextPosition = position + move * 0.5f;

        var angle = Vector2.SignedAngle(Vector2.up, move);

        _body.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Physics2D.OverlapArea(nextPosition + m_boxCheckSize, nextPosition - m_boxCheckSize, _notWalkableLayerMask))
            return;

        m_moveCoroutine = StartCoroutine(Move(move));
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

        m_moveCoroutine = null;
    }

    private void OnMovement(Vector2 value)
    {
        m_move = value;
    }

    private void OnButtonA()
    {
        var collider = Physics2D.OverlapArea(nextPosition + m_boxCheckSize, nextPosition - m_boxCheckSize, _interactableLayerMask);

        if (collider == null || !collider.TryGetComponent(out IInteractable interactable))
            return;

        interactable.Interact();
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

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawCube(nextPosition, Vector3.one * 0.25f);
    }

}

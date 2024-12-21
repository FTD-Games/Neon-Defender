using System;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public event Action<bool> onDirectionChanged;

    private Transform _target;
    private float _speed;
    private bool _isMovingRight;
    /// <summary>
    /// Is the input orientated to the right or left?
    /// </summary>
    public bool IsMovingRight
    {
        get { return _isMovingRight; }
        set
        {
            if (_isMovingRight != value)
                DirectionHasChanged(value);
            _isMovingRight = value;
        }
    }
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var move = new Vector2(Mathf.Clamp(_target.position.x - transform.position.x, -1, 1), Mathf.Clamp(_target.position.y - transform.position.y, -1, 1));
        Move(move);
    }

    public void Move(Vector2 move)
    {
        IsMovingRight = move.x >= 0;
        Debug.Log($"MONSTER: speed: {_speed} movement: {move}");
        _rb.MovePosition(_rb.position + _speed * move * Time.fixedDeltaTime);
    }

    public void SetupFollowTarget(Transform newTarget, float speed)
    {
        _target = newTarget;
        _speed = speed;
        Debug.Log($"SPEED SET TO: {_speed}");
        IsMovingRight = true;
    }

    /// <summary>
    /// Fires the event that the direction has changed.
    /// </summary>
    private void DirectionHasChanged(bool isMovingRight) => onDirectionChanged?.Invoke(isMovingRight);
}

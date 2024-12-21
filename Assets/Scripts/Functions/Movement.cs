using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public event Action<bool> onDirectionChanged;

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
    private float _speed;
    private Rigidbody2D _rb;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 move)
    {
        var moveMent = new Vector2(Mathf.Clamp(move.x, -1, 1), Mathf.Clamp(move.y, -1, 1));
        IsMovingRight = moveMent.x >= 0;
        Debug.Log($"PLAYER: speed: {_speed} movement: {moveMent}");
        _rb.MovePosition(_rb.position + _speed * moveMent * Time.fixedDeltaTime);
    }

    public void SetupMovement(float speed)
    {
        _speed = speed;
        IsMovingRight = true;
    }

    /// <summary>
    /// Fires the event that the direction has changed.
    /// </summary>
    private void DirectionHasChanged(bool isMovingRight) => onDirectionChanged?.Invoke(isMovingRight);
}

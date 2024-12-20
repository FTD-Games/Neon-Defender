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

    public void Move(Vector2 move)
    {
        IsMovingRight = move.x > 0;
        transform.position = new Vector2(transform.position.x + (_speed * move.x * Time.deltaTime), transform.position.y + (_speed * move.y * Time.deltaTime));
    }

    public void SetupMovement(float speed)
    {
        _speed = speed;
    }

    /// <summary>
    /// Fires the event that the direction has changed.
    /// </summary>
    private void DirectionHasChanged(bool isMovingRight) => onDirectionChanged?.Invoke(isMovingRight);
}

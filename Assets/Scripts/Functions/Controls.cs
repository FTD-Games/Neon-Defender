using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    private PlayerControls _currentControls;
    private InputAction _move { get; set; }

    private void Awake()
    {
        _currentControls = new PlayerControls();
        _currentControls.Enable();
        _move = _currentControls.Player.Move;
    }

    public bool IsMoving() => _move.inProgress;

    public Vector2 GetMovement() => _move.ReadValue<Vector2>();
}

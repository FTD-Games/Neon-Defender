using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    private PlayerControls _currentControls;
    private InputAction _move { get; set; }
    private InputAction _esc { get; set; }

    private void Awake()
    {
        _currentControls = new PlayerControls();
        _currentControls.Enable();
        _move = _currentControls.Player.Move;
        _esc = _currentControls.Player.Escape;
    }

    public bool IsMoving() => _move.inProgress;

    public bool PressedEsc() => _esc.triggered;

    public Vector2 GetMovement() => _move.ReadValue<Vector2>();
}

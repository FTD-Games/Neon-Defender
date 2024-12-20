using UnityEngine;

public class Player : MonoBehaviour
{
    private Controls _controls;
    private Movement _movement;

    // Visual links
    public SpriteRenderer body;

    private void Start()
    {
        SetupCamera();
        SetupControls();
        SetupMovement();
    }

    private void Update()
    {
        if (_controls.IsMoving())
            _movement.Move(_controls.GetMovement());
    }

    /// <summary>
    /// Initializes the camera to the player and let the camera follow the player.
    /// </summary>
    private void SetupCamera()
    {
        var cam = Camera.main.GetComponent<CameraController>();
        cam.SetNewTarget(gameObject);
        cam.ControlFollowing(true);
    }

    /// <summary>
    /// Initializes the controls of the player.
    /// </summary>
    private void SetupControls()
    {
        _controls = GetComponent<Controls>();
    }

    /// <summary>
    /// Initializes the movement controlling for the player.
    /// </summary>
    private void SetupMovement()
    {
        _movement = GetComponent<Movement>();
        _movement.SetupMovement(5f);
        _movement.onDirectionChanged += MovementDirectionChanged;
    }

    /// <summary>
    /// Do all the needed disposing / deleting stuff here before change the scene or remove the player.
    /// </summary>
    private void DisposePlayer()
    {
        _movement.onDirectionChanged -= MovementDirectionChanged;
    }

    /// <summary>
    /// Called by the movement as event when the players direction has changed.
    /// </summary>
    private void MovementDirectionChanged(bool isMovingRight)
    {
        Debug.Log("direction changed");
        body.flipX = !isMovingRight;
    }
}

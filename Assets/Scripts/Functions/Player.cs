using UnityEngine;

public class Player : MonoBehaviour
{
    private Controls _controls;
    private Movement _movement;
    private BaseStats _stats;
    private GameObject _myWeapon;
    private HealthDisplay _healthDisplay;

    // Prefabs
    public GameObject startWeapon;

    // Visual links
    public SpriteRenderer body;

    private void Start()
    {
        SetupBaseStats();
        SetupCamera();
        SetupControls();
        SetupMovement();
        SetupWeapon();
        SetupHealthDisplay();
        SetupSpawner();
    }

    private void FixedUpdate()
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
        _movement.SetupMovement(_stats.Speed);
        _movement.onDirectionChanged += MovementDirectionChanged;
    }

    /// <summary>
    /// Initializes the basestats for the player.
    /// </summary>
    private void SetupBaseStats()
    {
        _stats = GetComponent<BaseStats>();
        _stats.SetBaseStats();
    }

    /// <summary>
    /// Initializes the start weapon for the player.
    /// </summary>
    private void SetupWeapon()
    {
        _myWeapon = Instantiate(startWeapon, transform);
        _myWeapon.GetComponent<Sword>().SetupSword(1.25f, _stats.Damage, 100f);
    }

    /// <summary>
    /// Initializes the healh display for the player
    /// </summary>
    private void SetupHealthDisplay()
    {
        _healthDisplay = GetComponentInChildren<HealthDisplay>();
        _healthDisplay.SetHealth(_stats.MaxHealth, _stats.Health);
    }

    /// <summary>
    /// Initializes the spawner that brings in the MONSTERS & BOSSES
    /// </summary>
    private void SetupSpawner()
    {
        GetComponent<SpawnController>().SetupSpawner(transform);
    }

    /// <summary>
    /// Do all the needed disposing / deleting stuff here before change the scene or remove the player.
    /// </summary>
    private void DisposePlayer()
    {
        _movement.onDirectionChanged -= MovementDirectionChanged;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var layer = collision.gameObject.layer;
        switch (layer)
        {
            case 9:
                var expOrb = collision.gameObject.GetComponent<ExpOrb>();
                if (Vector3.Distance(transform.position, collision.transform.position) >= 1) {
                    expOrb.SetFollowTarget(transform);
            } else {
                    Debug.Log($"GIMME EXP: {expOrb.GetExperience()}");
                    Destroy(expOrb.gameObject);
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Called by the movement as event when the players direction has changed.
    /// </summary>
    private void MovementDirectionChanged(bool isMovingRight) => body.flipX = !isMovingRight;
}

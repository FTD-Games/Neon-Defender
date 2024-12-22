using UnityEngine;

public class Player : MonoBehaviour
{
    private Controls _controls;
    private Movement _movement;
    private BaseStats _stats;
    private GameObject _myWeapon;
    private HealthDisplay _healthDisplay;
    private Hud _hud;
    private ObjectPooling _pools;
    private AudioDataCollection _sounds;

    // Prefabs
    public GameObject startWeapon;

    // Visual links
    public SpriteRenderer body;

    private void Start()
    {
        _pools = GameControl.control.GetComponent<ObjectPooling>();
        _hud = GameControl.control.CurrentHUD.GetComponent<Hud>();
        SetupSoundPlayer();
        SetupBaseStats();
        SetupCamera();
        SetupControls();
        SetupMovement();
        SetupWeapon();
        SetupHealthDisplay();
        SetupSpawner();
        SetupLevelDisplays();
    }

    private void Update()
    {
        if (_controls.PressedEsc())
            SetPause();
    }

    private void FixedUpdate()
    {
        if (_controls.IsMoving())
            _movement.Move(_controls.GetMovement());
    }

    private void SetupSoundPlayer()
    {
        _sounds = GetComponent<AudioDataCollection>();
        _sounds.InitAudio();
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
        _stats.onLevelUp += LevelUp;
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
        GetComponent<SpawnController>().SetupSpawner(transform, _hud);
    }

    /// <summary>
    /// Show the current exp, needed exp and level on the hud.
    /// </summary>
    private void SetupLevelDisplays()
    {
        _hud.RefreshLevel(_stats.Level);
        _hud.RefreshExpProgress(_stats.CurrentExperience, _stats.CurrentNeededExperience);
    }

    private void SetPause() => _hud.SetPause();

    /// <summary>
    /// Do all the needed disposing / deleting stuff here before change the scene or remove the player.
    /// </summary>
    private void DisposePlayer()
    {
        _movement.onDirectionChanged -= MovementDirectionChanged;
        _stats.onLevelUp -= LevelUp;
    }

    private void ReceiveDamage(float value)
    {
        _stats.Health -= value;
        _healthDisplay.SetHealth(_stats.MaxHealth, _stats.Health);
        if (_stats.Health <= 0)
        {
            SetDead();
            return;
        }
        _sounds.PlayTargetSoundWithRandomPitch(Enums.E_Sound.GetHit);
    }

    private void ReceiveHealth(float value)
    {
        _stats.Health += value;
    }

    private void SetDead()
    {
        SetPause();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var layer = collision.gameObject.layer;
        switch (layer)
        {
            case 9:
                if (collision.gameObject.TryGetComponent(out DmgDealer dealer))
                    ReceiveDamage(dealer.Damage);
                break;
            case 10:
                if (!collision.gameObject.TryGetComponent(out ExpOrb orb))
                    return;

                if (Vector3.Distance(transform.position, collision.transform.position) >= 1)
                {
                    orb.SetFollowTarget(transform);
                    return;
                }
                _sounds.PlayTargetSoundWithRandomPitch(Enums.E_Sound.Collect);
                _stats.CurrentExperience += orb.GetExperience();
                _hud.RefreshExpProgress(_stats.CurrentExperience, _stats.CurrentNeededExperience);
                _pools.ReAddToAvailablePool(orb.gameObject, Enums.E_RequestableObject.ExpOrb);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Called by the base stats when the exp reached the needed exp.
    /// </summary>
    private void LevelUp()
    {
        _sounds.PlayTargetSoundWithRandomPitch(Enums.E_Sound.LevelUp);
        _stats.Level++;
        _stats.CurrentExperience = 0;
        _stats.CurrentNeededExperience = Mathf.RoundToInt(_stats.CurrentNeededExperience * 1.5f - (_stats.Level / 10));
        _hud.RefreshLevel(_stats.Level);
        _hud.RefreshExpProgress(_stats.CurrentExperience, _stats.CurrentNeededExperience);
    }

    /// <summary>
    /// Called by the movement as event when the players direction has changed.
    /// </summary>
    private void MovementDirectionChanged(bool isMovingRight) => body.flipX = !isMovingRight;
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Controls _controls;
    private Movement _movement;
    private BaseStats _stats;
    private List<GameObject> _weapons = new List<GameObject>();
    private HealthDisplay _healthDisplay;
    private Hud _hud;
    private ObjectPooling _pools;
    private AudioDataCollection _sounds;

    // Prefabs
    [SerializeField]
    private Enums.E_Weapon startWeapon;

    // Visual links
    public SpriteRenderer body;

    // Functional links
    [SerializeField]
    private CircleCollider2D _followCollectArea;

    private void Start()
    {
        _pools = GameControl.control.GetComponent<ObjectPooling>();
        _hud = GameControl.control.CurrentHUD.GetComponent<Hud>();
        _hud.SetupRewardDisplay(RewardTaken);
        SetupSoundPlayer();
        SetupBaseStats();
        SetupCamera();
        SetupControls();
        SetupMovement();
        SetupStartWeapon();
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
        _stats.AssignHud(_hud);
        _stats.SetBaseStats();
        _stats.onLevelUp += LevelUp;
    }

    /// <summary>
    /// Initializes the start weapon for the player.
    /// </summary>
    private void SetupStartWeapon()
    {
        var wep = Instantiate(GameControl.control.GetTargetWeapon(startWeapon), transform);
        switch (startWeapon)
        {
            case Enums.E_Weapon.Sword:
                if (wep.TryGetComponent(out Sword sword))
                    sword.SetupSword(1.25f, _stats.Damage, 100f, 0);
                _weapons.Add(wep);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Adds additional sword to weapons.
    /// </summary>
    private void AddSwordAmount()
    {
        var newWep = Instantiate(GameControl.control.GetTargetWeapon(Enums.E_Weapon.Sword), transform);
        var allSwords = new List<Sword>();
        foreach (var oneSword in FindAllWeaponsByType(Enums.E_Weapon.Sword))
        {
            allSwords.Add(oneSword.GetComponent<Sword>());
        }
        var highestSequence = allSwords.Max(s => s.SequenceNr);
        var firstSword = allSwords.FirstOrDefault(s => s.SequenceNr == 0);
        if (newWep.TryGetComponent(out Sword sword))
        {
            sword.SetupSword(firstSword.GetSize(), _stats.Damage, firstSword.GetRotationSpeed(), highestSequence + 1);
        }
        allSwords.Add(sword);
        _weapons.Add(newWep);
        RefreshSwordRotations(allSwords);
    }

    private void RefreshSwordRotations(List<Sword> allSwords)
    {
        for (int i = 0; i < allSwords.Count; i++)
        {
            if (i >= allSwords.Count - 1)
                return;
            var rot = allSwords.FirstOrDefault(s => s.SequenceNr == i).gameObject.transform.rotation;
            var currRot = rot.eulerAngles;
            var nextRot = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z + (360f / allSwords.Count));
            allSwords.FirstOrDefault(s => s.SequenceNr == i + 1).gameObject.transform.rotation = Quaternion.Euler(nextRot);
        }
    }

    /// <summary>
    /// Adds additional amount to weapon. (Only if possible)
    /// </summary>
    private void AddAmountToWeapon(Enums.E_Weapon wep)
    {
        if (!HasWeaponOfType(wep) || IsWeaponOnMaxAmount(wep))
            return;

        switch (wep)
        {
            case Enums.E_Weapon.Sword:
                AddSwordAmount();
                break;
        }
    }

    /// <summary>
    /// Is the target weapon on max amount already?
    /// </summary>
    private bool IsWeaponOnMaxAmount(Enums.E_Weapon wep)
    {
        var maxAmount = GameControl.control.weapons.FirstOrDefault(w => w.type == wep).maxAmount;
        return CurrentAmountOfEquippedWeaponType(wep) >= maxAmount;
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

                if (Vector3.Distance(transform.position, collision.transform.position) >= 0.5f)
                {
                    orb.SetFollowTarget(transform, CollectOrb, 0.5f);
                    return;
                }
                CollectOrb(orb);
                break;
            default:
                break;
        }
    }

    private void CollectOrb(ExpOrb orb)
    {
        _sounds.PlayTargetSoundWithRandomPitch(Enums.E_Sound.Collect);
        _stats.CurrentExperience += orb.GetExperience();
        _hud.RefreshExpProgress(_stats.CurrentExperience, _stats.CurrentNeededExperience);
        _pools.ReAddToAvailablePool(orb.gameObject, Enums.E_RequestableObject.ExpOrb);
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
        _hud.SetupLevelUp(_stats.Level);
    }

    /// <summary>
    /// Called by the movement as event when the players direction has changed.
    /// </summary>
    private void MovementDirectionChanged(bool isMovingRight) => body.flipX = !isMovingRight;

    /// <summary>
    /// Called by the reward controller as event after the player took a reward.
    /// </summary>
    private void RewardTaken(GameControl.RewardData reward)
    {
        var rewardType = reward.RewardType();

        if (!HasWeaponOfRewardType(rewardType))
            return;

        var allWeaponsOfType = FindAllWeaponsByRewardType(rewardType);
        switch (rewardType)
        {
            case Enums.E_Reward.SwordAmount:
                AddAmountToWeapon(Enums.E_Weapon.Sword);
                break;
            case Enums.E_Reward.SwordLength:
                foreach (var wep in allWeaponsOfType)
                {
                    if (wep.TryGetComponent(out Sword sword))
                        sword.IncreaseLength();
                }
                break;
            default:
                Debug.LogWarning($"IMPLEMENT REWARD DATA FOR {reward.GetTitle()}");
                break;
        }
        _hud.CloseLevelUp();
    }

    /// <summary>
    /// Checks if any weapon is affected by target reward.
    /// </summary>
    private bool HasWeaponOfRewardType(Enums.E_Reward type) => _weapons.Any(w => w.GetComponent<WeaponIdent>().matchingRewards.Contains(type));

    /// <summary>
    /// Checks if player has weapon by type.
    /// </summary>
    private bool HasWeaponOfType(Enums.E_Weapon type) => _weapons.Any(w => w.GetComponent<WeaponIdent>().type == type);

    /// <summary>
    /// How many amount (weapons) has the player of the target weapon type?
    /// </summary>
    private int CurrentAmountOfEquippedWeaponType(Enums.E_Weapon type) => _weapons.Count(w => w.GetComponent<WeaponIdent>().type == type);

    /// <summary>
    /// Receives the weapons from target reward type.
    /// </summary>
    private List<GameObject> FindAllWeaponsByRewardType(Enums.E_Reward type) => _weapons.Where(w => w.GetComponent<WeaponIdent>().matchingRewards.Contains(type)).ToList();

    /// <summary>
    /// Receives the weapons from target type.
    /// </summary>
    private List<GameObject> FindAllWeaponsByType(Enums.E_Weapon type) => _weapons.Where(w => w.GetComponent<WeaponIdent>().type == type).ToList();
}

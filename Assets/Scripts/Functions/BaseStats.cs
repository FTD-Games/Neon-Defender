using UnityEngine;

public class BaseStats : MonoBehaviour
{
    #region UNITY INSPECTOR VALUES (START VALUES)
    [SerializeField]
    private float health;
    [SerializeField]
    private float armor;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float experience;
    [SerializeField]
    private float critChance;
    [SerializeField]
    private float critDamage;
    #endregion UNITY INSPECTOR VALUES (START VALUES)

    #region REAL TIME VALUES IN-GAME
    private float _health;
    /// <summary>
    /// Current health that can be manipulated.
    /// </summary>
    public float Health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value, 0, health);
        }
    }

    private float _maxHealth;
    /// <summary>
    /// Current max health that can be manipulated.
    /// </summary>
    public float MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    private float _armor;
    /// <summary>
    /// Current armor that can be manipulated.
    /// </summary>
    public float Armor
    {
        get { return _armor; }
        set { _armor = value; }
    }

    private float _speed;
    /// <summary>
    /// Current speed that can be manipulated.
    /// </summary>
    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    private float _damage;
    /// <summary>
    /// Current damage that can be manipulated.
    /// </summary>
    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    private float _critChance;
    /// <summary>
    /// Current crit chance that can be manipulated.
    /// </summary>
    public float CritChance
    {
        get { return _critChance; }
        set { _critChance = value; }
    }

    private float _critDamage;
    /// <summary>
    /// Current crit damage that can be manipulated.
    /// </summary>
    public float CritDamage
    {
        get { return _critDamage; }
        set { _critDamage = value; }
    }
    #endregion REAL TIME VALUES IN-GAME

    private void Start()
    {
        Health = health;
        MaxHealth = health;
        Armor = armor;
        Damage = damage;
        CritChance = critChance;
        CritDamage = critDamage;
        Speed = speed;
    }
}

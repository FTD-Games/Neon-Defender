using System;
using UnityEngine;

public class Monster : EnemyIdent
{
    private bool _isSetup;

    private BaseStats _stats;
    private FollowTarget _followTarget;
    private Animator _animator;
    private ObjectPooling _pool;
    private AudioDataCollection _sounds;

    // Visual links
    public SpriteRenderer[] bodies;
    public GameObject expOrb;

    public void SetupMonster(Transform target, ObjectPooling pool)
    {
        if (_isSetup)
        {
            ResetMonster();
            return;
        }
        _pool = pool;
        SetupBaseStats();
        SetupMovement(target);
        SetupSoundPlayer();
        SetupAttacks();
        _isSetup = true;
    }

    /// <summary>
    /// Reset is used if the monster is already initialized and taken from pooling. (performance)
    /// </summary>
    private void ResetMonster()
    {
        _stats.SetBaseStats();
    }

    /// <summary>
    /// Initializes the basestats for the player.
    /// </summary>
    private void SetupBaseStats()
    {
        _animator = GetComponent<Animator>();
        _stats = GetComponent<BaseStats>();
        _stats.SetBaseStats();
    }

    private void SetupMovement(Transform target)
    {
        _followTarget = GetComponent<FollowTarget>();
        _followTarget.SetupFollowTarget(target, _stats.Speed);
        _followTarget.onDirectionChanged += MovementDirectionChanged;
    }

    private void SetupSoundPlayer()
    {
        _sounds = GetComponent<AudioDataCollection>();
        _sounds.InitAudio();
    }

    /// <summary>
    /// Initializes the attack depending on monster
    /// </summary>
    private void SetupAttacks()
    {
        if (IsBoss())
        {
            switch (boss)
            {
                case Enums.E_Bosses.Sanja:
                    GetComponentInChildren<DmgDealer>().Damage = _stats.Damage;
                    break;
            }
            return;
        }
        switch (monster)
        {
            case Enums.E_Monster.Verox:
                GetComponentInChildren<DmgDealer>().Damage = _stats.Damage;
                break;
        }
    }

    private void ReceiveDamage(float value)
    {
        _stats.Health -= value;
        if (_stats.Health <= 0) {
            SetDead();
            return;
        }
        _sounds.PlayTargetSoundWithRandomPitch(Enums.E_Sound.GetHit);
        _animator.Play("MonsterHurt");
    }

    private void ReceiveHealth(float value)
    {
        _stats.Health += value;
    }

    private void SetDead()
    {
        var newOrb = _pool.GetAvailableObject(Enums.E_RequestableObject.ExpOrb);
        newOrb.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        newOrb.transform.parent = null;
        newOrb.GetComponent<ExpOrb>().OrbType = (Enums.E_ExpOrbType) UnityEngine.Random.Range(0, Enum.GetNames(typeof(Enums.E_ExpOrbType)).Length);
        _pool.ReAddToAvailablePool(gameObject, IsBoss() ? Enums.E_RequestableObject.Boss : Enums.E_RequestableObject.Monster, monster, boss);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var layer = collision.gameObject.layer;
        switch (layer)
        {
            case 8:
                if (collision.gameObject.TryGetComponent(out DmgDealer dealer))
                    ReceiveDamage(dealer.Damage);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Called by the movement as event when the players direction has changed.
    /// </summary>
    private void MovementDirectionChanged(bool isMovingRight)
    {
        foreach (var body in bodies)
        {
            body.flipX = !isMovingRight;
        }
    }
}

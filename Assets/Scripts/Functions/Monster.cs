using UnityEngine;

public class Monster : MonoBehaviour
{
    public Enums.E_Monster monster;
    private BaseStats _stats;
    private FollowTarget _followTarget;

    // Visual links
    public SpriteRenderer body;

    public void SetupMonster(Transform target)
    {
        SetupBaseStats();
        SetupMovement(target);
    }

    /// <summary>
    /// Initializes the basestats for the player.
    /// </summary>
    private void SetupBaseStats()
    {
        _stats = GetComponent<BaseStats>();
        _stats.SetBaseStats();
    }

    private void SetupMovement(Transform target)
    {
        _followTarget = GetComponent<FollowTarget>();
        _followTarget.SetupFollowTarget(target, _stats.Speed);
        _followTarget.onDirectionChanged += MovementDirectionChanged;
    }

    private void ReceiveDamage(float value)
    {
        _stats.Health -= value;
        if (_stats.Health <= 0)
            SetDead();
        Debug.Log($"RECEIVE DMG: {value} my health now: {_stats.Health}");
    }

    private void ReceiveHealth(float value)
    {
        _stats.Health += value;
    }

    private void SetDead()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TRIGGER ENTER");
        var layer = collision.gameObject.layer;
        switch (layer)
        {
            case 8:
                ReceiveDamage(collision.gameObject.GetComponent<DmgDealer>().Damage);
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

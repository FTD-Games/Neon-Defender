using UnityEngine;

public class Monster : MonoBehaviour
{
    private BaseStats _stats;

    private void Start()
    {
        SetupBaseStats();
    }

    /// <summary>
    /// Initializes the basestats for the player.
    /// </summary>
    private void SetupBaseStats()
    {
        _stats = GetComponent<BaseStats>();
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLL ENTER");
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
}

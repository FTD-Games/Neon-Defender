using System;
using UnityEngine;

public class FollowTargetTransform : MonoBehaviour
{
    // Exp orb following
    private float _collectRange;
    private ExpOrb _orb;
    private Action<ExpOrb> onExpOrbCollected;

    private Transform _target;
    private float _speed;
    private bool _initialized;

    private void Update()
    {
        if (!_initialized) return;

        var move = new Vector3(Mathf.Clamp(_target.position.x - transform.position.x, -1, 1), Mathf.Clamp(_target.position.y - transform.position.y, -1, 1));
        Move(move);

        if (Vector3.Distance(_target.position, transform.position) <= _collectRange)
        {
            onExpOrbCollected?.Invoke(_orb);
            onExpOrbCollected = null;
            _orb = null;
        }
    }

    private void Move(Vector3 move) => transform.position = transform.position + (_speed * move) * Time.deltaTime;

    public void SetupFollowTarget(Transform newTarget, float speed)
    {
        _target = newTarget;
        _speed = speed;
        _initialized = true;
    }

    public void SetupFollowOrb(Transform newTarget, float speed, float collectRange, Action<ExpOrb> collectOrb, ExpOrb orb)
    {
        _collectRange = collectRange;
        _target = newTarget;
        _speed = speed;
        _initialized = true;
        onExpOrbCollected += collectOrb;
        _orb = orb;
    }
}

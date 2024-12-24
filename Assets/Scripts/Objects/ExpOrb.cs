using System;
using System.Linq;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public SpriteRenderer Sprite;
    private FollowTargetTransform _followTarget;
    private Enums.E_ExpOrbType _orbType;
    private int _experience;
    private bool _isCollected;

    private void Start()
    {
        _followTarget = GetComponent<FollowTargetTransform>();
    }

    public Enums.E_ExpOrbType OrbType {
        get { return _orbType; }
        set 
        { 
            _orbType = value;
            var orbData = GameControl.control.OrbDataList.FirstOrDefault(o => o.Type == value);
            _isCollected = false;
            transform.localScale = new Vector3(orbData.Size, orbData.Size);
            _experience = orbData.Experience;
            Sprite.color = orbData.Color;
        }
    }

    /// <summary>
    /// Use this only if you want to collect the experience, because this flags the orb as collected.
    /// </summary>
    public int GetExperience()
    {
        _isCollected = true;
        return _experience;
    }

    public void SetFollowTarget(Transform target, Action<ExpOrb> collectOrb, float collectRange) => _followTarget.SetupFollowOrb(target, 5, collectRange, collectOrb, this);

    public bool IsCollected() => _isCollected;
}

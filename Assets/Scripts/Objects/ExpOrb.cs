using System.Linq;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public SpriteRenderer Sprite;
    private FollowTargetTransform _followTarget;
    private Enums.E_ExpOrbType _orbType;
    private int _experience;

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

            transform.localScale = new Vector3(orbData.Size, orbData.Size);
            _experience = orbData.Experience;
            Sprite.color = orbData.Color;
        }
    }

    public int GetExperience() => _experience;

    public void SetFollowTarget(Transform target) => _followTarget.SetupFollowTarget(target, 5);
}

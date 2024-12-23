using UnityEngine;

public class Sword : WeaponIdent
{
    private float _size;
    private DmgDealer _dmgDealer;
    private RotateComponent _rotateComponent;
    public SpriteRenderer sword;
    [SerializeField]
    private BoxCollider2D attackTrigger;

    private void Awake()
    {
        _dmgDealer = GetComponentInChildren<DmgDealer>();
        _rotateComponent = GetComponent<RotateComponent>();
    }

    public void SetupSword(float size, float dmg, float rotateSpeed, int sequenceNr)
    {
        SequenceNr = sequenceNr;
        _size = size;
        sword.size = new Vector2(_size, sword.size.y);
        _dmgDealer.Damage = dmg;
        _rotateComponent.SetNewSpeed(rotateSpeed);
    }

    public void IncreaseLength()
    {
        var lengthToAdd = 0.25f;
        var newSize = sword.size;
        newSize.x += lengthToAdd;
        _size += lengthToAdd;
        sword.size = newSize;
        attackTrigger.size = new Vector2(attackTrigger.size.x + lengthToAdd, attackTrigger.size.y);
        attackTrigger.offset = new Vector2(attackTrigger.offset.x + (lengthToAdd/2), attackTrigger.offset.y);
    }

    public float GetSize() => _size;

    public float GetRotationSpeed() => _rotateComponent.GetSpeed();

}
using UnityEngine;

public class Sword : MonoBehaviour
{
    private float _size;
    private DmgDealer _dmgDealer;
    private RotateComponent _rotateComponent;
    public SpriteRenderer sword;

    private void Awake()
    {
        _dmgDealer = GetComponentInChildren<DmgDealer>();
        _rotateComponent = GetComponent<RotateComponent>();
    }

    public void SetupSword(float size, float dmg, float rotateSpeed)
    {
        sword.size = new Vector2(size, sword.size.y);
        _dmgDealer.Damage = dmg;
        _rotateComponent.SetNewSpeed(rotateSpeed);
    }
}
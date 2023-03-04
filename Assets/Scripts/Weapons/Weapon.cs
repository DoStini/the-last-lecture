using UnityEngine;

public abstract class Weapon : PickableItem
{
    public DamageStrategy damageStrategy;
    public float attackInterval;
    public LayerMask mask;
    public bool automatic = true;

    public Vector3 activePosition;
    public Quaternion activeRotation;
    
    private float _lastAttack;

    public void Attack(Vector3 pointerLocation, int holdTime)
    {
        if (!(_lastAttack + attackInterval < Time.time)) return;
        if (!automatic && holdTime > 0) return;
        
        if (_Attack(pointerLocation))
        {
            _lastAttack = Time.time;
        }
    }

    protected abstract bool _Attack(Vector3 pointerLocation);

    public override void Pick(GameObject parent)
    {
        base.Pick(parent, false);
    }
}

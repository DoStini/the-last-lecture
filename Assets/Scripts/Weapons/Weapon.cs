using System;
using UnityEngine;
using UnityEngine.Animations;

[Serializable]
public class WeaponHoldStyle
{
    public Transform leftGrip;
    public Transform rightGrip;
    public Transform parent;
    public LookAtConstraint lookAtConstraint;
}

public abstract class Weapon : PickableItem
{
    public DamageStrategy damageStrategy;
    public float attackInterval;
    public LayerMask mask;
    public bool automatic = true;

    public Vector3 activePosition;
    public Quaternion activeRotation;
    public RuntimeAnimatorController playerAnimator;
    public WeaponHoldStyle weaponHoldStyle;
    [NonSerialized] public Transform lastParent;
    
    private float _lastAttack;

    public bool Attack(Vector3 pointerLocation, int holdTime)
    {
        if (!(_lastAttack + attackInterval < Time.time)) return false;
        if (!automatic && holdTime > 0) return false;

        if (!_Attack(pointerLocation)) return false;
        
        _lastAttack = Time.time;
        return true;

    }

    protected abstract bool _Attack(Vector3 pointerLocation);

    public override void Pick(GameObject parent)
    {
        base.Pick(parent, false, false);
    }

    public override void Drop()
    {
        base.Drop();
        lastParent = null;
    }
}

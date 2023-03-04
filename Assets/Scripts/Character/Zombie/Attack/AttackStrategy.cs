using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackStrategy : MonoBehaviour
{
    [SerializeField] protected Player attackTarget;
    [SerializeField] protected Zombie zombie;
    [SerializeField] protected float attackRange;
    [SerializeField] protected Weapon weapon;

    protected int holdTime = 0;
    
    protected abstract bool _Shoot();

    public void Shoot()
    {
        float distance = Vector3.Distance(attackTarget.transform.position, transform.position);

        if (distance >  Mathf.Min(attackRange, zombie.viewRange))
        {
            holdTime = 0;
            return;
        }

        bool shot = _Shoot();
        holdTime = shot ? 1 : 0;
    }

    protected Vector3? TargetPoint()
    {
        var weaponTransform = weapon.transform;
        var weaponPosition = weaponTransform.position;

        float distance = Vector3.Distance( weaponPosition, attackTarget.transform.position);

        Vector3 direction = weaponTransform.forward.normalized;
        Vector3 targetPoint = weaponPosition + direction * distance;

        if (Physics.Raycast(targetPoint, Vector3.down, out var hit, float.MaxValue, LayerMask.GetMask("Ground")))
        {
            return hit.point;
        }

        return null;
    }
}

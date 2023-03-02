using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootingStrategy : MonoBehaviour
{
    [SerializeField] protected Player attackTarget;
    [SerializeField] protected float attackRange;
    [SerializeField] protected Weapon weapon;

    public GameObject pointer;
    
    protected abstract void _Shoot();

    public void Shoot()
    {
        float distance = Vector3.Distance(attackTarget.transform.position, transform.position);

        if (distance > attackRange) return;

        _Shoot();
    }

    protected Vector3? TargetPoint()
    {
        var weaponTransform = weapon.transform;
        var weaponPosition = weaponTransform.position;

        float distance = Vector3.Distance( weaponPosition, attackTarget.transform.position);

        Vector3 direction = weaponTransform.forward.normalized;
        Vector3 targetPoint = weaponPosition + direction * distance;

        pointer.transform.position = targetPoint;

        Debug.DrawRay(targetPoint, Vector3.down * 10, Color.magenta);

        if (Physics.Raycast(targetPoint, Vector3.down, out var hit, float.MaxValue, LayerMask.GetMask("Ground")))
        {
            return hit.point;
        }

        return null;
    }
}

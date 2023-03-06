using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackStrategy : MonoBehaviour
{
    private Zombie _zombie; 
    protected Player _attackTarget;
    protected Weapon _weapon;

    [SerializeField] protected float attackRange;

    protected int holdTime = 0;

    private void Start()
    {
        _zombie = GetComponent<Zombie>();
        _attackTarget = _zombie.target;
        _weapon = _zombie.weapon;
    }

    protected abstract bool _Shoot();

    public void Shoot()
    {
        float distance = Vector3.Distance(_attackTarget.transform.position, transform.position);

        if (distance >  Mathf.Min(attackRange, _zombie.viewRange))
        {
            holdTime = 0;
            return;
        }

        bool shot = _Shoot();
        holdTime = shot ? 1 : 0;
    }

    protected Vector3? TargetPoint()
    {
        var weaponTransform = _weapon.transform;
        var weaponPosition = weaponTransform.position;

        float distance = Vector3.Distance( weaponPosition, _attackTarget.transform.position);

        Vector3 direction = weaponTransform.forward.normalized;
        Vector3 targetPoint = weaponPosition + direction * (distance + 0.3f);

        if (Physics.Raycast(targetPoint, Vector3.down, out var hit, float.MaxValue, LayerMask.GetMask("Ground")))
        {
            return hit.point;
        }

        return null;
    }
}

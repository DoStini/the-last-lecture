using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectAttackStrategy : AttackStrategy
{
    protected override bool _Shoot()
    {
        var targetPoint = TargetPoint();

        if (targetPoint == null) return false;

        if (Vector3.Distance(
                new Vector3(targetPoint.Value.x, _attackTarget.transform.position.y, targetPoint.Value.z),
                _attackTarget.transform.position) > _attackTarget.radius) 
            return false;

        _weapon.Attack( targetPoint.Value, holdTime);
        return true;
    }
}

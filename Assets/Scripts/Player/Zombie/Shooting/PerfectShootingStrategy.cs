using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectShootingStrategy : ShootingStrategy
{
    protected override void _Shoot()
    {
        var targetPoint = TargetPoint();

        if (targetPoint == null) return;

        if (Vector3.Distance(
                new Vector3(targetPoint.Value.x, attackTarget.transform.position.y, targetPoint.Value.z),
                attackTarget.transform.position) > attackTarget.radius) return;

        weapon.Attack( targetPoint.Value, 0);
    }
}

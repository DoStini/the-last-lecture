using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAttackStrategy : AttackStrategy
{
    public float waitTime = 0.1f;
    public float delay = 0.5f;
    private float _lastTime = 0;
    private float _lastTarget = 0;
    private Vector3? _targetPoint;

    private void ChooseTarget()
    {
        if (_lastTime + waitTime > Time.time) return;
        _lastTime = Time.time;
        _targetPoint = TargetPoint();
        if (_targetPoint == null) return;
        
        _lastTarget = Time.time;
    }

    protected override bool _Shoot()
    {
        if (_targetPoint == null)
        {
            ChooseTarget();
            return false;
        }
        if (_lastTarget + delay > Time.time) return false;

        _weapon.Attack(_targetPoint.Value, holdTime);
        _targetPoint = null;
        return true;
    }
}

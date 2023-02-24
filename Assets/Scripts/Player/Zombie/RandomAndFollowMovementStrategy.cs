using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RandomAndFollowMovementStrategy : MovementStrategy
{
    [SerializeField] private float interval;
    [SerializeField] private float directionScale;
    
    private float _lastTime;

    private void Start()
    {
        _lastTime = Time.time;
    }

    public override void Move(NavMeshAgent agent)
    {
        if (FollowPlayer(agent)) return;

        if (Time.time < _lastTime + interval) return;

        _lastTime = Time.time;

        Vector3 direction = Random.insideUnitSphere * directionScale;
        NavMesh.SamplePosition(direction, out var hit, directionScale, 1);

        agent.SetDestination(hit.position);
    }
}

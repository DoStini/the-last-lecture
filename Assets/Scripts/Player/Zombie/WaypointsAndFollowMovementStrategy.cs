using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class WaypointsAndFollowMovementStrategy : MovementStrategy
{
    [SerializeField] private List<Transform> waypoints;

    private int _currentWaypoint = 0;

    public override void Move(NavMeshAgent agent)
    {   
        if (FollowPlayer(agent)) return;

        agent.SetDestination(waypoints[_currentWaypoint].position);

        if (agent.remainingDistance > 0.1f) return;

        _currentWaypoint = (_currentWaypoint + 1) % waypoints.Count;
        agent.SetDestination(waypoints[_currentWaypoint].position);
    }
}

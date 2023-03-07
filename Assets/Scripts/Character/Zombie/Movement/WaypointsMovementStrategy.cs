using System.Collections.Generic;
using UnityEngine;

public class WaypointsMovementStrategy : MovementStrategy
{
    [SerializeField] private List<Transform> waypoints;

    private int _currentWaypoint;

    private void Start()
    {
        _Start();
    }

    protected override void _Move()
    {   
        if (FollowPlayer()) return;

        agent.SetDestination(waypoints[_currentWaypoint].position);

        if (agent.remainingDistance > 0.1f) return;

        _currentWaypoint = (_currentWaypoint + 1) % waypoints.Count;
        agent.SetDestination(waypoints[_currentWaypoint].position);
    }
}

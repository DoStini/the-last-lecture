using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class WaypointsAndFollowMovementStrategy : MovementStrategy
{
    [SerializeField] private List<Transform> waypoints;

    private int _currentWaypoint = 0;

    [SerializeField] private Player target;
    [SerializeField] private float range;

    public override void Move(NavMeshAgent agent)
    {   
        if (PlayerInRange(agent))
        {
            agent.SetDestination(target.transform.position);
            return;
        }

        agent.SetDestination(waypoints[_currentWaypoint].position);

        if (agent.remainingDistance > 0.1f) return;

        _currentWaypoint = (_currentWaypoint + 1) % waypoints.Count;
        agent.SetDestination(waypoints[_currentWaypoint].position);
    }

    private bool PlayerInRange(NavMeshAgent agent)
    {
        return Vector3.Distance(target.transform.position, agent.transform.position) < range;
    }
}

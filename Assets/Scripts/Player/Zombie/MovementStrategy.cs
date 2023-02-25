using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public abstract class MovementStrategy : MonoBehaviour
{
    [SerializeField] private Player target;
    [SerializeField] private float viewRange;

    public abstract void Move(NavMeshAgent agent);

    private bool PlayerInViewRange(NavMeshAgent agent)
    {
        return Vector3.Distance(target.transform.position, agent.transform.position) < viewRange;
    }

    protected bool FollowPlayer(NavMeshAgent agent)
    {
        if (!PlayerInViewRange(agent)) return false;

        agent.SetDestination(target.transform.position);
        return true;
    }
}

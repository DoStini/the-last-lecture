using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class MovementStrategy : MonoBehaviour
{
    [SerializeField] private Player target;
    [SerializeField] private float range;

    public abstract void Move(NavMeshAgent agent);

    protected bool PlayerInRange(NavMeshAgent agent)
    {
        return Vector3.Distance(target.transform.position, agent.transform.position) < range;
    }

    protected bool FollowPlayer(NavMeshAgent agent)
    {
        if (PlayerInRange(agent))
        {
            agent.SetDestination(target.transform.position);
            return true;
        }

        return false;
    }
}

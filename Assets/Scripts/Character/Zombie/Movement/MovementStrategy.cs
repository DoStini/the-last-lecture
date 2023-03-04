using UnityEngine;
using UnityEngine.AI;

public abstract class MovementStrategy : MonoBehaviour
{
    [SerializeField] private Player target;
    [SerializeField] private Zombie zombie;
    [SerializeField] private float minRange;
    [SerializeField] protected NavMeshAgent agent;

    private float _originalSpeed;

    protected void _Start()
    {
        _originalSpeed = agent.speed;
    }

    public abstract void Move();

    private bool PlayerInMinRange(float distance)
    {
        return distance < minRange;
    }

    private bool PlayerInViewRange(float distance)
    {
        return distance < zombie.viewRange;
    }

    protected bool FollowPlayer()
    {
        if (!agent.enabled) return true;
        float distance = Vector3.Distance(target.transform.position, agent.transform.position);
        agent.isStopped = false;

        if (!PlayerInViewRange(distance)) return false;
        agent.SetDestination(target.transform.position);

        if (!PlayerInMinRange(distance)) return true;

        agent.isStopped = true;
        RotateTowardsTarget();

        return true;
    }

    private void RotateTowardsTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(target.transform.position - agent.transform.position);
        agent.transform.rotation =
            Quaternion.RotateTowards(agent.transform.rotation, rotation, Time.deltaTime * agent.angularSpeed);
    }
}

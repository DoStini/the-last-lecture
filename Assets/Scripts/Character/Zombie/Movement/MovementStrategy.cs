using UnityEngine;
using UnityEngine.AI;

public abstract class MovementStrategy : MonoBehaviour
{
    private Zombie _zombie;
    private Player _target;
    
    [SerializeField] private float minRange;
    [SerializeField] protected NavMeshAgent agent;

    private float _originalSpeed;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Angle = Animator.StringToHash("Angle");

    protected void _Start()
    {
        _originalSpeed = agent.speed;
        _zombie = GetComponent<Zombie>();
        _target = _zombie.target;
    }

    protected abstract void _Move();

    public void Move()
    {
        _Move();

        var velocity = agent.velocity;
        _zombie.zombieAnimator.SetFloat(Speed, velocity.magnitude);
    }

    private bool PlayerInMinRange(float distance)
    {
        return distance < minRange;
    }

    private bool PlayerInViewRange(float distance)
    {
        return distance < _zombie.viewRange;
    }

    protected bool FollowPlayer()
    {
        if (!agent.enabled) return true;
        float distance = Vector3.Distance(_target.transform.position, agent.transform.position);
        agent.isStopped = false;

        if (!PlayerInViewRange(distance)) return false;
        agent.SetDestination(_target.transform.position);

        if (!PlayerInMinRange(distance)) return true;

        agent.isStopped = true;
        RotateTowardsTarget();

        return true;
    }

    private void RotateTowardsTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(_target.transform.position - agent.transform.position);
        agent.transform.rotation =
            Quaternion.RotateTowards(agent.transform.rotation, rotation, Time.deltaTime * agent.angularSpeed);
    }
}

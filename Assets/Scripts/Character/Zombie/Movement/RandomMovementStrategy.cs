using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RandomMovementStrategy : MovementStrategy
{
    [SerializeField] private float interval;
    [SerializeField] private float directionScale;
    
    private float _lastTime;

    private void Start()
    {
        _Start();
        _lastTime = Time.time;
    }

    protected override void _Move()
    {
        if (FollowPlayer()) return;

        if (Time.time < _lastTime + interval) return;

        _lastTime = Time.time;

        Vector3 direction = Random.insideUnitSphere * directionScale;
        NavMesh.SamplePosition(direction, out var hit, directionScale, 1);

        agent.SetDestination(hit.position);
    }
}

using System;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Character
{
    [SerializeField] private MovementStrategy movementStrategy;
    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        movementStrategy.Move(_navMeshAgent);
    }
}

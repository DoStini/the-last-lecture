using System;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Character
{
    [SerializeField] private MovementStrategy movementStrategy;
    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        Init();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        movementStrategy.Move();

        if (_currentHealth == 0)
        {
            gameObject.SetActive(false);
        }
    }
}

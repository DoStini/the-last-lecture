using System;
using UnityEngine;
using UnityEngine.AI;

public class ZombieImpactHandler : ImpactHandler
{
    private Rigidbody _rigidbody;
    private NavMeshAgent _agent;
    private bool _inImpact;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (impulse.magnitude > 0.2f)
        {
            _agent.isStopped = true;
            _rigidbody.isKinematic = false;
            _inImpact = true;
            
            _rigidbody.velocity = impulse;
        }
        else if (_inImpact)
        {
            _agent.ResetPath();
            _agent.isStopped = false;
            _rigidbody.isKinematic = true;
            _inImpact = false;
        }
        
        impulse = Vector3.Lerp(impulse, Vector3.zero, 5*Time.deltaTime);
    }
}
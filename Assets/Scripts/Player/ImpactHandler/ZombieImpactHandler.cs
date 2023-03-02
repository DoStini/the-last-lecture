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
        if (_impulse.magnitude > 0.2f)
        {
            _agent.isStopped = true;
            _rigidbody.isKinematic = false;
            _inImpact = true;
            
            _rigidbody.velocity = _impulse;
        }
        else if (_inImpact)
        {
            _agent.ResetPath();
            _agent.isStopped = false;
            _rigidbody.isKinematic = true;
            _inImpact = false;
        }
        
        _impulse = Vector3.Lerp(_impulse, Vector3.zero, 5*Time.deltaTime);
    }
}
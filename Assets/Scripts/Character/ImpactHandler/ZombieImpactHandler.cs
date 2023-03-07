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

    private bool IsAgentOnNavMesh()
    {
        Vector3 agentPosition = _agent.transform.position;

        float height = _agent.height * 0.7f;
        if (!NavMesh.SamplePosition(agentPosition, out NavMeshHit hit, height, NavMesh.AllAreas)) return false;
        
        if ((agentPosition.x < hit.position.x + 0.5f && agentPosition.x > hit.position.x - 0.5f) &&
            (agentPosition.z < hit.position.z + 0.5f && agentPosition.z > hit.position.z - 0.5f))
        {
            return agentPosition.y >= hit.position.y;
        }

        return false;
    }

    private void Update()
    {
        if (_impulse.magnitude > 0.5f)
        {
            if (!_inImpact)
            {
                _agent.enabled = false;
                _rigidbody.isKinematic = false;
                _inImpact = true;
            }

            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0) + _impulse;
        }
        else if (_inImpact && IsAgentOnNavMesh())
        {
            _agent.enabled = true;
            _agent.ResetPath();
            _rigidbody.isKinematic = true;
            _inImpact = false;
        }
        
        _impulse = Vector3.Lerp(_impulse, Vector3.zero, 5*Time.deltaTime);
    }
}
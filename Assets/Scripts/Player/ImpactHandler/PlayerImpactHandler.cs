using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerImpactHandler : ImpactHandler
{
    private CharacterController _controller;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (impulse.magnitude > 0.2f)
        {
            _controller.Move(impulse * Time.deltaTime);
        }
        
        impulse = Vector3.Lerp(impulse, Vector3.zero, 5*Time.deltaTime);
    }
}
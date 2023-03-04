using UnityEngine;

public class PlayerImpactHandler : ImpactHandler
{
    private CharacterController _controller;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_impulse.magnitude > 0.2f)
        {
            _controller.Move(_impulse * Time.deltaTime);
        }
        
        _impulse = Vector3.Lerp(_impulse, Vector3.zero, 5*Time.deltaTime);
    }
}
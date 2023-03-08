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
            Vector3 movement = new Vector3(_impulse.x, -0.8f, _impulse.z);
            _controller.Move(movement * Time.deltaTime);
        }
        
        _impulse = Vector3.Lerp(_impulse, Vector3.zero, 5*Time.deltaTime);
    }
}
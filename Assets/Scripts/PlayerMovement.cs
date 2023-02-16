using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speedFactor = 6f;
    public float gravityFactor = 9.8f;

    private PlayerInputActions _playerActions;
    private InputAction _moveAction;
    private float _vSpeed;
    private float _lookAngle;
    private Vector2 _actionDirection = Vector2.zero;

    public void UpdateLookAngle(Vector3 pointerLocation)
    {
        var position = transform.position;
        var lookDirection = new Vector2(pointerLocation.x - position.x, pointerLocation.z - position.z);
        _lookAngle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;
    }

    private void Awake()
    {
        _playerActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _moveAction = _playerActions.Player.Move;
        _moveAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
    }
    
    // Update is called once per frame
    private void Update()
    {
        _actionDirection = _moveAction.ReadValue<Vector2>().normalized;
    }


    private void FixedUpdate()
    {
        if (controller.isGrounded)
        {
            _vSpeed = 0;
        }
        else
        {
            _vSpeed -= gravityFactor * Time.fixedDeltaTime;
        }
        
        if (_actionDirection.magnitude >= 0.1f || _vSpeed != 0)
        {
            _actionDirection = speedFactor * Time.fixedDeltaTime * _actionDirection;
            var moveDirection = new Vector3(_actionDirection.x, _vSpeed, _actionDirection.y);
            controller.Move(moveDirection);
        }

        transform.rotation = Quaternion.Euler(0f, _lookAngle,0f);
    }
}

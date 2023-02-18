using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public CharacterController controller;
    [SerializeField] public float speedFactor = 6f;
    [SerializeField] public float gravityFactor = 9.8f;

    [SerializeField] public Player player;

    private PlayerInputActions _playerActions;
    private InputAction _moveAction;
    private InputAction _reloadAction;
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

        _reloadAction = _playerActions.Player.Reload;
        _reloadAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _reloadAction.Disable();
    }
    
    // Update is called once per frame
    private void Update()
    {
        _actionDirection = _moveAction.ReadValue<Vector2>().normalized;
        transform.rotation = Quaternion.Euler(0f, _lookAngle,0f);

        if (_reloadAction.WasPerformedThisFrame())
        {
            player.HandleReload();
        }
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

        var moveDirection =
            Time.fixedDeltaTime *
            new Vector3(
                speedFactor * _actionDirection.x, _vSpeed, speedFactor * _actionDirection.y);
        
        controller.Move(moveDirection);
    }
}

using System;
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
    private InputAction _shootAction;
    private InputAction _dropAction;
    private InputAction _interactAction;
    private InputAction _scrollPositiveAction;
    private InputAction _scrollNegativeAction;
    private float _vSpeed;
    private float _lookAngle;
    private Vector2 _actionDirection = Vector2.zero;
    private Vector3 _pointerLocation = Vector3.zero;
    private Vector2 _lookDirection = Vector2.zero;
    private bool _shootHeld;
    private int _holdTime;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Angle = Animator.StringToHash("Angle");
    private static readonly int Falling = Animator.StringToHash("Falling");
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    public void UpdateLookAngle(Vector3 pointerLocation)
    {
        var position = transform.position;
        _lookDirection = new Vector2(pointerLocation.x - position.x, pointerLocation.z - position.z);
        _lookAngle = Mathf.Atan2(_lookDirection.x, _lookDirection.y) * Mathf.Rad2Deg;
        _pointerLocation = pointerLocation;
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

        _shootAction = _playerActions.Player.Fire;
        _shootAction.Enable();

        _dropAction = _playerActions.Player.Drop;
        _dropAction.Enable();

        _interactAction = _playerActions.Player.Interact;
        _interactAction.Enable();

        _scrollNegativeAction = _playerActions.Player.SwitchWeaponNeg;
        _scrollNegativeAction.Enable();

        _scrollPositiveAction = _playerActions.Player.SwitchWeaponPos;
        _scrollPositiveAction.Enable();

        _shootAction.started += StartShooting;
        _shootAction.canceled += StartShooting;
    }

    private void StartShooting(InputAction.CallbackContext ctx)
    {
        _shootHeld = ctx.control.IsPressed();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _reloadAction.Disable();
        _dropAction.Disable();
        _interactAction.Disable();
        _scrollNegativeAction.Disable();
        _scrollPositiveAction.Disable();
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

        if (_dropAction.WasPerformedThisFrame())
        {
            player.HandleDrop();
        }

        if (_interactAction.WasPerformedThisFrame())
        {
            player.HandleInteract(_pointerLocation);
        }

        if (_scrollNegativeAction.WasPerformedThisFrame())
        {
            player.backpack.SwitchPreviousWeapon();
        }
        else if (_scrollPositiveAction.WasPerformedThisFrame())
        {
            player.backpack.SwitchNextWeapon();
        }
        
        if (_shootHeld)
        {
            var attacked = player.HandleAttack(_pointerLocation, _holdTime);
            _holdTime++;

            if (attacked)
            {
                player.animator.SetTrigger(Shoot);
            }
        }
        else
        {
            _holdTime = 0;
        }

        if (_actionDirection.magnitude > 0)
        {
            player.animator.SetFloat(Speed, speedFactor/6);
            var angle = Vector2.SignedAngle(_lookDirection, _actionDirection);

            player.animator.SetFloat(Angle, angle);
        }
        else
        {
            player.animator.SetFloat(Speed, 0);
        }

        player.animator.SetBool(Falling, _vSpeed < -0.9f);
    }


    private void FixedUpdate()
    {
        if (controller.isGrounded)
        {
            _vSpeed = -0.8f;
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

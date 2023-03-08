using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public CharacterController controller;
    [SerializeField] public float gravityFactor = 9.8f;
    [SerializeField] public Player player;
    [SerializeField] public InventoryManager inventoryManager;

    private PlayerInputActions _playerActions;
    private InputAction _moveAction;
    private InputAction _reloadAction;
    private InputAction _shootAction;
    private InputAction _dropAction;
    private InputAction _interactAction;
    private InputAction _scrollPositiveAction;
    private InputAction _scrollNegativeAction;
    private InputAction _inventoryAction;
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
    private ImpactHandler _impactHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;
        
        Zombie character = other.gameObject.GetComponentInParent<Zombie>();
        if (character.touchDamage == null) return;
        
        player.RemoveHealth(character.touchDamage.CalculateDamage());

        Vector3 knockbackDirection = new Vector3(-_actionDirection.x, 0, -_actionDirection.y);
        knockbackDirection += other.gameObject.GetComponentInParent<NavMeshAgent>().velocity;
        
        _impactHandler.AddImpact(knockbackDirection, character.touchKnockback);
    }
    
    public void UpdateLookAngle(Vector3 pointerLocation)
    {
        if (inventoryManager.Active) return;

        var position = transform.position;
        _lookDirection = new Vector2(pointerLocation.x - position.x, pointerLocation.z - position.z);
        _lookAngle = Mathf.Atan2(_lookDirection.x, _lookDirection.y) * Mathf.Rad2Deg;
        _pointerLocation = pointerLocation;
    }

    private void Awake()
    {
        _playerActions = new PlayerInputActions();
        _impactHandler = GetComponent<ImpactHandler>();
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

        _inventoryAction = _playerActions.Player.Inventory;
        _inventoryAction.Enable();

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
        _inventoryAction.Disable();
        _scrollNegativeAction.Disable();
        _scrollPositiveAction.Disable();
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (_inventoryAction.WasPerformedThisFrame())
        {
            inventoryManager.Toggle();
        }

        _actionDirection = _moveAction.ReadValue<Vector2>().normalized;
        transform.rotation = Quaternion.Euler(0f, _lookAngle,0f);

        if (_reloadAction.WasPerformedThisFrame())
        {
            player.HandleReload();
        }

        if (_actionDirection.magnitude > 0)
        {
            player.animator.SetFloat(Speed, player.Speed/6);
            var angle = Vector2.SignedAngle(_lookDirection, _actionDirection);

            player.animator.SetFloat(Angle, angle);
        }
        else
        {
            player.animator.SetFloat(Speed, 0);
        }

        player.animator.SetBool(Falling, _vSpeed < -0.9f);
        
        
        if (inventoryManager.Active)
        {
            _holdTime = 0;
            if (player.backpack.weapon != null && player.backpack.weapon.weaponHoldStyle.lookAtConstraint != null)
            {
                player.backpack.weapon.weaponHoldStyle.lookAtConstraint.constraintActive = false;
            }
            return;
        }

        if (player.backpack.weapon != null && player.backpack.weapon.weaponHoldStyle.lookAtConstraint != null)
        {
            player.backpack.weapon.weaponHoldStyle.lookAtConstraint.constraintActive = true;
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
                player.Speed * _actionDirection.x, _vSpeed, player.Speed * _actionDirection.y);

        controller.Move(moveDirection);
    }
}

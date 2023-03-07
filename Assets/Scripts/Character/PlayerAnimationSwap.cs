using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class PlayerAnimationSwap : MonoBehaviour
{
    public Animator playerAnimator;
    public RigBuilder rigBuilder;
    public ParentConstraint leftConstraint;
    public ParentConstraint rightConstraint;
    public TwoBoneIKConstraint leftIKConstraint;
    public TwoBoneIKConstraint rightIKConstraint;

    private RuntimeAnimatorController _initController;
    private ConstraintSource _constraintSourceLeft;
    private ConstraintSource _constraintSourceRight;
    private Weapon _lastWeapon;

    // Start is called before the first frame update
    private void Awake()
    {
        _initController = playerAnimator.runtimeAnimatorController;
    }

    private void SwapOld()
    {
        if (_lastWeapon == null)
        {
            return;
        }
        
        if (_lastWeapon.weaponHoldStyle.lookAtConstraint != null)
        {
            _lastWeapon.weaponHoldStyle.lookAtConstraint.constraintActive = false;
        }
        
        
        if (_lastWeapon.weaponHoldStyle.parent != null)
        {
            _lastWeapon.transform.parent = _lastWeapon.lastParent;
        }
    }

    public void SwapAnimation(Weapon weapon)
    {
        SwapOld();
        _lastWeapon = weapon;
        if (weapon == null)
        {
            playerAnimator.runtimeAnimatorController = _initController;
            rigBuilder.layers[0].active = false;
            return;
        }

        if (weapon.weaponHoldStyle.parent != null)
        {
            weapon.lastParent = weapon.transform.parent;
            weapon.transform.parent = weapon.weaponHoldStyle.parent;
        }

        playerAnimator.runtimeAnimatorController = weapon.playerAnimator;
        if (weapon.weaponHoldStyle.leftGrip != null)
        {
            _constraintSourceLeft.weight = 1;
            _constraintSourceLeft.sourceTransform = weapon.weaponHoldStyle.leftGrip;
            leftIKConstraint.weight = 1;
            leftConstraint.SetSources(new List<ConstraintSource>() {_constraintSourceLeft});
        }
        else
        {
            leftIKConstraint.weight = 0;
        }
        
        if (weapon.weaponHoldStyle.rightGrip != null)
        {
            _constraintSourceRight.weight = 1;
            _constraintSourceRight.sourceTransform = weapon.weaponHoldStyle.rightGrip;
            rightIKConstraint.weight = 1;
            rightConstraint.SetSources(new List<ConstraintSource>() {_constraintSourceRight});
        }
        else
        {
            rightIKConstraint.weight = 0;
        }
        
        if (weapon.weaponHoldStyle.lookAtConstraint != null)
        {
            weapon.weaponHoldStyle.lookAtConstraint.constraintActive = true;
        }
        rigBuilder.layers[0].active = true;

    }
}

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

    // Start is called before the first frame update
    private void Awake()
    {
        _initController = playerAnimator.runtimeAnimatorController;
    }

    public void SwapAnimation(Weapon weapon)
    {
        if (weapon is null)
        {
            playerAnimator.runtimeAnimatorController = _initController;
            rigBuilder.layers[0].active = false;
            return;
        }

        playerAnimator.runtimeAnimatorController = weapon.playerAnimator;
        if (weapon.weaponHoldStyle.leftGrip is not null)
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
        
        if (weapon.weaponHoldStyle.rightGrip is not null)
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
        rigBuilder.layers[0].active = true;

    }
}

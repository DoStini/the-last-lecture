using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSwap : MonoBehaviour
{
    public Animator playerAnimator;
    private RuntimeAnimatorController _initController;
    
    // Start is called before the first frame update
    void Start()
    {
        _initController = playerAnimator.runtimeAnimatorController;
    }

    public void SwapAnimation(Weapon weapon)
    {
        if (weapon is null)
        {
            playerAnimator.runtimeAnimatorController = _initController;
            return;
        }

        playerAnimator.runtimeAnimatorController = weapon.playerAnimator;
    }
}

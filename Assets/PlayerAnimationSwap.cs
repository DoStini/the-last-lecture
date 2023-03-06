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

    public void SwapAnimation(RuntimeAnimatorController controller)
    {
        if (controller is null)
        {
            playerAnimator.runtimeAnimatorController = _initController;
            return;
        }

        playerAnimator.runtimeAnimatorController = controller;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealth : PickableItem
{
    public int health;
    private Player _player;

    public override void Pick(GameObject backpack)
    {
        base.Pick(backpack);
        _player = backpack.GetComponentInParent<Player>();
    }

    public override bool HasAction()
    {
        return true;
    }

    public override void RunAction()
    {
        _player.maxHealth += health;
        _player.backpack.RemovePickableItem(this, false);
        Destroy(this.gameObject);
    }
}

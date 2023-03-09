using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MaxHealth : PickableItem
{
    public int health;
    public int minHealth;
    public int maxHealth;    private Player _player;

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
    
    public override void Randomize()
    {
        health = new Random().Next(minHealth, maxHealth);
    }
}

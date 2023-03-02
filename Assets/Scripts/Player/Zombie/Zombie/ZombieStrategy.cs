using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZombieStrategy : MonoBehaviour
{
    [SerializeField] public MovementStrategy movementStrategy;
    [SerializeField] protected Zombie zombie;

    protected Player attackTarget;
    protected float attackRange;
    protected Weapon weapon;

    protected void _Start()
    {
        attackTarget = zombie.target;
        attackRange = zombie.attackRange;
        weapon = zombie.weapon;
    }

    public abstract void Act();
}

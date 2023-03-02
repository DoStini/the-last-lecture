using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZombieStrategy : MonoBehaviour
{
    [SerializeField] public MovementStrategy movementStrategy;
    [SerializeField] public ShootingStrategy shootingStrategy;

    public abstract void Act();
}

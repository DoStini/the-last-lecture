using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class ZombieStrategy : MonoBehaviour
{
    [SerializeField] public MovementStrategy movementStrategy;
    [FormerlySerializedAs("shootingStrategy")] [SerializeField] public AttackStrategy attackStrategy;

    public abstract void Act();
}

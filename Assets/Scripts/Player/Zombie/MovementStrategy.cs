using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class MovementStrategy : MonoBehaviour
{
    public abstract void Move(NavMeshAgent agent);
}

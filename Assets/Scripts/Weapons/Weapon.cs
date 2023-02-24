using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int damage;
    public float attackInterval;
    public LayerMask mask;

    private float _lastAttack;

    public virtual void Attack(Vector3 direction)
    {
        if (!(_lastAttack + attackInterval < Time.time)) return;
        
        if (_Attack(direction))
        {
            _lastAttack = Time.time;
        }
    }

    protected abstract bool _Attack(Vector3 direction);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int damage;
    public float attackInterval;
    public LayerMask mask;
    public bool holdToAttack = true;

    private float _lastAttack;

    public virtual void Attack(Vector3 direction, int holdTime)
    {
        if (!(_lastAttack + attackInterval < Time.time)) return;
        if (!holdToAttack && holdTime > 0) return;
        
        if (_Attack(direction))
        {
            _lastAttack = Time.time;
        }
    }

    protected abstract bool _Attack(Vector3 direction);
}

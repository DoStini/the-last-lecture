using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Weapon : MonoBehaviour
{
    public DamageStrategy damageStrategy;
    public float attackInterval;
    public LayerMask mask;
    public bool automatic = true;

    private float _lastAttack;

    public void Attack(Vector3 pointerLocation, int holdTime)
    {
        if (!(_lastAttack + attackInterval < Time.time)) return;
        if (!automatic && holdTime > 0) return;
        
        if (_Attack(pointerLocation))
        {
            _lastAttack = Time.time;
        }
    }

    protected abstract bool _Attack(Vector3 pointerLocation);
}

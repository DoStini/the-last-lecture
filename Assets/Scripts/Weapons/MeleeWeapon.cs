using UnityEngine;

public class MeleeWeapon : Weapon
{
    public float range;
    protected override bool _Attack(Vector3 direction)
    {
        return false;
    }
}

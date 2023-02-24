using UnityEngine;

public class Pistol : FiringWeapon
{
    protected override bool _Attack(Vector3 direction)
    {
        return false;
    }
}
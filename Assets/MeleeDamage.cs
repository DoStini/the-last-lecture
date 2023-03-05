using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    [SerializeField] private MeleeWeapon weapon;

    public void DealDamage()
    {
        weapon.DealDamage();
    }
}

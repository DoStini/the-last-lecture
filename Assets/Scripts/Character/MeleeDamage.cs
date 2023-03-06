using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    [SerializeField] private Player player;

    public void DealDamage()
    {
        Weapon weapon = player.backpack.weapon;
        if (weapon == null) return;
        
        if (weapon is MeleeWeapon mw)
        {
            mw.DealDamage();
        }
    }
}

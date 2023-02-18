using UnityEngine;

public class FiringWeapon : Weapon
{
    // HAVE GUN TIP TO KNOW WHERE THE BULLETS COME FROM
    [SerializeField] public uint ammo;
    [SerializeField] public Stock.Type stockType;

    public void Reload(Stock stock)
    {
        ammo = stock.baseCapacity;
    }
    
    public override void attack()
    {
        
    }
}

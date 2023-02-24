using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public uint maxWeight;
    public Backpack backpack;
    public Weapon weapon;

    private void Start()
    {
        Init();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickable"))
        {
            HandlePickable(other.gameObject);
        }
    }

    public void HandleReload()
    {
        if (weapon is not FiringWeapon firingWeapon)
        {
            return;
        }

        Stock stock = backpack.FindStock(firingWeapon.stockType);

        if (stock is null)
        {
            Debug.Log("No stocks remaining");
            return;
        }

        backpack.RemovePickableItem(stock);
        firingWeapon.Reload(stock);
    }
    
    private void HandlePickable(GameObject pickableGameObject)
    {
        var pickableItem = pickableGameObject.GetComponent<PickableItem>();
        if (!backpack.AddPickableItem(pickableItem))
        {
            Debug.Log("Backpack max capacity");
            return;
        }
        
        Destroy(pickableGameObject);
        Debug.Log(pickableItem);
    }
    
}

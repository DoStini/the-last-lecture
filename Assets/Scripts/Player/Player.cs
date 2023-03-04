using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Backpack backpack;

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
        if (backpack.weapon is not FiringWeapon firingWeapon)
        {
            return;
        }

        Stock stock = backpack.FindStock(firingWeapon.stockType);

        if (stock is null)
        {
            Debug.Log("No stocks remaining");
            return;
        }

        if (backpack.RemovePickableItem(stock))
        {
            stock.Drop(backpack.weapon.transform.position);
        }

        backpack.RemovePickableItem(stock);
        firingWeapon.Reload(stock);
    }

    public void HandleAttack(Vector3 pointerLocation, int holdTime)
    {
        if (!ReferenceEquals(backpack.weapon, null))
        {
            backpack.weapon.Attack(pointerLocation, holdTime);
        }
    }

    private void HandlePickable(GameObject pickableGameObject)
    {
        var pickableItem = pickableGameObject.GetComponent<PickableItem>();

        if (!backpack.AddPickableItem(pickableItem))
        {
            Debug.Log("Backpack max capacity");
            return;
        }

        pickableItem.Pick(gameObject);
    }
}

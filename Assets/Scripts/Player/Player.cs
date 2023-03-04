using System.Linq;
using UnityEngine;

public class Player : Character
{
    public Backpack backpack;
    public float pickRange;
    
    private void Start()
    {
        Init();
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

    public void HandleDrop()
    {
        if (!ReferenceEquals(backpack.weapon, null))
        {
            backpack.weapon.Drop(backpack.weapon.transform.position);
            backpack.RemovePickableItem(backpack.weapon);
        }
    }

    public void HandleInteract(Vector3 pointerLocation)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pointerLocation, 0.5f);
        Collider closestPickableItem = hitColliders.AsQueryable().OfType<Collider>()
            .Where(item => item.GetComponent<PickableItem>())
            .Where(item => Vector3.Distance(item.transform.position, transform.position) < pickRange)
            .OrderBy(item => Vector3.Distance(item.transform.position, pointerLocation)).FirstOrDefault();

        if (closestPickableItem is null)
        {
            return;
        }

        var pickableItem = closestPickableItem.GetComponent<PickableItem>();

        if (!backpack.AddPickableItem(pickableItem))
        {
            Debug.Log("Backpack max capacity");
            return;
        }

        pickableItem.Pick(gameObject);
    }
}

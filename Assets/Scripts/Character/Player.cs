using System.Linq;
using UnityEngine;

public class Player : Character
{
    [SerializeField] public Animator animator;

    public Backpack backpack;
    private static readonly int Hit = Animator.StringToHash("Hit");
    public float pickRange;

    private void Start()
    {
        Init();
        CharacterController controller = GetComponent<CharacterController>();
        controller.radius = radius;
        controller.height = height;
    }

    public override void RemoveHealth(int health)
    {
        base.RemoveHealth(health);
        
        animator.SetTrigger(Hit);
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

        if (firingWeapon.Stock != null) firingWeapon.Stock.Drop();

        backpack.RemovePickableItem(stock, false);

        firingWeapon.Reload(stock);
    }

    public bool HandleAttack(Vector3 pointerLocation, int holdTime)
    {
        return !ReferenceEquals(backpack.weapon, null) && backpack.weapon.Attack(pointerLocation, holdTime);
    }

    public void HandleDrop()
    {
        if (backpack.weapon is null) return;

        backpack.RemovePickableItem(backpack.weapon);
    }

    public void HandleInteract(Vector3 pointerLocation)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pointerLocation, 0.5f);
        float closest = float.MaxValue;
        Collider closestPickableItem = null;

        foreach (var hitCollider in hitColliders)
        {
            if (!hitCollider.CompareTag("Pickable")) continue;
            if (Vector3.Distance(hitCollider.transform.position, transform.position) >= pickRange) continue;

            float distance = Vector3.Distance(hitCollider.transform.position, pointerLocation);
            if (distance >= closest) continue;

            closest = distance;
            closestPickableItem = hitCollider;
        }

        if (closestPickableItem == null)
        {
            return;
        }
        var pickableItem = closestPickableItem.GetComponent<PickableItem>();
        Debug.Log(closestPickableItem);
        
        if (!backpack.AddPickableItem(pickableItem))
        {
            Debug.Log("Backpack max capacity");
            return;
        }
    }
}

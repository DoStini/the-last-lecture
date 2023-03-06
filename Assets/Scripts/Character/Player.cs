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

        if (backpack.RemovePickableItem(stock))
        {
            stock.Drop(backpack.weapon.transform.position);
        }

        backpack.RemovePickableItem(stock);
        firingWeapon.Reload(stock);
    }

    public bool HandleAttack(Vector3 pointerLocation, int holdTime)
    {
        return !ReferenceEquals(backpack.weapon, null) && backpack.weapon.Attack(pointerLocation, holdTime);
    }

    public void HandleDrop()
    {
        if (backpack.weapon is null) return;

        backpack.weapon.Drop(backpack.weapon.transform.position);
        backpack.RemovePickableItem(backpack.weapon);
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

        if (!backpack.AddPickableItem(pickableItem, gameObject))
        {
            Debug.Log("Backpack max capacity");
            return;
        }
    }
}

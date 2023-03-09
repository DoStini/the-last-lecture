using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerDeathEvent : UnityEvent
{
    
} 

public class Player : Character
{
    [SerializeField] private PlayerDeathEvent _playerDeathEvent;
    [SerializeField] public Animator animator;
    public RuntimeAnimatorController deathAnimator;

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
        if (_currentHealth == 0)
        {
            _playerDeathEvent.Invoke();

            animator.runtimeAnimatorController = deathAnimator;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            Time.timeScale = 0;
            return;
        }
        
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
        if (ReferenceEquals(backpack.weapon, null)) return false;

        bool attacked = backpack.weapon.Attack(pointerLocation, holdTime);
        if (!attacked) return false;

        if (backpack.weapon.durability <= 0)
        {
            var reference = backpack.weapon;
            backpack.RemovePickableItem(backpack.weapon, false);
            Destroy(reference.gameObject);
        }

        return true;
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
        
        if (!backpack.AddPickableItem(pickableItem))
        {
            Debug.Log("Backpack max capacity");
            return;
        }
    }
}

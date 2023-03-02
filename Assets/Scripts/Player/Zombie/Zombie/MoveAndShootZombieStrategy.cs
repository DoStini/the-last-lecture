using UnityEngine;

public class MoveAndShootZombieStrategy : ZombieStrategy
{
    private void Start()
    {
        _Start();
    }

    public override void Act()
    {
        movementStrategy.Move();

        float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
        
        if (distance > attackRange) return;

        var weaponTransform = weapon.transform;
        Vector3 direction = weaponTransform.forward.normalized;
        Vector3 targetPoint = weaponTransform.position + direction * distance;
        
        weapon.Attack( targetPoint, 0);
    }
}

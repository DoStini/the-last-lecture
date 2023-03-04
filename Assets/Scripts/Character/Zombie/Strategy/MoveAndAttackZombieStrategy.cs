public class MoveAndAttackZombieStrategy : ZombieStrategy
{
    public override void Act()
    {
        movementStrategy.Move();
        attackStrategy.Shoot();
    }
}

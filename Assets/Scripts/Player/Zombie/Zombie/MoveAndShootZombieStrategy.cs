public class MoveAndShootZombieStrategy : ZombieStrategy
{
    public override void Act()
    {
        movementStrategy.Move();
        shootingStrategy.Shoot();
    }
}

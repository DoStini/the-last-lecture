public class ConstantShootingStrategy : ShootingStrategy
{
    protected override void _Shoot()
    {
        var targetPoint = TargetPoint();

        if (targetPoint == null) return;

        weapon.Attack( targetPoint.Value, 0);
    }
}

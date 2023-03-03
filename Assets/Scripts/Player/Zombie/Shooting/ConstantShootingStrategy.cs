public class ConstantShootingStrategy : ShootingStrategy
{
    protected override bool _Shoot()
    {
        var targetPoint = TargetPoint();

        if (targetPoint == null) return false;

        weapon.Attack( targetPoint.Value, holdTime);
        return true;
    }
}

public class ConstantAttackStrategy : AttackStrategy
{
    protected override bool _Shoot()
    {
        var targetPoint = TargetPoint();

        if (targetPoint == null) return false;

        _weapon.Attack( targetPoint.Value, holdTime);
        return true;
    }
}

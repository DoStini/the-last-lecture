using UnityEngine;

public class RandomDamageStrategy : DamageStrategy
{
    public float baseDamage;
    public float variation;

    public override float CalculateDamage()
    {
        return Random.Range(baseDamage - baseDamage * variation, baseDamage + baseDamage * variation);
    }
}
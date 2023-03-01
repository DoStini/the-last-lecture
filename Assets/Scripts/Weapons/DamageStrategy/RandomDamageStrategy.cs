using UnityEngine;

public class RandomDamageStrategy : DamageStrategy
{
    public float baseDamage;
    public float variation;

    public override int CalculateDamage()
    {
        return Mathf.FloorToInt(Random.Range(baseDamage - baseDamage * variation, baseDamage + baseDamage * variation));
    }
}
using JetBrains.Annotations;
using UnityEngine;

public abstract class FiringWeapon : Weapon
{
    public Stock.Type stockType;
    public Transform bulletSpawnPoint;

    [field: SerializeField] public uint Ammo { get; private set; }
    [SerializeField] private bool bulletSpread = true;
    [SerializeField] private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private float bulletSpeed = 100;
    [SerializeField] private ShootingRenderer shootingRenderer;

    public void Reload(Stock stock)
    {
        Ammo = stock.baseCapacity;
    }

    private Vector3 GetDirection(Vector3 pointerLocation)
    {
        Vector3 direction = pointerLocation - bulletSpawnPoint.position;
        if (!bulletSpread) return direction;

        direction += new Vector3(
            Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
            Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
            Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
        );

        direction.Normalize();

        return direction;
    }
    
    protected override bool _Attack(Vector3 pointerLocation)
    {
        if (Ammo == 0)
        {
            return false;
        }

        Ammo--;
        Vector3 direction = GetDirection(pointerLocation);
        Vector3 position = bulletSpawnPoint.position;

        bool madeImpact = Physics.Raycast(
            position, direction,
            out RaycastHit hit, float.MaxValue, mask);

        shootingRenderer.Render(() =>
            {
                if (!hit.collider.gameObject.CompareTag("Enemy") && !hit.collider.gameObject.CompareTag("Player")) return;
                
                Character character = hit.collider.gameObject.GetComponent<Character>();
                character.RemoveHealth(damageStrategy.CalculateDamage());
            }, 
            direction, madeImpact, hit, bulletSpeed);

        return true;
    }
}

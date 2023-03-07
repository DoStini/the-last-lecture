using JetBrains.Annotations;
using UnityEngine;

public abstract class FiringWeapon : Weapon
{
    public Stock.Type stockType;
    public Transform bulletSpawnPoint;
    public bool infiniteAmmo = false;
    
    public uint Ammo => Stock != null ? Stock.Ammo : 0;
    [CanBeNull] public Stock Stock { get; private set; }

    [SerializeField] private bool bulletSpread = true;
    [SerializeField] private Vector3 bulletSpreadVariance = new (0.1f, 0.1f, 0.1f);
    [SerializeField] private float bulletSpeed = 100;
    [SerializeField] private ShootingRenderer shootingRenderer;
    [SerializeField] private PositionAndRotation stockPosition;

    public void Reload(Stock stock)
    {
        Stock = stock;
        stock.transform.SetParent(transform);
        stock.transform.SetLocalPositionAndRotation(stockPosition.position, stockPosition.rotation);
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
        if (!infiniteAmmo && (Stock is null || Stock.Ammo == 0))
        {
            return false;
        }

        if(Stock is not null) Stock.Ammo--;

        Vector3 direction = GetDirection(pointerLocation);
        Vector3 position = bulletSpawnPoint.position;

        bool madeImpact = Physics.Raycast(
            position, direction,
            out RaycastHit hit, float.MaxValue, mask);

        shootingRenderer.Render(() =>
            {
                if (!madeImpact) return;
                if (!hit.collider.gameObject.CompareTag("Enemy") && !hit.collider.gameObject.CompareTag("Player")) return;

                Character character = hit.collider.gameObject.GetComponent<Character>();
                character.RemoveHealth(damageStrategy.CalculateDamage());
            },
            direction, madeImpact, hit, bulletSpeed);

        return true;
    }
}

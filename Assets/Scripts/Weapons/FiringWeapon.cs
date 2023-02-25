using System.Collections;
using UnityEngine;

public abstract class FiringWeapon : Weapon
{
    public uint ammo;
    public Stock.Type stockType;
    public Transform bulletSpawnPoint;

    private bool _bulletSpread = true;
    private Vector3 _bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem shootingParticleSystem;
    [SerializeField] private float bulletSpeed = 100;
    [SerializeField] private ObjectPool impactPool;
    [SerializeField] private ObjectPool trailPool;


    public void Reload(Stock stock)
    {
        ammo = stock.baseCapacity;
    }

    private Vector3 GetDirection(Vector3 pointerLocation)
    {
        Vector3 direction = pointerLocation - bulletSpawnPoint.position;
        if (!_bulletSpread) return direction;

        direction += new Vector3(
            Random.Range(-_bulletSpreadVariance.x, _bulletSpreadVariance.x),
            Random.Range(-_bulletSpreadVariance.y, _bulletSpreadVariance.y),
            Random.Range(-_bulletSpreadVariance.z, _bulletSpreadVariance.z)
        );

        direction.Normalize();

        return direction;
    }
    
    protected override bool _Attack(Vector3 pointerLocation)
    {
        if (ammo == 0)
        {
            return false;
        }

        ammo--;
        shootingParticleSystem.Play();
        Vector3 direction = GetDirection(pointerLocation);

        Vector3 position = bulletSpawnPoint.position;
        TrailRenderer trail = trailPool.GetAndActivate((obj =>
        {
            obj.transform.position = position;
            obj.transform.rotation = Quaternion.identity;
        })).GetComponent<TrailRenderer>();
        
        StartCoroutine(Physics.Raycast(
            position, direction,
            out RaycastHit hit, float.MaxValue, mask)
            ? SpawnTrail(trail, hit.point, hit.normal, true)
            : SpawnTrail(trail, bulletSpawnPoint.position + direction * 100, Vector3.zero, false));

        return true;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal, bool madeImpact)
    {
        Vector3 position = trail.transform.position;

        float distance = Vector3.Distance(position, hitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            trail.transform.position = Vector3.Lerp(position, hitPoint, 1 - (remainingDistance / distance));
            remainingDistance -= bulletSpeed * Time.deltaTime;
            
            yield return null;
        }
        
        trail.transform.position = hitPoint;
        if (madeImpact)
        {
            impactPool.GetAndActivate((impactParticleSystem =>
            {
                impactParticleSystem.transform.position = hitPoint;
                impactParticleSystem.transform.rotation = Quaternion.LookRotation(hitNormal);
            }));
        }

        yield return new WaitForSeconds(trail.time);
        trailPool.Release(trail.gameObject);
    }
}

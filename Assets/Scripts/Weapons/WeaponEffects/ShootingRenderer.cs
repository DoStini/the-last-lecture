
using System;
using System.Collections;
using UnityEngine;

public class ShootingRenderer : MonoBehaviour
{
    public Transform bulletSpawnPoint;

    [SerializeField] private ObjectPool impactPool;
    [SerializeField] private ObjectPool trailPool;
    [SerializeField] private ParticleSystem shootingParticleSystem;
    
    public void Render(Action callback, Vector3 direction, bool madeImpact, RaycastHit hit, float bulletSpeed)
    {
        shootingParticleSystem.Play();

        StartCoroutine(madeImpact
            ? SpawnTrail(callback, hit.point, hit.normal, true, bulletSpeed)
            : SpawnTrail(callback, bulletSpawnPoint.position + direction * 100, Vector3.zero, false, bulletSpeed));
    }
    
    private IEnumerator SpawnTrail(Action callback, Vector3 hitPoint, Vector3 hitNormal, bool madeImpact, float bulletSpeed)
    {
        TrailRenderer trail = trailPool.GetAndActivate((obj =>
        {
            obj.transform.position = bulletSpawnPoint.position;
            obj.transform.rotation = Quaternion.identity;
        })).GetComponent<TrailRenderer>();
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
        callback();
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class ShootingRenderer : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public LayerMask impactLayers;

    [SerializeField] private ObjectPool impactPool;
    [SerializeField] private ObjectPool trailPool;
    [SerializeField] private ParticleSystem shootingParticleSystem;
    
    public void Render(Action callback, Vector3 direction, bool madeImpact, RaycastHit hit, float bulletSpeed)
    {
        shootingParticleSystem.Play();

        StartCoroutine(madeImpact
            ? SpawnTrail(callback, hit.point, hit.normal, hit, true, bulletSpeed)
            : SpawnTrail(
                callback, 
                bulletSpawnPoint.position + direction * 100, 
                Vector3.zero, hit, false, bulletSpeed));
    }
    
    private IEnumerator SpawnTrail(Action callback, Vector3 hitPoint, Vector3 hitNormal, RaycastHit hit, bool madeImpact, float bulletSpeed)
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
        if (madeImpact && impactLayers == (impactLayers | (1 << hit.collider.gameObject.layer)))
        {
            impactPool.GetAndActivate((impactParticleSystem =>
            {
                // ParentConstraint constraint = impactParticleSystem.GetComponent<ParentConstraint>();

                impactParticleSystem.transform.position = hitPoint;
                impactParticleSystem.transform.rotation = Quaternion.LookRotation(hitNormal);
                
                
                // ConstraintSource source = default;
                // source.sourceTransform = hit.transform;
                // source.weight = 1;
                // constraint.SetSource(0, source);
            }));
        }

        yield return new WaitForSeconds(trail.time);
        trailPool.Release(trail.gameObject);
        callback();
    }
}
using UnityEngine;

public abstract class ImpactHandler : MonoBehaviour
{
    public float mass = 3.0f;
    protected Vector3 impulse = Vector3.zero;

    public void AddImpact(Vector3 direction, float force)
    {
        impulse += direction.normalized * force / mass;
    }
}
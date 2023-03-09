using UnityEngine;

public class MeleeWeapon : Weapon
{
    public Transform playerCenter;
    public Vector3 boxSize;
    public float knockbackFactor;
    private bool _mStart;

    private Collider[] _colliders;
    private Vector3 _boxCenter;

    private void Start()
    {
        _colliders = new Collider[80];
        _mStart = true;
        playerCenter = GameObject.FindWithTag("PlayerCenter").transform;
        weaponHoldStyle.parent = GameObject.FindWithTag("MeleeParent").transform;
    }

    protected override bool _Attack(Vector3 pointerLocation)
    {
        return true;
    }

    // ReSharper disable once UnusedMember.Local
    public void DealDamage()
    {
        var rotation = playerCenter.rotation;

        _boxCenter = playerCenter.position + rotation * Vector3.forward * boxSize.z;
        int numColliders = Physics.OverlapBoxNonAlloc(_boxCenter, boxSize, _colliders, rotation, mask);

        audioSource.PlayOneShot(audioSource.clip);

        for (int i = 0; i < numColliders; i++)
        {
            Collider hitCollider = _colliders[i];
            if (!hitCollider.gameObject.CompareTag("Enemy") && !hitCollider.gameObject.CompareTag("Player")) return;

            Character character = hitCollider.gameObject.GetComponent<Character>();
            character.RemoveHealth(damageStrategy.CalculateDamage());
            
            ImpactHandler impactHandler = hitCollider.gameObject.GetComponent<ImpactHandler>();
            
            Vector3 knockbackDirection = rotation * Vector3.forward;
            impactHandler.AddImpact(knockbackDirection, knockbackFactor);
        }
    }

    private void OnDrawGizmos()
    {
// cache previous Gizmos settings
        if (_mStart)
        {
            _boxCenter = playerCenter.position + playerCenter.rotation * Vector3.forward * boxSize.z;

            Color prevColor = Gizmos.color;
            Matrix4x4 prevMatrix = Gizmos.matrix;

            Gizmos.color = Color.red;
            Gizmos.matrix = playerCenter.localToWorldMatrix;

            // convert from world position to local position 
            Vector3 boxPosition = playerCenter.InverseTransformPoint(_boxCenter);

            Gizmos.DrawWireCube(boxPosition, boxSize * 2f);

            // restore previous Gizmos settings
            Gizmos.color = prevColor;
            Gizmos.matrix = prevMatrix;
        }
    }

    public override void Randomize()
    {
    }
}

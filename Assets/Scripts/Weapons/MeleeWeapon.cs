using UnityEngine;

public class MeleeWeapon : Weapon
{
    public Transform playerCenter;
    public Vector3 boxSize;
    public Animator animator;
    public float knockbackFactor;
    private bool m_start;

    private Collider[] _colliders;
    private Vector3 _boxCenter;
    private static readonly int Attack1 = Animator.StringToHash("Attack");

    private void Start()
    {
        _colliders = new Collider[80];
        m_start = true;
    }

    protected override bool _Attack(Vector3 pointerLocation)
    {
        animator.SetTrigger(Attack1);
        return true;
    }

    private void DealDamage()
    {
        var rotation = playerCenter.rotation;
        
        _boxCenter = playerCenter.position + rotation * Vector3.forward * boxSize.z;
        int numColliders = Physics.OverlapBoxNonAlloc(_boxCenter, boxSize, _colliders, rotation, mask);

        for (int i = 0; i < numColliders; i++)
        {
            Collider hitCollider = _colliders[i];
            if (!hitCollider.gameObject.CompareTag("Enemy") && !hitCollider.gameObject.CompareTag("Player")) return;

            Character character = hitCollider.gameObject.GetComponent<Character>();
            character.RemoveHealth(damageStrategy.CalculateDamage());
        }
    }

    private void OnDrawGizmos()
    {
// cache previous Gizmos settings
        if (m_start)
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
}

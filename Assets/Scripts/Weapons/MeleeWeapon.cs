using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MeleeWeapon : Weapon
{
    public Transform playerCenter;
    public Vector3 boxSize;
    public float knockbackFactor;
    private bool m_start = false;

    private Collider[] _colliders;
    private Vector3 boxCenter;

    private void Start()
    {
        _colliders = new Collider[80];
        m_start = true;
    }

    protected override bool _Attack(Vector3 pointerLocation)
    {
        boxCenter = playerCenter.position + playerCenter.rotation * Vector3.forward * boxSize.z;

        Debug.Log(playerCenter.rotation.eulerAngles);
        Debug.Log(Physics.OverlapBoxNonAlloc(boxCenter, boxSize, _colliders, playerCenter.rotation, mask));
        return true;
    }

    private void OnDrawGizmos()
    {
// cache previous Gizmos settings
        if (m_start)
        {
            boxCenter = playerCenter.position + playerCenter.rotation * Vector3.forward * boxSize.z;

            Color prevColor = Gizmos.color;
            Matrix4x4 prevMatrix = Gizmos.matrix;

            Gizmos.color = Color.red;
            Gizmos.matrix = playerCenter.localToWorldMatrix;

            // convert from world position to local position 
            Vector3 boxPosition = playerCenter.InverseTransformPoint(boxCenter);

            Gizmos.DrawWireCube(boxPosition, boxSize * 2f);

            // restore previous Gizmos settings
            Gizmos.color = prevColor;
            Gizmos.matrix = prevMatrix;
        }
    }
}

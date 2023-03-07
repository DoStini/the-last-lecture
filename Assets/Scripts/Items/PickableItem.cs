using System;
using UnityEngine;

public abstract class PickableItem : MonoBehaviour
{
    [SerializeField] public uint weight;
    [SerializeField] public Sprite icon;
    [SerializeField] public GameObject model;
    [SerializeField] public new Rigidbody rigidbody;

    public bool isPicked;

    private GameObject _currentParent;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Pick(GameObject parent, bool deactivateModel, bool resetPosition)
    {
        isPicked = true;
        _rigidbody.isKinematic = true;
        transform.SetParent(parent.transform);
        model.SetActive(!deactivateModel);
        if (resetPosition) transform.SetLocalPositionAndRotation(Vector3.zero, transform.rotation);
    }

    public virtual void Drop()
    {
        _rigidbody.isKinematic = false;
        transform.SetParent(null);
        model.SetActive(true);
        isPicked = false;
    }

    public virtual void Pick(GameObject parent)
    {
        Pick(parent, true, true);
    }
}

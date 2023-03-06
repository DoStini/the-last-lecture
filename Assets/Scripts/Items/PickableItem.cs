using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class PickableItem : MonoBehaviour
{
    [SerializeField] public uint weight;
    [SerializeField] public Image icon;
    [SerializeField] public GameObject model;
    public bool isPicked;

    private GameObject _currentParent;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Pick(GameObject parent, bool deactivateModel)
    {
        isPicked = true;
        _rigidbody.isKinematic = true;
        transform.SetParent(parent.transform);
        model.SetActive(!deactivateModel);
    }

    public virtual void Drop(Vector3 position)
    {
        _rigidbody.isKinematic = false;
        transform.SetParent(null);
        transform.position = position;
        _rigidbody.AddTorque(5,5,5);
        isPicked = false;
    }

    public virtual void Pick(GameObject parent)
    {
        Pick(parent, true);
    }
}

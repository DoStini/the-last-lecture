using UnityEngine;
using UnityEngine.UI;

public abstract class PickableItem : MonoBehaviour
{
    [SerializeField] public uint weight;
    [SerializeField] public Image icon;
    [SerializeField] public GameObject model;
    [SerializeField] public Rigidbody rigidbody;
    public bool isPicked;

    private GameObject _currentParent;

    protected void _Pick(GameObject parent)
    {
        isPicked = true;
        rigidbody.isKinematic = true;
        transform.SetParent(parent.transform);
    }
    
    public virtual void Pick(GameObject parent)
    {
        _Pick(parent);
        model.SetActive(false);
    }

    public virtual void Drop(Vector3 position)
    {
        model.SetActive(true);
        rigidbody.isKinematic = false;
        transform.SetParent(null);
        transform.position = position;
        rigidbody.AddTorque(5,5,5);
        isPicked = false;
    }
}

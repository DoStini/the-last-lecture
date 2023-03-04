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

    public void Pick(GameObject parent, bool deactivateModel)
    {
        isPicked = true;
        rigidbody.isKinematic = true;
        transform.SetParent(parent.transform);
        model.SetActive(!deactivateModel);
    }

    public void Drop(Vector3 position)
    {
        rigidbody.isKinematic = false;
        transform.SetParent(null);
        transform.position = position;
        rigidbody.AddTorque(5,5,5);
        isPicked = false;
    }

    public virtual void Pick(GameObject parent)
    {
        Pick(parent, true);
    }
}

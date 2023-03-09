using UnityEngine;
using UnityEngine.Serialization;

public abstract class PickableItem : MonoBehaviour
{
    [SerializeField] public uint weight;
    [SerializeField] public Sprite icon;
    [SerializeField] public GameObject model;
    [SerializeField] public new Rigidbody rigidbody;

    public AudioSource dropAudioSource;
    public AudioSource pickAudioSource;

    private Outline _outline;
    
    private Collider _collider;

    public bool isPicked;

    private GameObject _currentParent;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _outline = GetComponent<Outline>();

        if (isPicked) _outline.enabled = false;
    }

    public void Pick(GameObject backpack, bool deactivateModel, bool resetPosition)
    {
        isPicked = true;
        _outline.enabled = false;
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        transform.SetParent(backpack.transform);
        model.SetActive(!deactivateModel);
        if (resetPosition) transform.SetLocalPositionAndRotation(Vector3.zero, transform.rotation);
        pickAudioSource.PlayOneShot(pickAudioSource.clip);
    }

    public virtual void Drop()
    {
        _rigidbody.isKinematic = false;
        _outline.enabled = true;
        _collider.enabled = true;
        transform.SetParent(null);
        model.SetActive(true);
        isPicked = false;
        rigidbody.AddTorque(15, 15, 15);
        dropAudioSource.PlayOneShot(dropAudioSource.clip);
    }

    public virtual void Pick(GameObject backpack)
    {
        Pick(backpack, true, true);
    }

    public virtual void RunAction()
    {
        
    }

    public virtual bool HasAction()
    {
        return false;
    }

    public abstract void Randomize();
}

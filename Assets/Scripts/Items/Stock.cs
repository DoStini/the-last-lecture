using UnityEngine;

public class Stock : PickableItem
{
    [SerializeField] public Type type;

    [SerializeField] public uint baseCapacity;

    public enum Type
    {
        Pistol,
        Rifle
    }

    public uint Ammo { get; set; }

    public override void Drop()
    {
        base.Drop();
        if (Ammo == 0)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        Ammo = baseCapacity;
    }
}

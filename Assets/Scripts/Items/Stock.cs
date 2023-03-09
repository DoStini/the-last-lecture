using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Stock : PickableItem
{
    [SerializeField] public Type type;

    [SerializeField] public uint pistolBaseCapacity;
    [SerializeField] public uint rifleBaseCapacity;

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

    public override void Randomize()
    {
        switch (type)
        {
            case Type.Pistol:
                Ammo = pistolBaseCapacity;
                break;
            case Type.Rifle: 
                Ammo = rifleBaseCapacity;
                break;
        }
    }

    public void Start()
    {
        Randomize();
    }
}

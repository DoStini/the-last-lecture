using UnityEngine;

public class Stock : PickableItem
{
    [SerializeField] public Type type;

    [SerializeField] public uint baseCapacity;
    private uint _currentCapacity;
    
    public enum Type
    {
        Pistol,
        Sniper
    }

    public void Start()
    {
        _currentCapacity = baseCapacity;
    }
}

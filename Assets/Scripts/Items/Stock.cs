using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock : PickableItem
{
    [SerializeField] public Type type;

    [SerializeField] public uint baseCapacity;
    private uint _currentCapacity;
    
    public enum Type
    {
        PISTOL,
        SNIPER
    }

    public void Start()
    {
        _currentCapacity = baseCapacity;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] public uint maxWeight;
    [SerializeField] public Backpack backpack;

    private void Start()
    {
        Init();
    }
}

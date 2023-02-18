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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickable"))
        {
            HandlePickable(other.gameObject);
        }
    }

    private void HandlePickable(GameObject pickableGameObject)
    {
        var pickableItem = pickableGameObject.GetComponent<PickableItem>();
        if (!backpack.AddPickableItem(pickableItem))
        {
            Debug.Log("Backpack max capacity");
            return;
        }
        
        Destroy(pickableGameObject);
        Debug.Log(pickableItem);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [SerializeField] public uint maxWeight;
    private uint _weight;

    private List<PickableItem> _items;

    public bool AddPickableItem(PickableItem item)
    {
        if (_weight + item.weight > maxWeight)
        {
            return false;
        }

        _items.Add(item);
        _weight += item.weight;
        
        return true;
    }

    public bool RemovePickableItem(PickableItem item)
    {
        if (!_items.Remove(item))
        {
            return false;
        }

        _weight -= item.weight;
        return true;
    }
    
    void Start()
    {
        _weight = maxWeight;
    }
}

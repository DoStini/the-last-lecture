using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [SerializeField] public uint maxWeight;
    private uint _weight;

    private readonly List<PickableItem> _items = new();

    public Stock FindStock(Stock.Type type)
    {
        var stocks = _items.AsQueryable();

        Stock stock = stocks.OfType<Stock>().FirstOrDefault(stock => stock.type == type);

        return default(Stock) ? null : stock;
    }

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
        _weight = 0;
    }
}

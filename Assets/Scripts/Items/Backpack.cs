using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [SerializeField] public uint maxWeight;
    private uint _weight;

    private readonly List<PickableItem> _items = new();

    private readonly List<Weapon> _weapons = new();
    public int maxWeapons;
    public int activeWeapon = -1;
    [CanBeNull] public Weapon weapon => activeWeapon == -1 ? null : _weapons[activeWeapon];

    public Stock FindStock(Stock.Type type)
    {
        var stocks = _items.AsQueryable();

        Stock stock = stocks.OfType<Stock>().FirstOrDefault(stock => stock.type == type);

        return default(Stock) ? null : stock;
    }

    public bool AddPickableItem(PickableItem item)
    {
        if (item.isPicked)
        {
            return false;
        }
        
        if (_weight + item.weight > maxWeight)
        {
            return false;
        }

        if (item is Weapon pickedWeapon)
        {
            if (_weapons.Count == maxWeapons) return false;

            _weapons.Add(pickedWeapon);
            activeWeapon = 0; // TODO CHANGE THIS
        }
        else
        {
            _items.Add(item);
        }
        
        _weight += item.weight;

        return true;
    }

    public bool RemovePickableItem(PickableItem item)
    {
        if (item is Weapon pickedWeapon)
        {
            _weapons.Remove(pickedWeapon);
        }
        else
        {
            _items.Remove(item);
        }

        _weight -= item.weight;
        return true;
    }
    
    void Start()
    {
        _weight = 0;
    }
}

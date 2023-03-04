using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [SerializeField] public uint maxWeight;
    private uint _weight;

    private readonly List<PickableItem> _items = new();

    private Weapon[] _weapons;
    public int maxWeapons;
    private int _numWeapons = 0;

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
            if (_numWeapons == maxWeapons) return false;

            AddWeapon(pickedWeapon);
        }
        else
        {
            _items.Add(item);
        }
        
        _weight += item.weight;

        return true;
    }

    private void AddWeapon(Weapon pickedWeapon)
    {
        int index = Array.FindIndex(_weapons, w => w is null);
        _weapons[index] = pickedWeapon;
        _numWeapons++;
    }

    public bool RemovePickableItem(PickableItem item)
    {
        if (item is Weapon pickedWeapon)
        {
            RemoveWeapon(pickedWeapon);
        }
        else
        {
            _items.Remove(item);
        }

        _weight -= item.weight;
        return true;
    }

    private void RemoveWeapon(Weapon pickedWeapon)
    {
        int index = Array.FindIndex(_weapons, w => pickedWeapon);
        _weapons[index] = pickedWeapon;
        _numWeapons--;
    }

    private void Start()
    {
        _weight = 0;
        _weapons = new Weapon[maxWeapons];
    }

    public void SwitchNextWeapon()
    {
        activeWeapon ++;
        if (activeWeapon == _numWeapons) activeWeapon = -1;
    }

    public void SwitchPreviousWeapon()
    {
        activeWeapon--;
        if (activeWeapon == -2) activeWeapon = _numWeapons - 1;
    }
}

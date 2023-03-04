using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class PositionAndRotation
{
    public Vector3 position;
    public Quaternion rotation;
}

public class Backpack : MonoBehaviour
{
    [SerializeField] public uint maxWeight;
    private uint _weight;

    private readonly List<PickableItem> _items = new();

    private Weapon[] _weapons;
    public int maxWeapons;
    private int _numWeapons = 0;
    public List<PositionAndRotation> weaponSlotPositions;

    public int activeWeapon = -1;
    [CanBeNull] public Weapon weapon => activeWeapon == -1 ? null : _weapons[activeWeapon];

    private void Start()
    {
        _weight = 0;
        _weapons = new Weapon[maxWeapons];
    }

    public Stock FindStock(Stock.Type type)
    {
        var stocks = _items.AsQueryable();

        Stock stock = stocks.OfType<Stock>().FirstOrDefault(stock => stock.type == type);

        return default(Stock) ? null : stock;
    }

    public bool AddPickableItem(PickableItem item, GameObject parent)
    {
        if (item.isPicked)
        {
            return false;
        }
        
        if (_weight + item.weight > maxWeight)
        {
            return false;
        }

        item.Pick(parent);

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
        
        VisuallyStoreWeapon(index);
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
        _weapons[index] = null;
        _numWeapons--;
    }

    public void SwitchNextWeapon()
    {
        VisuallyStoreWeaponInBackpack(activeWeapon);

        activeWeapon ++;
        if (activeWeapon == _numWeapons) activeWeapon = -1;

        VisuallyStoreWeaponInHand(activeWeapon);
    }

    private void VisuallyStoreWeaponInHand(int pickedWeaponIndex)
    {
        if (pickedWeaponIndex == -1) return;

        Weapon pickedWeapon = _weapons[pickedWeaponIndex];
        pickedWeapon.transform.SetLocalPositionAndRotation(pickedWeapon.activePosition, pickedWeapon.activeRotation);
    }

    public void VisuallyStoreWeapon(int index)
    {
        if (index == activeWeapon)
        {
            VisuallyStoreWeaponInHand(index);
        }
        else
        {
            VisuallyStoreWeaponInBackpack(index);
        }
    }

    private void VisuallyStoreWeaponInBackpack(int pickedWeaponIndex)
    {
        if (pickedWeaponIndex == -1) return;

        PositionAndRotation slot = weaponSlotPositions[pickedWeaponIndex];
        _weapons[pickedWeaponIndex].transform.SetLocalPositionAndRotation(slot.position, slot.rotation);
    }

    public void SwitchPreviousWeapon()
    {
        activeWeapon--;
        if (activeWeapon == -2) activeWeapon = _numWeapons - 1;
    }
}

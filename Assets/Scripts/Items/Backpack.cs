using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerBackpackUpdate : UnityEvent
{
    
}



[System.Serializable]
public class PositionAndRotation
{
    public Vector3 position;
    public Quaternion rotation;
}

public class Backpack : MonoBehaviour
{
    [SerializeField] private PlayerBackpackUpdate playerBackpackUpdate;
    [SerializeField] public uint maxWeight;

    public List<PickableItem> items = new() ;
    public InventoryManager inventoryManager;
    
    private Weapon[] _weapons;
    public int maxWeapons;
    private int _numWeapons = 0;
    public List<PositionAndRotation> weaponSlotPositions;

    public int activeWeapon = -1;
    [CanBeNull] public Weapon weapon => activeWeapon == -1 ? null : _weapons[activeWeapon];

    public int Count => items.Count;
    public uint Weight { get; private set; }

    private void Start()
    {
        Weight = 0;
        _weapons = new Weapon[maxWeapons];
    }

    public Stock FindStock(Stock.Type type)
    {
        var stocks = items.AsQueryable();

        Stock stock = stocks.OfType<Stock>().FirstOrDefault(stock => stock.type == type);

        return default(Stock) ? null : stock;
    }

    public int StockAmount(Stock.Type type)
    {
        var stocks = items.AsQueryable();

        return stocks.OfType<Stock>().Count(stock => stock.type == type);
    }

    public bool AddPickableItem(PickableItem item, GameObject parent)
    {
        if (item.isPicked)
        {
            return false;
        }
        
        if (Weight + item.weight > maxWeight)
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
            items.Add(item);
            inventoryManager.AddItem(item);
        }
        
        Weight += item.weight;
        playerBackpackUpdate.Invoke();
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
            items.Remove(item);
        }
        item.Drop();

        Weight -= item.weight;
        playerBackpackUpdate.Invoke();
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
        if (activeWeapon >= _numWeapons) activeWeapon = -1;

        VisuallyStoreWeaponInHand(activeWeapon);
        playerBackpackUpdate.Invoke();
    }

    public void SwitchPreviousWeapon()
    {
        VisuallyStoreWeaponInBackpack(activeWeapon);

        activeWeapon--;
        if (activeWeapon < -1) activeWeapon = _numWeapons - 1;

        VisuallyStoreWeaponInHand(activeWeapon);
        playerBackpackUpdate.Invoke();
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
}

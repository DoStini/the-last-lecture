using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

[Serializable]
public class PlayerBackpackUpdate : UnityEvent
{
    
}

[Serializable]
public class AnimationSwapEvent : UnityEvent<Weapon>
{
    
}

[Serializable]
public class PositionAndRotation
{
    public Vector3 position;
    public Quaternion rotation;
}

public class Backpack : MonoBehaviour
{
    [SerializeField] private PlayerBackpackUpdate playerBackpackUpdate;
    [SerializeField] private AnimationSwapEvent animationSwapEvent;
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
        animationSwapEvent.Invoke(weapon);
    }

    public Stock FindStock(Stock.Type type)
    {
        foreach (var pickableItem in _items)
        {
            if (pickableItem is not Stock stock) continue;

            if (stock.type == type)
            {
                return stock;
            }
        }

        return null;
    }

    public int StockAmount(Stock.Type type)
    {
        int count = 0;
        
        foreach (var pickableItem in _items)
        {
            if (pickableItem is not Stock stock) continue;

            if (stock.type == type)
            {
                count++;
            }
        }

        return count;
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

        item.Pick(gameObject);

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
            _items.Remove(item);
        }

        _weight -= item.weight;
        playerBackpackUpdate.Invoke();
        return true;
    }

    private void RemoveWeapon(Weapon pickedWeapon)
    {
        int index = Array.FindIndex(_weapons, w => pickedWeapon);
        _weapons[index] = null;
        _numWeapons--;

        if (index != activeWeapon) return;
        activeWeapon = -1;
        animationSwapEvent.Invoke(null);
    }

    private void SwitchWeapon(int lastWeapon)
    {
        VisuallyStoreWeaponInHand(activeWeapon);
        VisuallyStoreWeaponInBackpack(lastWeapon);
        playerBackpackUpdate.Invoke();
    }

    public void SwitchNextWeapon()
    {
        int lastWeapon = activeWeapon;
        activeWeapon ++;
        if (activeWeapon >= _numWeapons) activeWeapon = -1;

        SwitchWeapon(lastWeapon);
    }

    public void SwitchPreviousWeapon()
    {
        int lastWeapon = activeWeapon;
        activeWeapon--;
        if (activeWeapon < -1) activeWeapon = _numWeapons - 1;

        SwitchWeapon(lastWeapon);
    }

    private void VisuallyStoreWeaponInHand(int pickedWeaponIndex)
    {
        if (pickedWeaponIndex == -1)
        {
            animationSwapEvent.Invoke(null);
            return;
        }

        Weapon pickedWeapon = _weapons[pickedWeaponIndex];
        animationSwapEvent.Invoke(pickedWeapon);
        
        pickedWeapon.transform.SetLocalPositionAndRotation(pickedWeapon.activePosition, pickedWeapon.activeRotation);
    }

    private void VisuallyStoreWeapon(int index)
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

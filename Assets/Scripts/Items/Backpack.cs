using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerBackpackUpdate : UnityEvent
{
    
}

[Serializable]
public class AnimationSwapEvent : UnityEvent<RuntimeAnimatorController>
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
        animationSwapEvent.Invoke(activeWeapon == -1 ? null : _weapons[activeWeapon].playerAnimator);
    }

    public Stock FindStock(Stock.Type type)
    {
        var stocks = _items.AsQueryable();

        Stock stock = stocks.OfType<Stock>().FirstOrDefault(stock => stock.type == type);

        return default(Stock) ? null : stock;
    }

    public int StockAmount(Stock.Type type)
    {
        var stocks = _items.AsQueryable();

        return stocks.OfType<Stock>().Count(stock => stock.type == type);
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

    private void SwitchWeapon()
    {
        VisuallyStoreWeaponInHand(activeWeapon);
        playerBackpackUpdate.Invoke();
    }

    public void SwitchNextWeapon()
    {
        VisuallyStoreWeaponInBackpack(activeWeapon);

        activeWeapon ++;
        if (activeWeapon >= _numWeapons) activeWeapon = -1;

        SwitchWeapon();
    }

    public void SwitchPreviousWeapon()
    {
        VisuallyStoreWeaponInBackpack(activeWeapon);

        activeWeapon--;
        if (activeWeapon < -1) activeWeapon = _numWeapons - 1;

        SwitchWeapon();
    }

    private void VisuallyStoreWeaponInHand(int pickedWeaponIndex)
    {
        if (pickedWeaponIndex == -1)
        {
            animationSwapEvent.Invoke(null);
            return;
        }

        Weapon pickedWeapon = _weapons[pickedWeaponIndex];
        pickedWeapon.transform.SetLocalPositionAndRotation(pickedWeapon.activePosition, pickedWeapon.activeRotation);
        animationSwapEvent.Invoke(pickedWeapon.playerAnimator);
        
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

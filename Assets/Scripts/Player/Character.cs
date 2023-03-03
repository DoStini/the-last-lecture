using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int maxHealth;
    public float maxSpeed;
    public float minSpeed;
    
    public int baseHealth;
    public float baseSpeed;
    public List<DamageObserver> damageObservers;
    [CanBeNull] public HUDManager hudManager;

    private int _currentHealth;

    protected int CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = value;
            if (!ReferenceEquals(hudManager, null))
                hudManager.UpdateHealth(_currentHealth);
        }
    }

    private float _currentSpeed;

    public int GetHealth()
    {
        return CurrentHealth;
    }

    public void ResetHealth()
    {
        CurrentHealth = baseHealth;
    }

    public void ResetSpeed()
    {
        _currentSpeed = baseSpeed;
    }

    public void AddHealth(int health)
    {
        CurrentHealth += health;
        damageObservers.ForEach((observer => observer.HandleDamagePopup(this, health)));

        if (CurrentHealth > maxHealth)
        {
            CurrentHealth = maxHealth;
        }
    }

    public void RemoveHealth(int health)
    {
        CurrentHealth -= health;
        damageObservers.ForEach((observer => observer.HandleDamagePopup(this, -health)));

        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
    }

    public void BoostSpeed(float factor)
    {
        _currentSpeed *= factor;
        if (_currentSpeed > maxSpeed)
        {
            _currentSpeed = maxSpeed;
        }

        if (_currentSpeed < minSpeed)
        {
            _currentSpeed = minSpeed;
        }
    }

    protected void Init()
    {
        CurrentHealth = baseHealth;
        _currentSpeed = baseSpeed;
    }
}

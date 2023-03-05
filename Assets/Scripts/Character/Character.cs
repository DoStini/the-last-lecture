using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int maxHealth;
    public float maxSpeed;
    public float minSpeed;

    public int baseHealth;
    public float baseSpeed;
    public List<DamageObserver> damageObservers;

    public float radius;
    public float height;
    
    public float viewRange;
    
    protected int _currentHealth;
    private float _currentSpeed;

    public int GetHealth()
    {
        return _currentHealth;
    }

    public void ResetHealth()
    {
        _currentHealth = baseHealth;
    }

    public void ResetSpeed()
    {
        _currentSpeed = baseSpeed;
    }

    public void AddHealth(int health)
    {
        _currentHealth += health;
        damageObservers.ForEach((observer => observer.HandleDamagePopup(this, health)));

        if (_currentHealth > maxHealth)
        {
            _currentHealth = maxHealth;
        }
    }

    public void RemoveHealth(int health)
    {
        _currentHealth -= health;
        damageObservers.ForEach((observer => observer.HandleDamagePopup(this, -health)));

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
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
        _currentHealth = baseHealth;
        _currentSpeed = baseSpeed;
    }
}

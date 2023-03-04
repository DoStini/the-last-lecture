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

    public float attackRange;
    public float viewRange;
    
    protected int currentHealth;
    private float _currentSpeed;

    public int GetHealth()
    {
        return currentHealth;
    }

    public void ResetHealth()
    {
        currentHealth = baseHealth;
    }

    public void ResetSpeed()
    {
        _currentSpeed = baseSpeed;
    }

    public void AddHealth(int health)
    {
        currentHealth += health;
        damageObservers.ForEach((observer => observer.HandleDamagePopup(this, health)));

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void RemoveHealth(int health)
    {
        currentHealth -= health;
        damageObservers.ForEach((observer => observer.HandleDamagePopup(this, -health)));

        if (currentHealth < 0)
        {
            currentHealth = 0;
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
        currentHealth = baseHealth;
        _currentSpeed = baseSpeed;
            
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
            
        collider.radius = radius;
        collider.height = height;
    }
}

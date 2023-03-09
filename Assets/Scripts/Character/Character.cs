using System.Collections.Generic;
using UnityEngine;

public class giitCharacter : MonoBehaviour
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

    public float Speed { get; private set; }

    public bool IsDead()
    {
        return _currentHealth == 0;
    }

    public bool HasMaxSpeed()
    {
        return _currentHealth == maxHealth;
    }
    
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
        Speed = baseSpeed;
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

    public virtual void RemoveHealth(int health)
    {
        _currentHealth -= health;
        damageObservers.ForEach((observer => observer.HandleDamagePopup(this, -health)));

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }
    }

    public float BoostSpeed(float factor)
    {
        float prevSpeed = Speed;
        Speed *= factor;
        if (Speed > maxSpeed)
        {
            Speed = maxSpeed;
        }

        else if (Speed < minSpeed)
        {
            Speed = minSpeed;
        }

        return Speed / prevSpeed;
    }

    public void DecreaseSpeed(float factor)
    {
        Speed /= factor;
        if (Speed > baseSpeed - 0.1 && Speed < baseSpeed + 0.1)
        {
            Speed = baseSpeed;
        }
        else if (Speed > maxSpeed)
        {
            Speed = maxSpeed;
        }
        else if (Speed < minSpeed)
        {
            Speed = minSpeed;
        }
    }
    
    protected void Init()
    {
        _currentHealth = baseHealth;
        Speed = baseSpeed;
    }
}

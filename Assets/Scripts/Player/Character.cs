using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHealth;
    public float maxSpeed;
    public float minSpeed;
    
    public float baseHealth;
    public float baseSpeed;

    protected float _currentHealth;
    private float _currentSpeed;

    public float GetHealth()
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

    public void AddHealth(float health)
    {
        _currentHealth += health;

        if (_currentHealth > maxHealth)
        {
            _currentHealth = maxHealth;
        }
    }

    public void RemoveHealth(float health)
    {
        _currentHealth -= health;

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

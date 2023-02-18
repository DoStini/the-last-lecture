using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float minSpeed;
    
    [SerializeField] public int baseHealth;
    [SerializeField] public float baseSpeed;

    private int _currentHealth;
    private float _currentSpeed;

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

        if (_currentHealth > maxHealth)
        {
            _currentHealth = maxHealth;
        }
    }

    public void RemoveHealth(int health)
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
    
    private void Start()
    {
        _currentHealth = baseHealth;
        _currentSpeed = baseSpeed;
    }
}

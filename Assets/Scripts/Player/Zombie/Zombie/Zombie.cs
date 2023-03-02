using System;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Character
{
    [SerializeField] private ZombieStrategy zombieStrategy;
    [SerializeField] public Weapon weapon;
    [SerializeField] public Player target;
    [SerializeField] public float attackRange;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        zombieStrategy.Act();

        if (_currentHealth == 0)
        {
            gameObject.SetActive(false);
        }
    }
}

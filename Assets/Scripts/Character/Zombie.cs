using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zombie : Character
{
    [SerializeField] private ZombieStrategy zombieStrategy;
    [SerializeField] public Weapon weapon;
    [SerializeField] public Player target;
    [SerializeField] public Dropper dropper;

    public Animator zombieAnimator;
    public DamageStrategy touchDamage;
    public float touchKnockback;
    private static readonly int Hit = Animator.StringToHash("Hit");
    private HUDManager _hudManager;

    public override void RemoveHealth(int health)
    {
        base.RemoveHealth(health);
        
        zombieAnimator.SetTrigger(Hit);
    }
    
    private void Start()
    {
        Init();
        
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            
        capsuleCollider.radius = radius;
        capsuleCollider.height = height;

        _hudManager = GameObject.FindWithTag("HUD").GetComponent<HUDManager>();
    }

    private void Update()
    {
        zombieStrategy.Act();

        if (_currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _hudManager.AddToCounter();
        gameObject.SetActive(false);
        List<GameObject> gameObjects = dropper.GetDrops();

        foreach (var drop in gameObjects)
        {
            PickableItem item = drop.GetComponent<PickableItem>();
            if (item is null) continue;

            drop.transform.SetPositionAndRotation(transform.position, transform.rotation);
            item.Randomize();
        }
    }
}

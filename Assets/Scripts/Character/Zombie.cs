using UnityEngine;

public class Zombie : Character
{
    [SerializeField] private ZombieStrategy zombieStrategy;
    [SerializeField] public Weapon weapon;
    [SerializeField] public Player target;
    public Animator zombieAnimator;
    public DamageStrategy touchDamage;
    public float touchKnockback;
    private static readonly int Hit = Animator.StringToHash("Hit");

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

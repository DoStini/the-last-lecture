using UnityEngine;

public class Zombie : Character
{
    [SerializeField] private ZombieStrategy zombieStrategy;
    [SerializeField] public Weapon weapon;
    [SerializeField] public Player target;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        zombieStrategy.Act();

        if (currentHealth == 0)
        {
            gameObject.SetActive(false);
        }
    }
}

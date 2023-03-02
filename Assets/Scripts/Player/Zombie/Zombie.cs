using UnityEngine;

public class Zombie : Character
{
    [SerializeField] private MovementStrategy movementStrategy;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        movementStrategy.Move();

        if (_currentHealth == 0)
        {
            gameObject.SetActive(false);
        }
    }
}

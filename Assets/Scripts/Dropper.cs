using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Dropper : MonoBehaviour
{
    public List<GameObject> prefabs;
    public int min;
    public int max;
    public List<double> chances;
    public float chance;

    private Random _random;

    private void Start()
    {
        _random = new Random();
    }

    public List<GameObject> GetDrops()
    {
        List<GameObject> gameObjects = new();

        if (_random.NextDouble() > chance)
        {
            return gameObjects;
        }

        int count = _random.Next(min, max);

        for (int _ = 0; _ < count; _++)
        {
            double dropChance = _random.NextDouble();
            int index = chances.BinarySearch(dropChance);
            if (index < 0)
            {
                index = ~index;
            }

            GameObject gameObject = Instantiate(prefabs[index]);
            Debug.Log(gameObject);
            gameObjects.Add(gameObject);
        }

        return gameObjects;
    }
}

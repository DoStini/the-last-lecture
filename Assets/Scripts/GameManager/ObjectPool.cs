using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectToPool;
    public int amountToPool;

    private LinkedList<GameObject> _pooledObjects;

    private void Start()
    {
        _pooledObjects = new LinkedList<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            var tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            _pooledObjects.AddLast(tmp);
        }
    }

    public GameObject GetAndActivate(Action<GameObject> instantiate)
    {
        GameObject obj = _pooledObjects.First.Value;
        instantiate(gameObject);
        
        _pooledObjects.RemoveFirst();
        _pooledObjects.AddLast(obj);
        gameObject.SetActive(true);

        return obj;
    }

    public void Release(GameObject toRelease)
    {
        for (var node = _pooledObjects.First; node != null; node = node.Next)
        {
            if (!node.Value.activeInHierarchy || !ReferenceEquals(node.Value, toRelease)) continue;
            
            _pooledObjects.Remove(node);
            _pooledObjects.AddLast(toRelease);
            toRelease.SetActive(false);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectToPool;
    public int amountToPool;

    protected LinkedList<GameObject> _pooledObjects;

    private void Start()
    {
        _pooledObjects = new LinkedList<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            var tmp = Instantiate(objectToPool, gameObject.transform);
            tmp.SetActive(false);
            _pooledObjects.AddLast(tmp);
        }
    }

    /**
     * Always returns an object, even if it is in use. No new objects created
     */
    public virtual GameObject GetAndActivate(Action<GameObject> instantiate)
    {
        GameObject obj = _pooledObjects.First.Value;
        instantiate(obj);
        
        _pooledObjects.RemoveFirst();
        _pooledObjects.AddLast(obj);
        obj.SetActive(true);

        return obj;
    }

    public virtual void Release(GameObject toRelease)
    {
        for (var node = _pooledObjects.First; node != null; node = node.Next)
        {
            if (!node.Value.activeInHierarchy || !ReferenceEquals(node.Value, toRelease)) continue;
            
            _pooledObjects.Remove(node);
            _pooledObjects.AddFirst(toRelease);
            toRelease.SetActive(false);

            break;
        }
    }
}

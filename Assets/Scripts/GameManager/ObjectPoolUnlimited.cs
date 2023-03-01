using System;
using UnityEngine;

public class ObjectPoolUnlimited : ObjectPool
{
    /**
     * Always returns an object, even if it is in use. No new objects created
     */
    public override GameObject GetAndActivate(Action<GameObject> instantiate)
    {
        GameObject obj = _pooledObjects.First.Value;

        if (obj.activeInHierarchy)
        {
            obj = Instantiate(objectToPool, gameObject.transform);
        }
        else
        {
            _pooledObjects.RemoveFirst();
        }
        instantiate(obj);
        
        _pooledObjects.AddLast(obj);
        obj.SetActive(true);

        return obj;
    }

    public override void Release(GameObject toRelease)
    {
        for (var node = _pooledObjects.First; node != null; node = node.Next)
        {
            if (!node.Value.activeInHierarchy || !ReferenceEquals(node.Value, toRelease)) continue;
            
            _pooledObjects.Remove(node);

            if (_pooledObjects.Count >= amountToPool)
            {
                Destroy(node.Value);
                break;
            }
            
            _pooledObjects.AddFirst(toRelease);
            toRelease.SetActive(false);

            break;
        }
    }
}
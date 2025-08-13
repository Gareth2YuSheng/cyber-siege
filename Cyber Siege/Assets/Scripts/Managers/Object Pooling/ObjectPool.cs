using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> pool = new Queue<GameObject>();
    private int size;
    private Transform parent;

    public ObjectPool(GameObject _prefab, int _size, Transform _parent)
    {
        prefab = _prefab;
        size = _size;
        parent = _parent;
        // Cannot Initialise pool objects here as this is not a MonoBehavior script
    }

    public GameObject GetPrefab()
    {
        return prefab;
    }

    public Transform GetParent()
    {
        return parent;
    }

    public int GetPoolSize()
    {
        return size;
    }

    public void SetPoolSize(int _size)
    {
        size = _size;
    }

    public int GetCurrentPoolSize()
    {
        return pool.Count;
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        GameObject obj = pool.Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        // Only set active after updating the position
        obj.SetActive(true);

        // Initialise pooled object script
        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}

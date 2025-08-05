using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager main;

    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform poolParent;
    }

    [SerializeField] private Pool[] pools;
    private Dictionary<string, ObjectPool> poolDictionary;

    private void Awake()
    {
        if (main != null && main != this)
        {
            Destroy(this);
        }
        else
        {
            main = this;
        }
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, ObjectPool>();

        // Initialise all the pools
        foreach (Pool pool in pools)
        {
            ObjectPool objectPool = new ObjectPool(pool.prefab, pool.size, pool.poolParent);
            poolDictionary.Add(pool.tag, objectPool);
            ExpandPool(pool.tag, objectPool.GetPoolSize());
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag <" + tag + "> does not exist!");
            return null;
        }

        ObjectPool pool = poolDictionary[tag];
        // Expand pool if empty
        if (pool.GetCurrentPoolSize() == 0)
        {
            ExpandPool(tag, Math.Max(pool.GetPoolSize(), 1));
            pool.SetPoolSize(Math.Max(pool.GetPoolSize(), 1) * 2);
        }

        return pool.GetObject(position, rotation);
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        poolDictionary[tag].ReturnObject(obj);
    }

    public void ExpandPool(string tag, int size)
    {
        ObjectPool pool = poolDictionary[tag];
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(pool.GetPrefab(), pool.GetParent());
            pool.ReturnObject(obj);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

public class PoolService : MonoBehaviour
{
    public static PoolService Instance { get; private set; }

    private class Pool
    {
        public GameObject prefab;
        public Queue<GameObject> poolQueue = new Queue<GameObject>();
    }

    private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // gi? PoolService khi chuy?n scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterPool(string key, GameObject prefab, int count)
    {
        if (pools.ContainsKey(key))
            return;

        Pool newPool = new Pool();
        newPool.prefab = prefab;

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);
            newPool.poolQueue.Enqueue(obj);
        }

        pools.Add(key, newPool);
    }

    public GameObject Spawn(string key, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogWarning($"Pool with key '{key}' not found.");
            return null;
        }

        Pool pool = pools[key];
        GameObject obj;

        if (pool.poolQueue.Count > 0)
        {
            obj = pool.poolQueue.Dequeue();
        }
        else
        {
            obj = Instantiate(pool.prefab);
            obj.transform.SetParent(this.transform);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    // Tr? object v? l?i pool.
    public void Despawn(string key, GameObject obj)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogWarning($"Pool with key '{key}' not found.");
            Destroy(obj); // fallback n?u không có pool
            return;
        }

        obj.SetActive(false);
        pools[key].poolQueue.Enqueue(obj);
    }
}

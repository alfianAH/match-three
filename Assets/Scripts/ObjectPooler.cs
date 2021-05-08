using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size = 30;
    }

    public List<Pool> pools; // To save all prefab pool
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    
    // Singleton for object pooler
    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        AddIntoPool();
    }
    
    /// <summary>
    /// To add prefab to pool
    /// </summary>
    private void AddIntoPool()
    {
        pools.ForEach((pool) =>
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        });
    }
    
    /// <summary>
    /// To get object from pool 
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        // If pool dictionary contains requested object
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Object with tag {tag} doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}

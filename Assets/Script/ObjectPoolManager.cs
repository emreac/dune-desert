using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject objectPrefab;
    public int initialPoolSize = 10;

    private Queue<GameObject> objectPool = new Queue<GameObject>();

    void Start()
    {
        InitializePool(initialPoolSize);
    }

    void InitializePool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        while (objectPool.Count > 0)
        {
            GameObject obj = objectPool.Dequeue();
            if (obj != null)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        Debug.LogWarning("Object pool exhausted. Increasing pool size.");
        ExpandPool();
        return GetObjectFromPool();
    }

    private void ExpandPool()
    {
        int newObjectsCount = initialPoolSize; // Add the same amount of objects as the initial pool size
        for (int i = 0; i < newObjectsCount; i++)
        {
            GameObject newObj = Instantiate(objectPrefab);
            newObj.SetActive(false);
            objectPool.Enqueue(newObj);
        }
        Debug.Log($"Expanded pool size by {newObjectsCount} objects.");
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("Attempted to return a null object to the pool.");
        }
    }
}

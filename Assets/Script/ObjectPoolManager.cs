using System.Collections;
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
        if (objectPool.Count == 0)
        {
            Debug.LogWarning("Object pool exhausted. Increasing pool size.");
            ExpandPool();
        }

        GameObject obj = objectPool.Dequeue();
        obj.SetActive(true);
        return obj;
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
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}

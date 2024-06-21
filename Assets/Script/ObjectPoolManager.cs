using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject objectPrefab;
    public int poolSize = 10; // Adjust as needed

    private Queue<GameObject> objectPool = new Queue<GameObject>();

    void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
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
        obj.SetActive(true); // Make sure the object is set active when retrieved
        return obj;
    }

    private void ExpandPool()
    {
        int currentPoolSize = objectPool.Count;
        for (int i = 0; i < poolSize - currentPoolSize; i++)
        {
            GameObject newObj = Instantiate(objectPrefab);
            newObj.SetActive(false);
            objectPool.Enqueue(newObj);
        }
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}

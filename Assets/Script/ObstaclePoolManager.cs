using System.Collections.Generic;
using UnityEngine;

public class ObstaclePoolManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public int initialPoolSize = 10;

    private Queue<GameObject> obstaclePool = new Queue<GameObject>();

    void Start()
    {
        InitializePool(initialPoolSize);
    }

    void InitializePool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab);
            obj.SetActive(false);
            obstaclePool.Enqueue(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        while (obstaclePool.Count > 0)
        {
            GameObject obj = obstaclePool.Dequeue();
            if (obj != null)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        Debug.LogWarning("Obstacle pool exhausted. Increasing pool size.");
        ExpandPool();
        return GetObjectFromPool();
    }

    private void ExpandPool()
    {
        int newObjectsCount = initialPoolSize; // Add the same amount of objects as the initial pool size
        for (int i = 0; i < newObjectsCount; i++)
        {
            GameObject newObj = Instantiate(obstaclePrefab);
            newObj.SetActive(false);
            obstaclePool.Enqueue(newObj);
        }
        Debug.Log($"Expanded obstacle pool size by {newObjectsCount} objects.");
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
            obstaclePool.Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("Attempted to return a null object to the pool.");
        }
    }
}

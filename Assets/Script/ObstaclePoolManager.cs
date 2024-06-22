using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePoolManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public int initialPoolSize = 10; // Initial pool size

    private Queue<GameObject> obstaclePool = new Queue<GameObject>();
    private HashSet<GameObject> allObstacles = new HashSet<GameObject>();

    void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("Obstacle prefab is not assigned.");
            return;
        }

        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewObstacle();
        }

        Debug.Log("Obstacle pool initialized with " + initialPoolSize + " objects.");
    }

    GameObject CreateNewObstacle()
    {
        GameObject obj = Instantiate(obstaclePrefab);
        obj.SetActive(false);
        obstaclePool.Enqueue(obj);
        allObstacles.Add(obj);
        return obj;
    }

    public GameObject GetObjectFromPool()
    {
        if (obstaclePool.Count == 0)
        {
            Debug.LogWarning("Obstacle pool exhausted. Expanding pool.");
            ExpandPool();
        }

        if (obstaclePool.Count > 0)
        {
            GameObject obj = obstaclePool.Dequeue();
            obj.SetActive(true);
            Debug.Log("Obstacle retrieved from pool. Pool size: " + obstaclePool.Count);
            return obj;
        }
        else
        {
            Debug.LogError("Failed to get object from pool after expansion.");
            return null;
        }
    }

    private void ExpandPool()
    {
        int newObjectsToCreate = initialPoolSize;

        for (int i = 0; i < newObjectsToCreate; i++)
        {
            CreateNewObstacle();
        }

        Debug.Log("Obstacle pool expanded. New pool size: " + obstaclePool.Count);
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        if (obj != null && allObstacles.Contains(obj))
        {
            obj.SetActive(false);
            if (!obstaclePool.Contains(obj))
            {
                obstaclePool.Enqueue(obj);
                Debug.Log("Obstacle returned to pool. Pool size: " + obstaclePool.Count);
            }
            else
            {
                Debug.LogWarning("Attempted to return an already pooled object: " + obj.name);
            }
        }
        else
        {
            Debug.LogError("Attempted to return a null or untracked object to the pool.");
        }
    }
}

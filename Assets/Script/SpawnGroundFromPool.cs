using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroundFromPool : MonoBehaviour
{
    public ObjectPoolManager poolManager; // Assign this in the Inspector
    public ObstaclePoolManager obstaclePoolManager; // Assign this in the Inspector
    public bool spawned;
    public int maxObstacles = 5; // Maximum number of obstacles to spawn

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !spawned)
        {
            if (poolManager == null)
            {
                Debug.LogError("Object pool manager reference is not set.");
                return;
            }
            if (obstaclePoolManager == null)
            {
                Debug.LogError("Obstacle pool manager reference is not set.");
                return;
            }

            Vector3 pos = new Vector3(transform.parent.position.x,
                                     transform.parent.position.y,
                                     transform.parent.position.z + 75.7f);
            GameObject newGround = poolManager.GetObjectFromPool();

            if (newGround != null)
            {
                newGround.transform.position = pos;
                newGround.transform.rotation = transform.parent.rotation;
                spawned = true;

                // Spawn obstacles on the new ground
                // Spawn obstacles
                for (int i = 0; i < 5; i++) // Adjust the number of obstacles as needed
                {
                    GameObject obstacle = obstaclePoolManager.GetObjectFromPool();
                    if (obstacle != null)
                    {
                        // Place the obstacle at a random position on the ground
                        Vector3 obstaclePos = new Vector3(
                            pos.x + Random.Range(-10f, 10f), // Adjust the range as needed
                            pos.y,
                            pos.z + Random.Range(-37.5f, 37.5f) // Adjust the range as needed
                        );
                        obstacle.transform.position = obstaclePos;
                        obstacle.transform.rotation = Quaternion.identity; // Adjust rotation as needed
                    }
                    else
                    {
                        Debug.LogError("Failed to get obstacle from pool.");
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to get object from pool. Object pool manager might not be properly initialized.");
            }
        }
    }

    

    // Example method to return the ground object to the pool when no longer needed
    public void ReturnGroundToPool(GameObject groundObject)
    {

        if (groundObject != null && poolManager != null)
        {
            poolManager.ReturnObjectToPool(groundObject);
        }
        //poolManager.ReturnObjectToPool(groundObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroundFromPool : MonoBehaviour
{
    public ObjectPoolManager poolManager; // Assign this in the Inspector
    public bool spawned;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !spawned)
        {
            if (poolManager == null)
            {
                Debug.LogError("Object pool manager reference is not set.");
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
        poolManager.ReturnObjectToPool(groundObject);
    }
}
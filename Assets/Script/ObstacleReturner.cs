using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleReturner : MonoBehaviour
{
    public ObstaclePoolManager obstaclePoolManager; // Assign this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle exiting, returning to pool: " + other.gameObject.name);
            obstaclePoolManager.ReturnObjectToPool(other.gameObject);
        }
    }
}

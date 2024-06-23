using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleReturner : MonoBehaviour
{
    private Transform eraser;
    private float velocity;
    public float eraserzSpeed = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Eraser"))
        {
            Debug.Log("Eraser!");
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        eraser = transform;
    }
    private void FixedUpdate()
    {
        eraser.position += Vector3.forward * eraserzSpeed * Time.fixedDeltaTime;
    }
}

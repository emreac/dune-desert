using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawning : MonoBehaviour
{

    public GameObject player;
    public GameObject obstacle;

    public float amountOfObstacle;


    public float minX, maxX;

    public void Start()
    {
        player = GameObject.FindWithTag("Player");

        for (int i = 0; i < amountOfObstacle; i++)
        {
            float xAXIS, zAXIS;

            xAXIS = Random.Range(minX, maxX);
            zAXIS = Random.Range(player.transform.localPosition.z + 50, player.transform.localPosition.z + 80);

            Vector3 pos = new Vector3(xAXIS, 0, zAXIS);
            Instantiate(obstacle.transform, pos, Quaternion.identity);

        }
    }
}

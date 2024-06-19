using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private float velocity;
    private Camera mainCam;
    public float roadEndPoint;

    private Transform player;
    private Vector3 firstMousePos, firstPlayerPos;

    private bool moveTheBall;

    private float camVelocity;
    public float camSpeed = 0.4f;
    private Vector3 offset;
    

    private void Start()
    {
        mainCam = Camera.main;
        player = this.transform;

        offset = mainCam.transform.position - player.position;

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            moveTheBall = true;

        }else if (Input.GetMouseButtonUp(0))
        {
            moveTheBall = false;

        }

        if (moveTheBall)
        {
            Plane newPlane = new Plane(Vector3.up, 0.8f);
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if(newPlane.Raycast(ray, out var distance))
            {
                Vector3 newMousePos = ray.GetPoint(distance) - firstMousePos;
                Vector3 newPlayerPos = newMousePos + firstPlayerPos;
                newPlayerPos.x = Mathf.Clamp(newPlayerPos.x, -roadEndPoint, roadEndPoint);
                player.position = new Vector3(Mathf.SmoothDamp(player.position.x, newPlayerPos.x,
                    ref velocity, speed * Time.deltaTime), player.position.y, player.position.z);
            }
        }
    }

    private void LateUpdate()
    {
        Vector3 newCamPos = mainCam.transform.position;
        mainCam.transform.position = new Vector3(Mathf.SmoothDamp(newCamPos.x, player.position.x, ref camVelocity,
            camSpeed * Time.deltaTime),newCamPos.y,player.position.z+offset.z);
    }
}

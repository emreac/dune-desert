using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Animator
    public Animator c2Animator;

    //Player Movement
    public float speed;
    private float velocity;
    private Camera mainCam;
    public float roadEndPoint;
    private Transform player;
    private Vector3 firstMousePos, firstPlayerPos;

    //Player State
    private bool moveTheBall;
    private bool isGameOver;

    //Camera
    private float camVelocity;
    public float camSpeed = 0.4f;
    private Vector3 offset;

    //Player Speed
    public float playerzSpeed = 15f;

    //Rotation
    private Vector3 previousPosition;
    public float rotationSpeed = 5f;
    public float returnRotationSpeed = 2f; // Speed for returning to the neutral rotation
    public float maxRotationAngle = 45f; // Maximum angle to rotate

    //Trail Renderer
    private TrailRenderer trailRenderer;

    private void Start()
    {
        c2Animator.Rebind();
        mainCam = Camera.main;
        player = this.transform;
        offset = mainCam.transform.position - player.position;
        previousPosition = player.position; // Initialize the previous position

        // Get the TrailRenderer component and disable it
        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false; // Disable the TrailRenderer initially
        }

        // Start the coroutine to enable the TrailRenderer after 1 second
        StartCoroutine(EnableTrailRendererAfterDelay(1f));
    }

    private IEnumerator EnableTrailRendererAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true; // Enable the TrailRenderer after the delay
        }
    }

    private void Update()
    {
        if (isGameOver) return;

        if (Input.GetMouseButtonDown(0))
        {
            moveTheBall = true;

            // Capture the initial positions when the mouse button is pressed
            Plane newPlane = new Plane(Vector3.up, 0.8f);
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (newPlane.Raycast(ray, out var distance))
            {
                firstMousePos = ray.GetPoint(distance);
                firstPlayerPos = player.position;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            moveTheBall = false;
        }

        if (moveTheBall)
        {
            Plane newPlane = new Plane(Vector3.up, 0.8f);
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (newPlane.Raycast(ray, out var distance))
            {
                Vector3 newMousePos = ray.GetPoint(distance);
                Vector3 deltaMousePos = newMousePos - firstMousePos;
                Vector3 newPlayerPos = deltaMousePos + firstPlayerPos;
                newPlayerPos.x = Mathf.Clamp(newPlayerPos.x, -roadEndPoint, roadEndPoint);
                player.position = new Vector3(Mathf.SmoothDamp(player.position.x, newPlayerPos.x,
                    ref velocity, speed * Time.deltaTime), player.position.y, player.position.z);

                // Determine the direction of movement
                Vector3 direction = newPlayerPos - previousPosition;
                if (direction != Vector3.zero)
                {
                    float angle = Mathf.Clamp(direction.x * rotationSpeed, -maxRotationAngle, maxRotationAngle);
                    Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
                    player.rotation = Quaternion.Lerp(player.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                }

                previousPosition = player.position; // Update the previous position
            }
        }
        else
        {
            // Smoothly reset rotation to zero when not moving
            player.rotation = Quaternion.Lerp(player.rotation, Quaternion.identity, Time.deltaTime * returnRotationSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (isGameOver) return;
        player.position += Vector3.forward * playerzSpeed * Time.fixedDeltaTime;
    }

    private void LateUpdate()
    {
        Vector3 newCamPos = mainCam.transform.position;
        mainCam.transform.position = new Vector3(Mathf.SmoothDamp(newCamPos.x, player.position.x, ref camVelocity,
            camSpeed * Time.deltaTime), newCamPos.y, player.position.z + offset.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            c2Animator.SetTrigger("isDead");
            isGameOver = true;
            GameManager.instance.GameOver();
        }
    }
}

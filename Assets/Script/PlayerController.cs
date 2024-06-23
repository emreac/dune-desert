using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Animator c2Animator;
    public float speed;
    private float velocity;
    private Camera mainCam;
    public float roadEndPoint;

    public GameObject gameOverUI;
    private Transform player;
    private Vector3 firstMousePos, firstPlayerPos;
    private bool moveTheBall;
    private bool isGameOver;

    private float camVelocity;
    public float camSpeed = 0.4f;
    private Vector3 offset;

    public float playerzSpeed = 15f;
    private Vector3 previousPosition;
    public float rotationSpeed = 5f;
    public float returnRotationSpeed = 2f;
    public float maxRotationAngle = 45f;

    private TrailRenderer trailRenderer;

    private void Start()
    {
        if (c2Animator == null)
        {
            Debug.LogError("Animator not assigned");
        }

        mainCam = Camera.main;
        player = transform;
        offset = mainCam.transform.position - player.position;
        previousPosition = player.position;

        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }

        StartCoroutine(EnableTrailRendererAfterDelay(1f));
    }

    private IEnumerator EnableTrailRendererAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }
    }

    private void Update()
    {
        if (isGameOver) return;

        if (Input.GetMouseButtonDown(0))
        {
            moveTheBall = true;
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

                Vector3 direction = newPlayerPos - previousPosition;
                if (direction != Vector3.zero)
                {
                    float angle = Mathf.Clamp(direction.x * rotationSpeed, -maxRotationAngle, maxRotationAngle);
                    Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
                    player.rotation = Quaternion.Lerp(player.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                }

                previousPosition = player.position;
            }
        }
        else
        {
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
        if (other.gameObject.CompareTag("Obstacle"))
        {
            c2Animator.SetTrigger("isDead");
            isGameOver = true;
            StartCoroutine(GameOverSequence());
           
        }
    }
    private IEnumerator GameOverSequence()
    {

        yield return new WaitForSeconds(0.5f);
        gameOverUI.SetActive(true);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        // Load the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

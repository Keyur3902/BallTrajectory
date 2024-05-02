using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallProjection : MonoBehaviour
{
    public float initialSpeed = 20f;
    public float launchAngle = 45f;
    public float gravityScale = 0.5f;
    private Rigidbody rb;
    public LineRenderer trajectoryLine;
    public GameObject ballPrefab;
    private bool isBallActive = false;

    public int linePoints = 100;
    public float pointSpacing = 0.1f;
    private bool isBallLaunched = false;
    private bool hasCollided = false;
    public float angleVariance = 15f; 

    private List<Vector3> trajectoryPoints = new List<Vector3>();

    private void Start()
    {
        rb = ballPrefab.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isBallActive && isBallLaunched == false)
        {
            SpawnBall();
            isBallActive = true;
            isBallLaunched = true;
        }
        if (Input.touchCount>0 && !isBallActive && isBallLaunched == false)
        {
            SpawnBall();
            isBallActive = true;
            isBallLaunched = true;
        }
    }
    private void LaunchBall(GameObject ball)
    {

        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            // Create a new ball instance from the prefab
            // GameObject newBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);

            // Calculate launch direction with variation on the base angle
            // float currentAngle = launchAngle + Random.Range(-angleVariance / 2f, angleVariance / 2f);
            // float angleRad = currentAngle * Mathf.Deg2Rad;
            // Vector3 launchDirection = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f).normalized;

            float randomX = Random.Range(0, 1f);
            float randomY = Random.Range(0, 1f);
            float randomZ = Random.Range(-1f, 1f);
            Vector3 randomDirection = new Vector3(randomX, randomY, randomZ).normalized;

            // Apply physics and launch the ball
            // Rigidbody newRb = newBall.GetComponent<Rigidbody>();
            Physics.gravity = new Vector3(0, -9.81f * gravityScale, 0);
            Vector3 launchForce = randomDirection * initialSpeed * ballRb.mass;
            ballRb.AddForce(launchForce, ForceMode.Impulse);
            isBallLaunched = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            hasCollided = true;
            DrawTrajectoryLine();
            rb.velocity = Vector3.zero;
            rb.isKinematic = false;
            isBallActive = false;
        }
    }

    private void DrawTrajectoryLine()
    {
        Vector3[] points = trajectoryPoints.ToArray();

        // Set positions of LineRenderer to draw the trajectory
        trajectoryLine.positionCount = trajectoryPoints.Count;
        trajectoryLine.SetPositions(points);
    }

    private void CalculateTrajectoryPoints()
    {
        Vector3 currentPosition = transform.position;
        Vector3 currentVelocity = rb.velocity;

        // Calculate trajectory points using projectile motion equations
        for (int i = 0; i < linePoints; i++)
        {
            float time = i * pointSpacing;
            Vector3 point = CalculatePointAtTime(currentPosition, currentVelocity, time);
            trajectoryPoints.Add(point);
        }
    }

    private Vector3 CalculatePointAtTime(Vector3 position, Vector3 velocity, float time)
    {
        float gravity = Physics.gravity.magnitude;
        Vector3 gravityVector = Vector3.down * gravity;

        Vector3 horizontalPosition = position + velocity * time;
        Vector3 verticalVelocity = velocity + gravityVector * time;
        Vector3 verticalPosition = position + 0.5f * (velocity + verticalVelocity) * time;

        return new Vector3(horizontalPosition.x, verticalPosition.y, horizontalPosition.z);
    }

    private void FixedUpdate()
    {
        if (!hasCollided)
        {
            CalculateTrajectoryPoints();
        }
    }

    private void SpawnBall(){
        GameObject newBall = Instantiate(ballPrefab, new Vector3(0, 0.6f, 0), Quaternion.identity);
        hasCollided = false;
        LaunchBall(newBall);
    }
}


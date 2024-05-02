using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryVisualization : MonoBehaviour
{
    // public LineRenderer lineRenderer;
    // public int numPoints = 100;
    // public float pointSpacing = 0.1f;

    // private void Start()
    // {
    //     lineRenderer.positionCount = numPoints;
    // }

    // public void UpdateTrajectory(Vector3 initialPosition, Vector3 initialVelocity)
    // {
    //     for (int i = 0; i < numPoints; i++)
    //     {
    //         float time = i * pointSpacing;
    //         Vector3 pointPosition = CalculatePointAtTime(initialPosition, initialVelocity, time);
    //         lineRenderer.SetPosition(i, pointPosition);
    //     }
    // }

    // private Vector3 CalculatePointAtTime(Vector3 initialPosition, Vector3 initialVelocity, float time)
    // {
    //     // Use projectile motion equations to calculate position at a given time
    //     Vector3 newPosition = initialPosition + initialVelocity * time + 0.5f * Physics.gravity * time * time;
    //     return newPosition;
    // }




    public LineRenderer lineRenderer;
    public int numPoints = 100;
    public float pointSpacing = 0.1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void DrawTrajectory()
    {
        Vector3[] points = new Vector3[numPoints];
        Vector3 currentVelocity = rb.velocity;
        Vector3 currentPosition = transform.position;

        for (int i = 0; i < numPoints; i++)
        {
            float time = i * pointSpacing;
            Vector3 pointPosition = CalculatePointAtTime(currentPosition, currentVelocity, time);
            points[i] = pointPosition;
        }

        lineRenderer.positionCount = numPoints;
        lineRenderer.SetPositions(points);
    }

    private Vector3 CalculatePointAtTime(Vector3 initialPosition, Vector3 initialVelocity, float time)
    {
        // Use projectile motion equations to calculate position at a given time
        Vector3 newPosition = initialPosition + initialVelocity * time + 0.5f * Physics.gravity * time * time;
        return newPosition;
    }
}

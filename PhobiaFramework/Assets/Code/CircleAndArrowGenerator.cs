using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAndArrowGenerator : MonoBehaviour
{
    public Material standardMat;
    public Material hoverMat;
    public Material dragMat;

    public GameObject arrowPrefab;
    public Transform objectToSurround;
    public int circleResolution = 30; // Number of vertices for the circle
    public float circleRadius = 1f; // Radius of the circle
    public float lineWidth = 0.02f; // Thickness of the circle line

    private LineRenderer lineRenderer;
    private GameObject arrow;
    private Transform mainCameraTransform; // Reference to the main camera's transform



    void Start()
    {
        // Create and configure LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = circleResolution + 1; // Add one extra point for closing the loop
        lineRenderer.useWorldSpace = false; // Use local space
        lineRenderer.startWidth = lineRenderer.endWidth = lineWidth; // Set line width
        lineRenderer.material = standardMat; // Assign material

        UpdateCircle();

        // Instantiate arrow object
        arrow = Instantiate(arrowPrefab, transform);
        UpdateArrowPosition();

        // Get reference to the main camera's transform
        mainCameraTransform = Camera.main.transform;


    }

    // Update is called once per frame
    void Update()
    {
        UpdateCircle();
        UpdateArrowPosition();

        // Ensure the GameObject faces the camera (except rotation around y-axis)
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
            mainCameraTransform.rotation * Vector3.up);
    }

    void UpdateCircle()
    {
        // Calculate circle vertices
        for (int i = 0; i < circleResolution; i++)
        {
            float angle = (i / (float)circleResolution) * 2 * Mathf.PI;
            float x = Mathf.Cos(angle) * circleRadius;
            float y = Mathf.Sin(angle) * circleRadius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));
        }
        lineRenderer.SetPosition(circleResolution, lineRenderer.GetPosition(0));

    }

    void UpdateArrowPosition()
    {
        // Calculate arrow position at 45-degree angle
        float angle = 45f * Mathf.Deg2Rad;
        float arrowX = Mathf.Cos(angle) * circleRadius;
        float arrowY = Mathf.Sin(angle) * circleRadius;
        arrow.transform.localPosition = new Vector3(arrowX, arrowY, 0f); // Positive X position

        // Rotate the arrow to -45 degrees
        arrow.transform.localRotation = Quaternion.Euler(0f, 0f, -45f);
    }
}

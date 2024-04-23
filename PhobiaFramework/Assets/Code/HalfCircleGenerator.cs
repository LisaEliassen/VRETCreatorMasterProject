using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfCircleGenerator : MonoBehaviour
{
    public Material standardMat;
    public Material hoverMat;
    public Material dragMat;

    public Material circleMat;

    public GameObject rotateButtonPrefab;
    public int circleResolution = 40; // Number of vertices for the circle
    public float circleRadius = 1f; // Radius of the circle
    public float lineWidth = 0.02f; // Thickness of the circle line
    public float startAngleDegrees = 0f; // Starting angle for the draggable GameObject (in degrees)

    private LineRenderer lineRenderer;
    public GameObject rotateButton;
    private Transform mainCameraTransform; // Reference to the main camera's transform

    // Start is called before the first frame update
    void Start()
    {
        // Create and configure LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = circleResolution + 1; // Add one extra point for closing the loop
        lineRenderer.useWorldSpace = false; // Use local space
        lineRenderer.startWidth = lineRenderer.endWidth = lineWidth; // Set line width
        lineRenderer.material = circleMat; // Assign material

        UpdateHalfCircle();

        // Instantiate draggable GameObject
        rotateButton = Instantiate(rotateButtonPrefab, transform);

        // Position draggable GameObject in the middle of the half circle
        Vector3 buttonPosition = CalculateButtonPosition();
        rotateButton.transform.localPosition = buttonPosition;
    }

    void UpdateHalfCircle()
    {
        lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
        // Calculate half circle vertices
        for (int i = 0; i <= circleResolution; i++)
        {
            float angle = Mathf.Lerp(0, Mathf.PI, i / (float)circleResolution); // Interpolate angle between 0 and pi
            float x = Mathf.Cos(angle) * circleRadius;
            float y = Mathf.Sin(angle) * circleRadius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));
        }
    }

    Vector3 CalculateButtonPosition()
    {
        float angleRadians = startAngleDegrees * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRadians) * circleRadius;
        float y = Mathf.Sin(angleRadians) * circleRadius;
        return new Vector3(x, y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

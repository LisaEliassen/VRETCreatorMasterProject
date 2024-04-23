using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAndArrowGenerator : MonoBehaviour
{
    public Material standardMat;
    public Material hoverMat;
    public Material dragMat;

    public GameObject arrowPrefab;
    public GameObject scaleButtonPrefab;
    public Transform objectToSurround;
    public int circleResolution = 40; // Number of vertices for the circle
    public float circleRadius = 1f; // Radius of the circle
    public float lineWidth = 0.02f; // Thickness of the circle line

    private LineRenderer lineRenderer;
    private GameObject arrow;
    public GameObject scaleButton;
    private Transform mainCameraTransform; // Reference to the main camera's transform

    Transform parent;

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
        //arrow = Instantiate(arrowPrefab, transform);
        scaleButton = Instantiate(scaleButtonPrefab, transform);

        //UpdateArrowPosition();
        UpdateButtonPosition();

        // Get reference to the main camera's transform
        mainCameraTransform = Camera.main.transform;

        this.parent = transform.parent;

        while (this.parent.parent != null)
        {
            this.parent = parent.parent;
        }

        BoxCollider boxCollider = parent.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            // Calculate the radius based on the box collider's size
            float colliderSize = Mathf.Max(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z);
            this.circleRadius = colliderSize * 0.9f; // Adjust the multiplier as needed to control the size
        }
        else
        {
            Debug.LogWarning("No BoxCollider found on the parent object.");
        }

        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCircle();
        //UpdateArrowPosition();
        UpdateButtonPosition();

        // Ensure the GameObject faces the camera (except rotation around y-axis)
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
            mainCameraTransform.rotation * Vector3.up);
    }

    void UpdateCircle()
    {
        lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
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

    void UpdateButtonPosition()
    {
        // Calculate arrow position at 45-degree angle
        float angle = 45f * Mathf.Deg2Rad;
        float arrowX = Mathf.Cos(angle) * circleRadius;
        float arrowY = Mathf.Sin(angle) * circleRadius;
        scaleButton.transform.localPosition = new Vector3(arrowX, arrowY, 0f); // Positive X position
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

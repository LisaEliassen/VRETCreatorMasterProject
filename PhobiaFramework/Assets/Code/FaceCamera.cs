using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        // Find the main camera
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        // Check if the main camera exists
        if (mainCamera != null)
        {
            // Calculate the direction from the canvas to the camera
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;

            // Face the canvas towards the camera using LookRotation
            transform.rotation = Quaternion.LookRotation(directionToCamera, Vector3.up);
        }
    }
}
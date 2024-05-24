using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The script provides functionality to reset the position of an XR rig GameObject to its original position when a button is clicked.
// The original position is stored in a Vector3 variable named originalPosition, which is recorded when the scene starts.
// The OnResetButtonClick method is called when the button is clicked, resetting the XR rig's position to its original position.

public class StopExposure : MonoBehaviour
{
    public GameObject xrRig; // Reference to your XR rig GameObject
    public Button stopExposureButton;
    private Vector3 originalPosition; // Variable to store the original position

    private void Start()
    {
        // Record the original position when the scene starts
        originalPosition = xrRig.transform.position;

        // Add listener for the button click event
        stopExposureButton.onClick.AddListener(OnResetButtonClick);

    }

    public void OnResetButtonClick()
    {
        // Move XR rig back to its original position
        xrRig.transform.position = originalPosition;
    }
}

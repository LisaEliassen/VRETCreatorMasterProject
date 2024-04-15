using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

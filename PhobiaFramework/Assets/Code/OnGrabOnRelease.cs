using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OnGrabOnRelease : MonoBehaviour
{
    private Rigidbody rb;
    private bool isGrabbed = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        // Register event listeners for grab and release events
        if (grabInteractable != null)
        {
            grabInteractable.onSelectEntered.AddListener(OnGrab);
            grabInteractable.onSelectExited.AddListener(OnRelease);
        }
        else
        {
            Debug.LogWarning("XRGrabInteractable component not found on object.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGrab(XRBaseInteractor interactor)
    {
        // Handle object grabbed event
        Debug.Log("Object grabbed");
        isGrabbed = true;
        rb.isKinematic = false; // Turn off isKinematic
        // Add any additional functionality you need here
    }

    private void OnRelease(XRBaseInteractor interactor)
    {
        // Handle object released event
        Debug.Log("Object released");
        isGrabbed = false;
        rb.isKinematic = true;
        // Add any additional functionality you need here
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isGrabbed && collision.collider.CompareTag("Scenery"))
        {
            rb.isKinematic = false; // Turn off isKinematic when colliding with a box collider
        }
    }
}

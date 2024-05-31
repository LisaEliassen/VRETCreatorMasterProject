#region License
// Copyright (C) 2024 Lisa Maria Eliassen & Olesya Pasichnyk
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the Commons Clause License version 1.0 with GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// Commons Clause License and GNU General Public License for more details.
// 
// You should have received a copy of the Commons Clause License and GNU General Public License
// along with this program. If not, see <https://commonsclause.com/> and <https://www.gnu.org/licenses/>.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// This script is designed to handle events when an object is grabbed and released using XR grab interaction in Unity. 

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
        rb.isKinematic = false;
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

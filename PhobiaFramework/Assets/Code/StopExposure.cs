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

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
using TMPro;

// This script allows for the dynamic scaling of specific game objects (sphere, exposureScene, exposureRoom, waitingRoom, doors) in the scene.

public class ScaleGameObjects : MonoBehaviour
{
    public GameObject sphere;
    public GameObject exposureScene;
    public GameObject exposureRoom;
    public GameObject waitingRoom;
    public GameObject doors;

    public Slider sphereSlider;
    public TMP_InputField sizeInputSphere;
    public Slider platformSlider;
    public TMP_InputField sizeInputPlatform;

    private Vector3 initialScale;   // Initial scale of the room to scale
    private float initialZPosition; // Initial Z position of the room to scale

    private void Start()
    {
        //hingeJoint1 = door1.GetComponent<HingeJoint>();
        //hingeJoint2 = door2.GetComponent<HingeJoint>();
        initialScale = exposureRoom.transform.localScale;
        initialZPosition = exposureScene.transform.position.z;

        // Add listeners to detect changes in the input field values
        sizeInputSphere.onValueChanged.AddListener(delegate { UpdateSphereFromInputField(); });
        sizeInputPlatform.onValueChanged.AddListener(delegate { UpdatePlatformFromInputField(); });

        sphereSlider.onValueChanged.AddListener(UpdateSphereScale);
        platformSlider.onValueChanged.AddListener(UpdatePlatformScale);

        sphereSlider.interactable = true;
        sizeInputSphere.interactable = true;
        platformSlider.interactable = true;
        sizeInputPlatform.interactable = true;
        sizeInputSphere.interactable = true;
        sizeInputPlatform.interactable = true;
        SetInitialValues();

    }

    private void UpdateSphereFromInputField()
    {
        float value;
        if (float.TryParse(sizeInputSphere.text, out value))
        {
            sphereSlider.value = value;
        }
    }

    private void UpdatePlatformFromInputField()
    {
        float value;
        if (float.TryParse(sizeInputPlatform.text, out value))
        {
            platformSlider.value = value;
        }
    }


    private void SetInitialValues()
    {
        sphereSlider.value = 30;
        ((TextMeshProUGUI)sizeInputSphere.placeholder).text = "30";

        platformSlider.value = 10;
        ((TextMeshProUGUI)sizeInputPlatform.placeholder).text = "10";

        //scaling from 5-20, but default value is 10
        exposureRoom.transform.localScale = new Vector3(2, exposureRoom.transform.localScale.y, 2);
        float newPositionZ = initialZPosition + (5 * (10 / 5) - 5) / 2; 
        exposureScene.transform.position = new Vector3(exposureScene.transform.position.x, exposureScene.transform.position.y, newPositionZ+0.5f);
    }

    private void UpdatePlatformScale(float scaleValue)
    {
        UpdatePlatformFromInputField();

        // Update the scale of the platform based on the value of the platform slider
        exposureRoom.transform.localScale = new Vector3(scaleValue/5, exposureRoom.transform.localScale.y, scaleValue/5);

        sizeInputPlatform.text = scaleValue.ToString();

        // Adjust the position of the scaled room to prevent intersection
        float newPositionZ = initialZPosition + (5 * (scaleValue / 5) - 5) / 2; 

        // Apply the new position
        exposureScene.transform.position = new Vector3(exposureScene.transform.position.x, exposureScene.transform.position.y, newPositionZ);
    

    }

    private void UpdateSphereScale(float scaleValue)
    {
        UpdateSphereFromInputField();

        // Update the scale of the sphere based on the value of the sphere slider
        //float sphereScale = sphereSlider.value;
        sphere.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

        sizeInputSphere.text = scaleValue.ToString();
    }

}

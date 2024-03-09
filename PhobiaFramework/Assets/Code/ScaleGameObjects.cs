using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Specialized;

public class ScaleGameObjects : MonoBehaviour
{
    public GameObject sphere;
    public GameObject exposureScene;
    public GameObject waitingRoom;
    public GameObject doorWay;

    public Slider sphereSlider;
    public TMP_InputField sizeInputSphere;
    public Slider platformSlider;
    public TMP_InputField sizeInputPlatform;

    private void Start()
    {
        // Add listeners to detect changes in the input field values
        sizeInputSphere.onValueChanged.AddListener(delegate { UpdateSphereFromInputField(); });
        sizeInputPlatform.onValueChanged.AddListener(delegate { UpdatePlatformFromInputField(); });

        sphereSlider.interactable = true;
        sizeInputSphere.interactable = true;
        platformSlider.interactable = true;
        sizeInputPlatform.interactable = true;
        sizeInputSphere.interactable = true;
        sizeInputPlatform.interactable = true;
        SetInitialValues();

    }

    private void Update()
    {
        // Call the method to update the scale of the objects
        UpdateSphereScale();
        // implement here to disable the hinge joint of the doors and activate it after scaling is finished to fix the issue?
        UpdatePlatformScale();
    }

    private void UpdateSphereFromInputField()
    {
        float value;
        if (float.TryParse(sizeInputSphere.text, out value))
        {
            sphereSlider.value = value;
            UpdateSphereScale();
        }
    }

    private void UpdatePlatformFromInputField()
    {
        float value;
        if (float.TryParse(sizeInputPlatform.text, out value))
        {
            platformSlider.value = value;
            UpdatePlatformScale();
        }
    }


    private void SetInitialValues()
    {
        sphereSlider.value = 30;
        ((TextMeshProUGUI)sizeInputSphere.placeholder).text = "30";

        platformSlider.value = 10;
        ((TextMeshProUGUI)sizeInputPlatform.placeholder).text = "10";
    }

    private void UpdatePlatformScale()
    {
        // Update the scale of the platform based on the value of the platform slider
        float platformScale = platformSlider.value;
        exposureScene.transform.localScale = new Vector3(platformScale / 10, exposureScene.transform.localScale.y, platformScale / 10);

        sizeInputPlatform.text = platformScale.ToString();
    }

    private void UpdateSphereScale()
    {
        // Update the scale of the sphere based on the value of the sphere slider
        float sphereScale = sphereSlider.value;
        sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);

        sizeInputSphere.text = sphereScale.ToString();
    }

}

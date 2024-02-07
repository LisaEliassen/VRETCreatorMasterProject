using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleGameObjects : MonoBehaviour
{
    public GameObject sphere;
    public GameObject platform;

    public Slider sphereSlider;
    public TMP_InputField sizeInputSphere;
    public Slider platformSlider;
    public TMP_InputField sizeInputPlatform;

    private void Start()
    {
        // Set initial values for the sliders and objects
        
        sphereSlider.interactable = true;
        sizeInputSphere.interactable = true;
        platformSlider.interactable = true;
        sizeInputPlatform.interactable = true;
        SetInitialValues();
    }

    private void Update()
    {
        // Call the method to update the scale of the objects
        UpdateObjectScales();
    }

    private void SetInitialValues()
    {

        sphereSlider.value = 30;
        ((TextMeshProUGUI)sizeInputSphere.placeholder).text = "30";

        // Set initial scale for the platform
        //platform.transform.localScale = new Vector3(10f, platform.transform.localScale.y, 10f);
        platformSlider.value = 10;
        ((TextMeshProUGUI)sizeInputPlatform.placeholder).text = "10";
    }

    private void UpdateObjectScales()
    {
        // Update the scale of the sphere based on the value of the sphere slider
        float sphereScale = sphereSlider.value;
        sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);

        ((TextMeshProUGUI)sizeInputSphere.placeholder).text = sphereScale.ToString();
        sizeInputSphere.text = sphereScale.ToString();

        // Update the scale of the platform based on the value of the platform slider
        float platformScale = platformSlider.value;
        platform.transform.localScale = new Vector3(platformScale, platform.transform.localScale.y, platformScale);

        ((TextMeshProUGUI)sizeInputPlatform.placeholder).text = platformScale.ToString();
        sizeInputPlatform.text = platformScale.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleGameObjects : MonoBehaviour
{
    public GameObject sphere;
    public GameObject exposureAndWaitingScene;
    //public GameObject waitingRoom;

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
        UpdateObjectScales();
    }

    private void UpdateSphereFromInputField()
    {
        float value;
        if (float.TryParse(sizeInputSphere.text, out value))
        {
            sphereSlider.value = value;
            UpdateObjectScales();
        }
    }

    private void UpdatePlatformFromInputField()
    {
        float value;
        if (float.TryParse(sizeInputPlatform.text, out value))
        {
            platformSlider.value = value;
            UpdateObjectScales();
        }
    }


    private void SetInitialValues()
    {

        sphereSlider.value = 30;
        ((TextMeshProUGUI)sizeInputSphere.placeholder).text = "30";

        platformSlider.value = 10;
        ((TextMeshProUGUI)sizeInputPlatform.placeholder).text = "10";
    }

    private void UpdateObjectScales()
    {
        // Update the scale of the sphere based on the value of the sphere slider
        float sphereScale = sphereSlider.value;
        sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);

        //((TextMeshProUGUI)sizeInputSphere.placeholder).text = sphereScale.ToString();
        sizeInputSphere.text = sphereScale.ToString();

        // Update the scale of the platform based on the value of the platform slider
        float platformScale = platformSlider.value;
        exposureAndWaitingScene.transform.localScale = new Vector3(platformScale / 10, exposureAndWaitingScene.transform.localScale.y, platformScale / 10);
        //waitingRoom.transform.localScale = new Vector3(platformScale / 10, waitingRoom.transform.localScale.y, platformScale / 10);

        //((TextMeshProUGUI)sizeInputPlatform.placeholder).text = platformScale.ToString();
        sizeInputPlatform.text = platformScale.ToString();
    }
}

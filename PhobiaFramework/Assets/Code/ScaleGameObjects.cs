using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    public GameObject door4;

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
    }

    private void UpdatePlatformScale(float scaleValue)
    {
        UpdatePlatformFromInputField();

        // Update the scale of the platform based on the value of the platform slider
        exposureRoom.transform.localScale = new Vector3(scaleValue/10, exposureRoom.transform.localScale.y, scaleValue/10);

        sizeInputPlatform.text = scaleValue.ToString();

        // Calculate the distance between the centers of the two rooms
        float distanceBetweenRooms = Mathf.Abs(exposureScene.transform.position.z + waitingRoom.transform.position.z);
        Debug.Log(distanceBetweenRooms);

        // Calculate the offset
        float offset = (exposureRoom.transform.localScale.z - initialScale.z) / 2f;

        // Adjust the position of the scaled room to prevent intersection
        float newPositionZ = initialZPosition + (10 * (scaleValue / 10) - 10) / 2; 

        // Apply the new position
        exposureScene.transform.position = new Vector3(exposureScene.transform.position.x, exposureScene.transform.position.y, newPositionZ);
        
        if(scaleValue < 10)
        {
            door1.SetActive(false);
            door2.SetActive(false);
            door3.SetActive(false);
            door4.SetActive(false);
        }
        else
        {
            door1.SetActive(true);
            door2.SetActive(true);
            door3.SetActive(true);
            door4.SetActive(true);
        }

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

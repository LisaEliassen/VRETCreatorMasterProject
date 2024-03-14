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
    public GameObject exposureRoom;
    public GameObject waitingRoom;
    public GameObject doors;

    public GameObject door1;
    public GameObject door2;
    private HingeJoint hingeJoint1;
    private HingeJoint hingeJoint2;

    public Slider sphereSlider;
    public TMP_InputField sizeInputSphere;
    public Slider platformSlider;
    public TMP_InputField sizeInputPlatform;

    public double previousScaleValue;
    public double currentScaleValue;

    private void Start()
    {
        //hingeJoint1 = door1.GetComponent<HingeJoint>();
        //hingeJoint2 = door2.GetComponent<HingeJoint>();

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

    private void Update()
    {
        //UpdateSphereScale();
        //UpdatePlatformScale();
        /*
        hingeJoint1 = door1.GetComponent<HingeJoint>();
        hingeJoint2 = door2.GetComponent<HingeJoint>();
        // Call the method to update the scale of the objects
        
        Destroy(hingeJoint1);
        Destroy(hingeJoint2);
        // implement here to disable the hinge joint of the doors and activate it after scaling is finished to fix the issue?
        print("Before");
        print(doors.transform.position);
        print("");
        
        print("After");
        print(doors.transform.position);
        door1.AddComponent<HingeJoint>();
        door2.AddComponent<HingeJoint>();

        hingeJoint1 = door1.GetComponent<HingeJoint>();
        hingeJoint2 = door2.GetComponent<HingeJoint>();

        // Set the limits
        JointLimits limits = hingeJoint1.limits;
        limits.min = 0f; // Minimum angle
        limits.max = 90f; // Maximum angle
        hingeJoint1.limits = limits;
        hingeJoint2.limits = limits;

        /*
        // Set connected anchor
        Vector3 anchor1 = new Vector3(1.030685f, 1.004f, -4.9399f);
        hingeJoint1.connectedAnchor = anchor1;
        Vector3 anchor2 = new Vector3(-1.0064f, 1.004f, -5.078898f);
        hingeJoint2.connectedAnchor = anchor2;
        */
        
        
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
        previousScaleValue = 10;
    }

    private void UpdatePlatformScale(float scaleValue)
    {
        UpdatePlatformFromInputField();

        float platformScale = platformSlider.value;
        // Update the scale of the platform based on the value of the platform slider

        exposureRoom.transform.localScale = new Vector3(scaleValue/10, exposureRoom.transform.localScale.y, scaleValue/10);

        sizeInputPlatform.text = scaleValue.ToString();

        /*
        if(previousScaleValue < platformScale)
        {
            previousScaleValue = platformScale;
            Vector3 newPosition = doors.transform.position - new Vector3(0, 0, ((platformScale / 10) * 2.2f - 2.2f) / 2f);
            doors.transform.position = newPosition;

            Vector3 waitingRoomNewPosition = waitingRoom.transform.position - new Vector3(0,0,5) - new Vector3(0, 0, exposureScene.transform.localScale.z * 5);
            waitingRoom.transform.position = waitingRoomNewPosition;
        }

        if (previousScaleValue > platformScale)
        {
            previousScaleValue = platformScale;
            Vector3 newPosition = doors.transform.position + new Vector3(0, 0, ((platformScale / 10) * 2.2f - 2.2f) / 2f);
            doors.transform.position = newPosition;

            Vector3 waitingRoomNewPosition = waitingRoom.transform.position + new Vector3(0, 0, 5) + new Vector3(0, 0, exposureScene.transform.localScale.z * 5);
            waitingRoom.transform.position = waitingRoomNewPosition;
        }*/


        /*float new_z = (5/10) * (exposureRoom.transform.localScale.z / 10) - 5;
        Vector3 newPos = new Vector3(exposureScene.transform.position.x, exposureScene.transform.position.y, exposureScene.transform.position.z + new_z);

        exposureScene.transform.position = newPos;*/

        float zOffset = (exposureRoom.transform.localScale.z - 1f) * 0.5f * exposureRoom.transform.localScale.z;
        exposureScene.transform.position += new Vector3(0f, 0f, zOffset);

        //exp_pos = wait_pos - 5 - exposureScene.transform.localScale * 5
        // door walls: pos_x +- (scale * size_x - size_x) / 2

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

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
    public GameObject doors;

    public GameObject door1;
    public GameObject door2;
    private HingeJoint hingeJoint1;
    private HingeJoint hingeJoint2;

    public Slider sphereSlider;
    public TMP_InputField sizeInputSphere;
    public Slider platformSlider;
    public TMP_InputField sizeInputPlatform;

    private void Start()
    {
        //hingeJoint1 = door1.GetComponent<HingeJoint>();
        //hingeJoint2 = door2.GetComponent<HingeJoint>();


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

        hingeJoint1 = door1.AddComponent<HingeJoint>();
        hingeJoint2 = door2.AddComponent<HingeJoint>();

        // Set the limits
        JointLimits limits = hingeJoint1.limits;
        limits.min = 0f; // Minimum angle
        limits.max = 90f; // Maximum angle
        hingeJoint1.limits = limits;
        hingeJoint2.limits = limits;

        // Set connected anchor
        Vector3 anchor1 = new Vector3(1.030685f, 1.004f, -4.9399f);
        hingeJoint1.connectedAnchor = anchor1;
        Vector3 anchor2 = new Vector3(-1.0064f, 1.004f, -5.078898f);
        hingeJoint2.connectedAnchor = anchor2;
        */
        UpdateSphereScale();
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
        float platformScale = platformSlider.value;
        // Update the scale of the platform based on the value of the platform slider
        
        exposureScene.transform.localScale = new Vector3(platformScale / 10, exposureScene.transform.localScale.y, platformScale / 10);

        sizeInputPlatform.text = platformScale.ToString();

        //Vector3 newPosition = doors.transform.position - new Vector3(0, 0, platformScale*0.2f);
        //doors.transform.position = newPosition;
        //exp_pos = wait_pos - 5 - exposureScene.transform.localScale * 5
        // door walls: pos_x +- (scale * size_x - size_x) / 2
   
        
    }

    private void UpdateSphereScale()
    {
        // Update the scale of the sphere based on the value of the sphere slider
        float sphereScale = sphereSlider.value;
        sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);

        sizeInputSphere.text = sphereScale.ToString();
    }

}

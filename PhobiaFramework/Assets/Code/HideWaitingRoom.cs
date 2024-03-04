using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class HideWaitingRoom : MonoBehaviour
{
    public GameObject waitingRoom;
    public GameObject xrRig;
    public GameObject door1;
    public GameObject door2;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(xrRig.gameObject.transform.position);
        // Check if the XR rig is past Z value 5 and the door is closed
        if (xrRig.transform.position.z > -5f && IsDoorClosed())
        {
            Debug.Log("Door is closed");
            // Hide the waiting room
            waitingRoom.SetActive(false);
        }
        else
        {
            // Show the waiting room
            waitingRoom.SetActive(true);
        }
    }

    // Method to check if the door is closed
    bool IsDoorClosed()
    {
        if (door1.transform.rotation.eulerAngles.y <= 5 && door2.transform.rotation.eulerAngles.y <= 185)
        {
            return true;
        }
        else { 
            return false; 
        }
    }
}


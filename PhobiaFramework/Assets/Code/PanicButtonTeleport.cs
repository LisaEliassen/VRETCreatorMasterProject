using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Specialized;

// This script is designed to teleport the player to a specified position when a button (stopButton) is clicked. 

public class PanicButtonTeleport : MonoBehaviour
{
    public Button stopButton;
    public GameObject xrRig;
    public GameObject xrRigCamera;
    public GameObject position;

    // Start is called before the first frame update
    void Start()
    {
        stopButton.onClick.AddListener(TeleportPlayer);
    }

    private void TeleportPlayer()
    {
        // Vector is the position which the person will be teleported to
        xrRig.transform.position = position.transform.position;
        xrRigCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}

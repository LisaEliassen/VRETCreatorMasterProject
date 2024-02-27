using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Specialized;

public class PanicButtonTeleport : MonoBehaviour
{
    public Button stopButton;
    public GameObject xrRig;
    public GameObject xrRigCamera;

    // Start is called before the first frame update
    void Start()
    {
        stopButton.onClick.AddListener(TeleportPlayer);
    }

    private void TeleportPlayer()
    {
        // Vector is the position which the person will be teleported to
        xrRig.transform.position = new Vector3(0, 0, 12.37f);
        xrRigCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}

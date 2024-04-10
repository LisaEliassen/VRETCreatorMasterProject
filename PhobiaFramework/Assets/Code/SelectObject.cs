using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SelectObject : MonoBehaviour
{
    public Camera mainCamera;

    public GameObject databaseServiceObject;
    ObjectDropdownManager objDropdownManager;

    Ray ray;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        if (databaseServiceObject != null)
        {
            objDropdownManager = databaseServiceObject.GetComponent<ObjectDropdownManager>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (objDropdownManager.GetObjects().Values.Contains(hit.collider.gameObject))
                {
                    objDropdownManager.setCurrentObject(hit.collider.name);
                    Debug.Log("Yuup");
                }
                else if (hit.collider.name == "Trigger")
                {
                    objDropdownManager.setCurrentObject(hit.collider.name);
                }
            }
        }
    }
}

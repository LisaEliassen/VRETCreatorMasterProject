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
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (objDropdownManager.GetObjects().Values.Contains(hit.collider.gameObject))
                {
                    objDropdownManager.setCurrentObject(hit.collider.name);
                }
                else if (hit.collider.name == "Trigger")
                {
                    objDropdownManager.setCurrentObject("Trigger");
                }
                else if (hit.collider.name.StartsWith("Trigger"))
                {
                    objDropdownManager.setCurrentObject("Copy");

                    hit.collider.transform.GetChild(1).gameObject.SetActive(true);

                    objDropdownManager.GetTrigger().transform.GetChild(1).gameObject.SetActive(false);

                    foreach (GameObject obj in objDropdownManager.GetObjects().Values)
                    {
                        obj.transform.GetChild(1).gameObject.SetActive(false);
                    }
                    foreach (GameObject copy in objDropdownManager.GetCopies())
                    {
                        if (copy != hit.collider.gameObject)
                        {
                            copy.transform.GetChild(1).gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    if (!hit.collider.CompareTag("Scaling"))
                    {
                        objDropdownManager.removeRedBoxes();
                    }
                }
            }
        }
    }
}

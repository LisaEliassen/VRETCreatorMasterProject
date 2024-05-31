#region License
// Copyright (C) 2024 Lisa Maria Eliassen & Olesya Pasichnyk
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the Commons Clause License version 1.0 with GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// Commons Clause License and GNU General Public License for more details.
// 
// You should have received a copy of the Commons Clause License and GNU General Public License
// along with this program. If not, see <https://commonsclause.com/> and <https://www.gnu.org/licenses/>.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

// The script allows users to interact with objects in the scene by selecting them with the mouse. 

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
        if (Input.GetMouseButtonDown(1))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name != "Trigger" && !hit.collider.name.StartsWith("Trigger") && !objDropdownManager.GetObjects().Values.Contains(hit.collider.gameObject))
                {
                    objDropdownManager.removeRedBoxes();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
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

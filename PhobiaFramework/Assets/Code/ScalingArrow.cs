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

// The script allows scaling an object in response to mouse input.

public class ScalingArrow : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    private Vector3 mOffset;
    private float mZCoord;
    private bool canScale;

    public Camera mainCamera;

    GameObject objToScale;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        Transform parent = transform.parent;

        while (parent.parent != null)
        {
            parent = parent.parent;
        }

        this.objToScale = parent.gameObject;

        this.canScale = false;
    }

    public void SetCamera(Camera camera)
    {
        this.mainCamera = camera;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mainCamera != null)
            {
                ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Scaling"))
                    {
                        print(hit.collider.name);
                        this.canScale = true;
                    }
                    else
                    {
                        this.canScale = false;
                    }
                }
            }
        }
    }

    void OnMouseDown()
    {
        if (mainCamera != null)
        {
            mZCoord = mainCamera.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 mouseWorldPos = GetMouseWorldPos();
            if (mouseWorldPos != null)
            {
                mOffset = gameObject.transform.position - GetMouseWorldPos();
            }
        }
        else
        {
            Debug.LogWarning("Main camera is null!");
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;

        if (mainCamera != null)
        {
            return mainCamera.ScreenToWorldPoint(mousePoint);
        }
        else
        {
            Debug.LogWarning("Main camera is null!");
            return mousePoint;
        }
    }

    void OnMouseDrag()
    {
        Debug.Log("dragging");
        if (this.canScale)
        {
            // Scale the parent object based on mouse movement
            float scaleSpeed = 0.1f;
            float mouseY = Input.GetAxis("Mouse Y") * scaleSpeed;

            // Apply scaling while maintaining the object's proportions
            Vector3 newScale = this.objToScale.transform.localScale + Vector3.one * mouseY;
            newScale = Vector3.Max(newScale, new Vector3(0.3f, 0.3f, 0.3f));

            if (this.objToScale != null)
            {
                this.objToScale.transform.localScale = newScale;
            }
        }
        else
        {
            Debug.Log("Cannot scale");
        }
    }
}

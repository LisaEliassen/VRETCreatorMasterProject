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

// The script controlls the dragging, scaling, and rotation behavior of an object in the Unity scene.

public class DragObject : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    public Camera mainCamera;
    public bool isRotatingObject;
    public bool canMoveObj;
    private bool canScale;
    public GameObject platform;

    CircleAndArrowGenerator circleAndArrowGenerator;
    GameObject scaleButton;

    Ray ray;
    RaycastHit hit;

    public void Start()
    {
        isRotatingObject = false;
        canMoveObj = false;
        canScale = false;

        circleAndArrowGenerator = transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<CircleAndArrowGenerator>();
        scaleButton = circleAndArrowGenerator.scaleButton;
    }

    public void SetCamera(Camera camera)
    {
        mainCamera = camera;
    }

    public void SetPlatform(GameObject platform)
    {
        this.platform = platform;
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

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                print(hit.collider.name);
                if (hit.collider.name == transform.name)
                {
                    this.isRotatingObject = true;
                }
                else
                {
                    this.isRotatingObject = false;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //print(hit.collider.name);
                if (hit.collider.name == transform.name && !hit.collider.CompareTag("Arrow"))
                {
                    this.canMoveObj = true;
                }
                else
                {
                    this.canMoveObj = false;
                }

                if (hit.collider.CompareTag("Scaling"))
                {
                    this.canScale = true;
                }
                else
                {
                    this.canScale = false;
                }
            }
        }

        if (this.isRotatingObject)
        {
            RotateObject(); // Handle object rotation
        }
    }

    Bounds CalculatePlatformBounds()
    {
        Collider platformCollider = platform.GetComponent<Collider>();
        if (platformCollider != null)
        {
            return platformCollider.bounds;
        }
        else
        {
            Debug.LogError("Platform collider not found.");
            return new Bounds();
        }
    }

    void OnMouseDrag()
    {
        if (this.canMoveObj)
        {
            // Calculate the platform bounds
            Bounds platformBounds = CalculatePlatformBounds();

            // Get the current position of the object
            Vector3 currentPos = GetMouseWorldPos() + mOffset;
            currentPos.y = transform.position.y;

            transform.position = currentPos;

            /*
            // Get the object's collider
            Collider objectCollider = GetComponent<Collider>();

            Collider[] colliders = Physics.OverlapBox(currentPos, objectCollider.bounds.extents, transform.rotation);
            bool canMove = true;

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Wall"))
                {
                    canMove = false;
                    break;
                }
            }

            // If not colliding with walls, update the position of the object
            if (canMove)
            {
                transform.position = currentPos;
            */

            if (transform.rotation.x > 0 || transform.rotation.z > 0)
            {
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
        }

        if (this.canScale)
        {
            ScaleObject();
        }
    }

    void ScaleObject()
    {
        float scaleSpeed = 0.1f;
        float mouseY = Input.GetAxis("Mouse Y") * scaleSpeed;

        Vector3 newScale = transform.localScale + Vector3.one * mouseY;
        newScale = Vector3.Max(newScale, new Vector3(0.3f, 0.3f, 0.3f));

        float scaleChange = newScale.x / transform.localScale.x;
        float lineWidthChangeFactor = Mathf.Pow(scaleChange, 0.5f);

        transform.localScale = newScale;

        float minLineWidth = 0.05f;
        float maxLineWidth = 0.3f;

        float newLineWidth = Mathf.Clamp(circleAndArrowGenerator.lineWidth * lineWidthChangeFactor, minLineWidth, maxLineWidth);

        circleAndArrowGenerator.lineWidth = newLineWidth;

        Vector3 childPos = scaleButton.transform.localPosition;
        childPos *= scaleChange;
        scaleButton.transform.localPosition = childPos;
        scaleButton.transform.localScale /= scaleChange;

    }

    void RotateObject()
    {
        if (Input.GetMouseButtonUp(1)) 
        {
            this.isRotatingObject = false;
        }
        else
        {
            // Rotate the object around its y-axis based on mouse movement
            float rotSpeed = 10f;
            float mouseX = Input.GetAxis("Mouse X") * rotSpeed;
            transform.Rotate(Vector3.up, -mouseX);
            
        }
    }
}


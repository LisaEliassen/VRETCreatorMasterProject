using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    public Camera mainCamera;
    private bool isDragging = false;
    private bool isRotatingObject = false;
    private int count = 0;

    public void SetCamera(Camera camera)
    {
        mainCamera = camera;
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

            /* This makes rotation possible but only when left clicking on collider, cursed
            if (Input.GetMouseButtonDown(0)) 
            {
                count++;
                if (count == 1)
                {
                    isRotatingObject = true; 
                }
                else if (count >= 2)
                {
                    count = 0;
                    isRotatingObject = false;
                }
            }*/
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

        /* This makes rotation possible whenever the left click is pressed, cursed as well
        if (Input.GetMouseButtonDown(0))
        {
            count++;
            if (count == 1)
            {
                isRotatingObject = true;
            }
            else if (count >= 2)
            {
                count = 0;
                isRotatingObject = false;
            }
        }*/

        if (isRotatingObject)
        {
            RotateObject(); // Handle object rotation
        }
    }

    void OnMouseDrag()
    {
        // Get the current position of the object
        Vector3 currentPos = GetMouseWorldPos() + mOffset;

        // Restrict movement along the y-axis
        currentPos.y = transform.position.y;

        // Update the position of the object
        transform.position = currentPos;
    }

    void RotateObject()
    {
        /*if (Input.GetMouseButtonUp(0)) 
        {
            isRotatingObject = false; 
        }
        else*/
        {
            // Rotate the object around its y-axis based on mouse movement
            float rotSpeed = 10f;
            float mouseX = Input.GetAxis("Mouse X") * rotSpeed;
            transform.Rotate(Vector3.up, -mouseX);
        }
    }
}


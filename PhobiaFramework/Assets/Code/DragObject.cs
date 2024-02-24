using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    public Camera mainCamera;
    public bool isRotatingObject = false;

    Ray ray;
    RaycastHit hit;

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

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                print(hit.collider.name);
                if (hit.collider.name == transform.name)
                {
                    isRotatingObject = true;
                }
            }
        }

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
        if (currentPos.y <= transform.position.y) {
            currentPos.y = transform.position.y;
        }

        // Update the position of the object
        transform.position = currentPos;
    }

    void RotateObject()
    {
        if (Input.GetMouseButtonUp(1)) 
        {
            isRotatingObject = false; 
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


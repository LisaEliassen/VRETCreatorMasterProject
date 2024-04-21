using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "arrow" && hit.collider.CompareTag("Arrow"))
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
            // Scale the object based on mouse movement
            float scaleSpeed = 0.1f;
            float mouseY = Input.GetAxis("Mouse Y") * scaleSpeed;

            // Apply scaling while maintaining the object's proportions
            Vector3 newScale = transform.localScale + Vector3.one * mouseY;
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

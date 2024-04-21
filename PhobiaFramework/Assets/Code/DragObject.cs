using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    public Camera mainCamera;
    public bool isRotatingObject;
    public bool canMoveObj;
    private bool canScale;

    CircleAndArrowGenerator circleAndArrowGenerator;


    Ray ray;
    RaycastHit hit;

    public void Start()
    {
        isRotatingObject = false;
        canMoveObj = false;
        canScale = false;

        circleAndArrowGenerator = transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<CircleAndArrowGenerator>();
    }

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

        if (this.isRotatingObject)
        {
            RotateObject(); // Handle object rotation
        }
    }

    void OnMouseDrag()
    {
        if (this.canMoveObj)
        {
            // Get the current position of the object
            Vector3 currentPos = GetMouseWorldPos() + mOffset;
            currentPos.y = transform.position.y;

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

            // If not outside boundaries, update the position of the object
            if (canMove)
            {
                transform.position = currentPos;
            }

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


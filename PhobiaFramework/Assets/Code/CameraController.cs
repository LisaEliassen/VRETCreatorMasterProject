using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
//using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;

// The script is responsible for controlling the movement, rotation, and zoom of the camera in a Unity scene.

public class CameraController : MonoBehaviour
{
    public GameObject EditUI;
    public GameObject SaveSceneUI;
    //public GameObject cam;
    public Button resetCam;

    public float moveSpeed = 1.0f;
    public float rotationSpeed = 0.5f;
    public float scrollSpeed = 5.0f;
    public float minZoomDistance = 0f;
    public float maxZoomDistance = 20.0f;

    private Vector2 movementInput;
    private Vector2 rotationInput;
    private bool isRightMouseButtonDown = false;

    private float rotationX = 0.0f;

    private float scroll;
    public Toggle invertCameraMovement;
    public Toggle invertRotationMovement;
    public Toggle invertScrollingMovement;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    Ray ray;
    RaycastHit hit;
    bool isRotatingObject = false;

    public GameObject databaseServiceObject;
    LoadGlb loadGlb;

    private void Start()
    {
        float scrollInput = Mouse.current.scroll.y.ReadValue();
        scroll = scrollInput;
        bool isRotatingObject = false;

        defaultPosition = transform.position;
        defaultRotation = transform.rotation;

        loadGlb = databaseServiceObject.GetComponent<LoadGlb>();
    }

    private void Update()
    {
        resetCam.onClick.AddListener(resetCameraToDefaultPos);

        // Read scroll wheel input for moving the camera up and down
        float scrollInput = Mouse.current.scroll.y.ReadValue();

        if (EditUI.activeSelf && !SaveSceneUI.activeSelf)
        {
            if (invertCameraMovement != null && invertCameraMovement.isOn)
            {

                // Read movement input from the new Input System
                movementInput = Keyboard.current.wKey.ReadValue() * Vector2.up +
                            Keyboard.current.aKey.ReadValue() * Vector2.left +
                            Keyboard.current.sKey.ReadValue() * Vector2.down +
                            Keyboard.current.dKey.ReadValue() * Vector2.right +
                            Keyboard.current.upArrowKey.ReadValue() * Vector2.up +
                            Keyboard.current.leftArrowKey.ReadValue() * Vector2.left +
                            Keyboard.current.downArrowKey.ReadValue() * Vector2.down +
                            Keyboard.current.rightArrowKey.ReadValue() * Vector2.right;

            }
            else
            {
                // Read movement input from the new Input System
                movementInput = Keyboard.current.wKey.ReadValue() * Vector2.down +
                                Keyboard.current.aKey.ReadValue() * Vector2.right +
                                Keyboard.current.sKey.ReadValue() * Vector2.up +
                                Keyboard.current.dKey.ReadValue() * Vector2.left +
                                Keyboard.current.upArrowKey.ReadValue() * Vector2.down +
                                Keyboard.current.leftArrowKey.ReadValue() * Vector2.right +
                                Keyboard.current.downArrowKey.ReadValue() * Vector2.up +
                                Keyboard.current.rightArrowKey.ReadValue() * Vector2.left;

            }
            // Move the camera based on the input
            Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
            transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime);


            // Read right mouse button state
            isRightMouseButtonDown = Mouse.current.rightButton.isPressed;

            // Rotate the camera based on the right mouse button
            if (isRightMouseButtonDown)
            {
                GameObject trigger = loadGlb.GetTrigger();
                List<GameObject> copies = loadGlb.GetCopies();
                List<GameObject> sceneryObjects = loadGlb.GetSceneryObjectList();

                isRotatingObject = false; // Reset flag

                // Check if trigger is rotating
                if (trigger != null && trigger.GetComponent<DragObject>().isRotatingObject)
                {
                    isRotatingObject = true;
                }

                // Check if any copies are rotating
                foreach (GameObject copy in copies)
                {
                    if (copy.GetComponent<DragObject>().isRotatingObject)
                    {
                        isRotatingObject = true;
                        break;
                    }
                }

                // Check if any scenery objects are rotating
                foreach (GameObject scenery in sceneryObjects)
                {
                    if (scenery.GetComponent<DragObject>().isRotatingObject)
                    {
                        isRotatingObject = true;
                        break;
                    }
                }

                if (!isRotatingObject)
                {
                    rotateCamera();
                }

            }
            else
            {
                rotationInput = Vector2.zero;
            }

            if (scroll != scrollInput)
            {
                // Update the camera's position based on the scroll input
                MoveCameraVertically(scrollInput);

                scroll = scrollInput;
            }
        }
    }

    private void rotateCamera()
    {
        // Read mouse position for rotation when the right mouse button is held down
        rotationInput = Mouse.current.delta.ReadValue();

        if (invertRotationMovement != null && invertRotationMovement.isOn)
        {
            // Calculate rotation based on mouse movement
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            eulerRotation.y += rotationInput.x * rotationSpeed;
            rotationX -= rotationInput.y * rotationSpeed;

            // Clamp vertical rotation to prevent flipping
            rotationX = Mathf.Clamp(rotationX, -90, 90);

            // Apply the rotation
            transform.rotation = Quaternion.Euler(rotationX, eulerRotation.y, 0);
        }
        else
        {
            // Calculate rotation based on mouse movement
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            eulerRotation.y -= rotationInput.x * rotationSpeed;
            rotationX += rotationInput.y * rotationSpeed;

            // Clamp vertical rotation to prevent flipping
            rotationX = Mathf.Clamp(rotationX, -90, 90);

            // Apply the rotation
            transform.rotation = Quaternion.Euler(rotationX, eulerRotation.y, 0);
        }

        /*
        // ANOTHER METHOD:
        // Read mouse position for rotation when the right mouse button is held down
        rotationInput = Mouse.current.delta.ReadValue();

        if (invertRotationMovement != null && invertRotationMovement.isOn)
        {
            // Calculate rotation based on mouse movement
            float rotationDeltaX = rotationInput.x * rotationSpeed;
            float rotationDeltaY = -rotationInput.y * rotationSpeed; // Invert vertical rotation

            // Apply rotation to the Camera object
            camera.transform.Rotate(Vector3.up, rotationDeltaX, Space.World);
            camera.transform.Rotate(Vector3.right, rotationDeltaY, Space.Self);
        }
        else
        {
            // Calculate rotation based on mouse movement
            float rotationDeltaX = -rotationInput.x * rotationSpeed;
            float rotationDeltaY = rotationInput.y * rotationSpeed; // Invert vertical rotation

            // Apply rotation to the Camera object
            camera.transform.Rotate(Vector3.up, rotationDeltaX, Space.World);
            camera.transform.Rotate(Vector3.right, rotationDeltaY, Space.Self);
        }*/
    }

    private void resetCameraToDefaultPos()
    {
        transform.position = defaultPosition;
        transform.rotation = defaultRotation;
    }

    private void MoveCameraVertically(float scrollInput)
    {

        if (invertScrollingMovement != null && invertScrollingMovement.isOn)
        {
            // Update the camera's vertical position based on the scroll input
            Vector3 newPosition = transform.position + Vector3.down * -scrollInput * scrollSpeed * Time.deltaTime;

            // Ensure the new position is within the specified bounds
            newPosition.y = Mathf.Clamp(newPosition.y, minZoomDistance, maxZoomDistance);

            transform.position = newPosition;
        }
        else
        {
            // Update the camera's vertical position based on the scroll input
            Vector3 newPosition = transform.position + Vector3.down * scrollInput * scrollSpeed * Time.deltaTime;

            // Ensure the new position is within the specified bounds
            newPosition.y = Mathf.Clamp(newPosition.y, minZoomDistance, maxZoomDistance);

            transform.position = newPosition;

        }
    }
}
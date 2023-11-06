using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject EditUI;

    public float moveSpeed = 1.0f;
    public float rotationSpeed = 0.5f;
    public float scrollSpeed = 5.0f;
    public float minZoomDistance = 2.0f;
    public float maxZoomDistance = 20.0f;

    private Vector2 movementInput;
    private Vector2 rotationInput;
    private bool isRightMouseButtonDown = false;
    private Vector2 mouseStartPosition;

    private float rotationX = 0.0f;

    private float scroll;

    private void Start()
    {
        float scrollInput = Mouse.current.scroll.y.ReadValue();
        scroll = scrollInput;
    }

    private void Update()
    {
        // Read scroll wheel input for moving the camera up and down
        float scrollInput = Mouse.current.scroll.y.ReadValue();

        if (EditUI.activeSelf)
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

            // Move the camera based on the input
            Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
            transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime);


            // Read right mouse button state
            isRightMouseButtonDown = Mouse.current.rightButton.isPressed;

            // Rotate the camera based on the right mouse button
            if (isRightMouseButtonDown)
            {
                // Read mouse position for rotation when the right mouse button is held down
                rotationInput = Mouse.current.delta.ReadValue();

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

    private void MoveCameraVertically(float scrollInput)
    {
        // Update the camera's vertical position based on the scroll input
        Vector3 newPosition = transform.position + Vector3.up * scrollInput * scrollSpeed * Time.deltaTime;

        // Ensure the new position is within the specified bounds
        newPosition.y = Mathf.Clamp(newPosition.y, minZoomDistance, maxZoomDistance);

        transform.position = newPosition;
    }
}
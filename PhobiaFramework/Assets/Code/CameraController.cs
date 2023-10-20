using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 2.0f;

    private Vector2 movementInput;
    private Vector2 rotationInput;
    private bool isRightMouseButtonDown = false;
    private Vector2 mouseStartPosition;

    private float rotationX = 0.0f; // Store the camera's X rotation separately.

    // Define the minimum and maximum vertical rotation angles
    private float minVerticalRotation = -90.0f;
    private float maxVerticalRotation = 90.0f;

    private void Update()
    {
        // Read movement input from the new Input System
        movementInput = Keyboard.current.wKey.ReadValue() * Vector2.down +
                        Keyboard.current.aKey.ReadValue() * Vector2.right +
                        Keyboard.current.sKey.ReadValue() * Vector2.up +
                        Keyboard.current.dKey.ReadValue() * Vector2.left;

        // Read right mouse button state
        isRightMouseButtonDown = Mouse.current.rightButton.isPressed;

        // Read mouse position for rotation when the right mouse button is held down
        if (isRightMouseButtonDown)
        {
            rotationInput = Mouse.current.delta.ReadValue();
        }
        else
        {
            rotationInput = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        // Move the camera based on the input
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
        transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime);

        // Rotate the camera based on the right mouse button
        if (isRightMouseButtonDown)
        {
            // Calculate rotation based on mouse movement
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            eulerRotation.y += rotationInput.x * rotationSpeed;
            rotationX -= rotationInput.y * rotationSpeed;

            // Clamp vertical rotation to prevent flipping
            rotationX = Mathf.Clamp(rotationX, minVerticalRotation, maxVerticalRotation);

            // Apply the rotation
            transform.rotation = Quaternion.Euler(rotationX, eulerRotation.y, 0);
        }
    }
}





















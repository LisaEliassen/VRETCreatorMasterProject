using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotator : MonoBehaviour
{
    public float rotationSpeed = 60.0f;
    public float heightSpeed = 10.0f;

    private void Update()
    {
        // Rotate the camera left with the "A" key
        if (Keyboard.current.aKey.isPressed)
        {
            MoveCamera(Vector3.right * heightSpeed * Time.deltaTime);
        }

        // Rotate the camera right with the "D" key
        if (Keyboard.current.dKey.isPressed)
        {
            MoveCamera(Vector3.left * heightSpeed * Time.deltaTime);
        }

        // Move the camera up with the "W" key
        if (Keyboard.current.wKey.isPressed)
        {
            MoveCamera(Vector3.up * heightSpeed * Time.deltaTime);
        }

        // Move the camera down with the "S" key
        if (Keyboard.current.sKey.isPressed)
        {
            MoveCamera(Vector3.down * heightSpeed * Time.deltaTime);
        }
    }

    private void RotateCamera(Vector3 axis, float angle)
    {
        transform.Rotate(axis, angle);
    }

    private void MoveCamera(Vector3 translation)
    {
        transform.Translate(translation);
    }
}











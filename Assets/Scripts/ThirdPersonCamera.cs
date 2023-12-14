using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;  // Reference to the player's Transform component
    public float rotationSpeed = 5f;  // Speed of camera rotation around the player
    public Vector3 offset = new Vector3(0f, 2f, -5f);  // Offset from the player
    public float angularRotation = 30f;  // Angular rotation in degrees

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired rotation based on player's input
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 playerDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
            Vector3 desiredForward = Vector3.Slerp(transform.forward, playerDirection, rotationSpeed * Time.deltaTime);

            // Calculate the desired position based on the offset
            Vector3 desiredPosition = target.position + offset;

            // Apply angular rotation to the camera
            transform.Rotate(Vector3.up, angularRotation * Time.deltaTime);

            // Update the camera's rotation and position
            transform.forward = desiredForward;
            transform.position = desiredPosition;

            // Ensure the camera is looking at the player
            transform.LookAt(target);
        }
    }
}
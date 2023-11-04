using UnityEngine;

public class CameraPositioning : MonoBehaviour
{
    public Transform target;  // The target point to look at

    public float initialHeight = 15f; // Initial height of the camera above the target
    public float initialDistance = 10f; // Initial distance from the target
    public float rotationSpeed = 3f; // Speed of camera rotation
    public float zoomSpeed = 5f; // Speed of zooming

    private float mouseX = 0f;
    private float mouseY = 45f; // Set initial angle to 45 degrees
    private float distance = 10f; // Initial distance from the target
    private bool isCameraLocked = false;

    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("Target transform is not assigned. Please assign a target in the Inspector.");
        }
        else
        {
            UpdateCameraPosition();
        }
    }

    void Update()
    {
        if (target == null)
            return;

        // Toggle camera lock when the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            isCameraLocked = !isCameraLocked;
        }

        if (!isCameraLocked)
        {
            // Camera rotation with mouse input
            mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            mouseY = Mathf.Clamp(mouseY, 0f, 90f); // Limit vertical rotation to avoid flipping

            // Zoom in and out with the mouse wheel
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            distance -= scroll * zoomSpeed;
            distance = Mathf.Max(distance, 1f); // Ensure minimum zoom distance
        }

        // Set the camera's position to the target's position
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        // Calculate the camera's position based on the target, height, distance, and rotation
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);
        Vector3 targetPosition = target.position + offset;

        // Set the camera's position and rotation
        transform.position = targetPosition;
        transform.LookAt(target);
    }
}

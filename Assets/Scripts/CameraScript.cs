using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target; // Reference to the marble's transform
    public float followSpeed = 10f; // Speed at which the camera follows the marble

    void FixedUpdate()
    {
        if (target != null) // Check if target is assigned
        {
            Vector2 targetPosition = new Vector2(target.position.x, target.position.y); // Get target position
            transform.position = Vector2.Lerp(transform.position, targetPosition, followSpeed * Time.fixedDeltaTime); // Smoothly move the camera towards the target position
        }
    }
}

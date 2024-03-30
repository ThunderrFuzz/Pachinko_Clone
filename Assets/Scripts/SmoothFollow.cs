using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;

    private void FixedUpdate()
    {
        
        
        // Set the camera position to match the target's position
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Set the camera to look at the target
        transform.LookAt(target);
        
    }
}

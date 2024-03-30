using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollower : MonoBehaviour
{
    public Transform marbleTransform; // Reference to the marble's transform
    public Vector3 offset; // Offset distance from the marble
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure the marbleTransform reference is not null
        if (marbleTransform != null)
        {
            // Set the position of this object to the marble's position plus the offset
            transform.position = marbleTransform.position + offset;

            // Rotate this object to face the marble with a fixed viewing angle
            transform.LookAt(marbleTransform);
        }
    }
}

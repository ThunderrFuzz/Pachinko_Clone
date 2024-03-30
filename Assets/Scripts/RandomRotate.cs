using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomRotation : MonoBehaviour
{
    void Start()
    {
        // Generate a random rotation angle between 5 and 25 degrees
        float randomRotation = Random.Range(5f, 45f);

        // Apply the random rotation around the Y-axis
        transform.Rotate(0f, 0f, randomRotation);
    }
}


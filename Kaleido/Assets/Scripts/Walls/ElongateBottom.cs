using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElongateBottom : MonoBehaviour
{
    Vector3 originalScale;
    Vector3 originalPosition;

    private float prevElongationFactor;
    public float currElongationFactor = 1.0f;

    void Start()
    {
        // Get the original scale of the cube
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    void Update()
    {
        if (currElongationFactor != prevElongationFactor)
        {
            Elongate();

            prevElongationFactor = currElongationFactor;
        }
    }

    void Elongate()
    {
        // Calculate the new Y scale based on the elongation factor
        float newYScale = originalScale.y * currElongationFactor;

        // Calculate the offset to keep the top of the cube stationary
        float yOffset = (newYScale - originalScale.y) / 2.0f;

        // Apply the new scale
        transform.localScale = new Vector3(originalScale.x, newYScale, originalScale.z);

        // Adjust the position to keep the top of the cube in place
        transform.position = new Vector3(originalPosition.x, originalPosition.y - yOffset, originalPosition.z); //Subtract the yOffset to invert the elongation direction
    }
}

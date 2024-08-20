using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public Vector3 originalScale;
    public Vector3 originalPosition;

    public float currElongationFactor = 1f;

    public float moveSpeed = 1f;
    public bool isBeingEaten = false;

    void Start()
    {
        // Get the original scale of the cube
        originalScale = transform.localScale;
        originalPosition = transform.position;

        Elongate(currElongationFactor);
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Get's the relative bottom of the rect for this wall
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRectBottomCenterPosition()
    {
        return new Vector3(0, -transform.localScale.y, 0);
    }

    public void Elongate(float elongationFactor)
    {
        // Update the elongation factor
        currElongationFactor = elongationFactor;

        // Calculate the new Y scale based on the elongation factor
        float newYScale = originalScale.y * currElongationFactor;

        // Apply the new scale to the rect
        transform.localScale = new Vector3(transform.localScale.x, newYScale, transform.localScale.z);
    }
}

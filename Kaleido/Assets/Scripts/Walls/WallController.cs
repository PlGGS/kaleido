using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public Vector3 originalScale;
    public Vector3 originalPosition;

    public float currElongationFactor = 1f;
    private float prevElongationFactor;

    public float moveSpeed = 1f;
    public bool isBeingEaten = false;

    void Start()
    {
        // Get the original scale of the cube
        originalScale = transform.localScale;
        originalPosition = transform.position;

        //Set these to the same value initial since we update this in the SpawnWalls.MoveAndRotateWall coroutine
        //prevElongationFactor = currElongationFactor;
    }

    void Update()
    {
        if (currElongationFactor != prevElongationFactor)
        {
            Elongate(currElongationFactor);

            prevElongationFactor = currElongationFactor;
        }
    }

    public Vector3 GetWallTopCenterPosition()
    {
        return transform.position + new Vector3(0, transform.localScale.y / 2, 0);
    }

    public Vector3 GetWallBottomCenterPosition()
    {
        return transform.position - new Vector3(0, transform.localScale.y / 2, 0);
    }

    public void Elongate(float elongationFactor)
    {
        // Calculate the new Y scale based on the elongation factor
        float newYScale = originalScale.y * elongationFactor;

        // Calculate the offset to keep the top of the cube stationary
        float yOffset = newYScale / 2.0f;

        // Apply the new scale
        transform.localScale = new Vector3(originalScale.x, newYScale, originalScale.z);

        // Adjust the position to keep the top of the cube in place
        transform.position = new Vector3(originalPosition.x, originalPosition.y - yOffset, originalPosition.z); //Subtract the yOffset to invert the elongation direction
    }
}

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

    /// <summary>
    /// Get's the relative top of the rect for this wall
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRectTopCenterPosition()
    {
        return new Vector3(0, transform.localScale.y / 2, 0);
    }

    /// <summary>
    /// Get's the relative bottom of the rect for this wall
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRectBottomCenterPosition()
    {
        return new Vector3(0, -transform.localScale.y / 2, 0);
    }

    public void Elongate(float elongationFactor)
    {
        // Calculate the new Y scale based on the elongation factor
        float newYScale = originalScale.y * elongationFactor;

        // Calculate the offset to keep the top of the cube stationary
        float yOffset = newYScale / 2.0f;







        //TODO THIS IS NECESSARY IF WE WANT TO KEEP THE HEIGHT THE SAME, BUT IT BREAKS THE POSITIONING IN CENTERPOINTCONTROLLER
        //TODO NEED TO LOOK UP / FIGURE OUT IF WE CAN JUST LOCK THE HEIGHT IN UNITY IDK

        // Apply the new scale
        //transform.localScale = new Vector3(originalScale.x, newYScale, originalScale.z);

        // Adjust the position to keep the top of the cube in place
        //transform.parent.transform.position = new Vector3(originalPosition.x, originalPosition.y - yOffset, originalPosition.z); //Subtract the yOffset to invert the elongation direction
    }
}

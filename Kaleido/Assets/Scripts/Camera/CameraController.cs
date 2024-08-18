using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;           // The player object to follow
    public GameObject centerPoint;      // The center point that the camera will face
    public float forwardOffset = 2f;    // Distance behind the player
    public float heightOffset = 15f;     // Height offset for the camera
    public float followSpeed = 2f;      // Speed at which the camera follows the player
    public float rotationSpeed = 5f;    // Speed at which the camera rotates to face the CenterPoint

    private Vector3 currentVelocity;    // Used for smooth damping

    void LateUpdate()
    {
        if (player != null && centerPoint != null)
        {
            //Position
            //------------------------------------------------

            Vector3 distanceFromCenter = player.transform.position - centerPoint.transform.position;

            // Desired position is behind the player, at a specific distance and height
            Vector3 desiredPosition;
            if (distanceFromCenter.x > 0)
            {
                // Player is on the right side, so camera should be in front
                desiredPosition = player.transform.position + (player.transform.right * forwardOffset) + (Vector3.up * heightOffset);
            }
            else
            {
                // Player is on the left side, so camera should be behind
                desiredPosition = player.transform.position + (player.transform.right * -forwardOffset) + (Vector3.up * heightOffset);
            }

            // Smoothly move the camera to the desired position
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, followSpeed);


            //Rotation
            //------------------------------------------------

            // Look straight down (towards -Y direction)
            Vector3 downwardDirection = Vector3.down;

            // Compute the direction forward horizontally
            Vector3 forwardDirection = (transform.position - player.transform.position).normalized;
            forwardDirection.y = 0; // Zero out the y component to get the horizontal direction

            // Compute the direction towards the centerpoint horizontally
            Vector3 centerDirection = (centerPoint.transform.position - player.transform.position).normalized;
            centerDirection.y = 0; // Zero out the y component to get the horizontal direction

            // Create a rotation that looks straight down with the top pointing towards the centerpoint
            Quaternion targetRotation = Quaternion.LookRotation(forwardDirection) * Quaternion.Euler(90, 0, 0);

            // Smoothly rotate the camera to the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

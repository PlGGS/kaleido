using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWalls : MonoBehaviour
{
    public GameObject wallPrefab;
    public int numberOfWalls = 10;
    public float radius = 10;
    public GameObject[] walls;

    private float wallWidthPaddingPercentage = 0.057f;

    void Start()
    {
        walls = new GameObject[numberOfWalls];
        SpawnWallsInCircle();
    }

    void SpawnWallsInCircle()
    {
        // Calculate the angle between each wall
        float angleStep = 360.0f / numberOfWalls;
        float angleStepRadians = angleStep * Mathf.Deg2Rad;

        for (int i = 0; i < numberOfWalls; i++)
        {
            // Determine the position of the wall
            Vector3 wallPosition = new Vector3(
                transform.position.x + Mathf.Cos(i * angleStepRadians) * radius,
                transform.position.y,
                transform.position.z + Mathf.Sin(i * angleStepRadians) * radius
            );

            // Instantiate the wall at the calculated position
            GameObject wall = Instantiate(wallPrefab, wallPosition, Quaternion.identity);
            walls[i] = wall;

            // Adjust the wall's position so that the edge is on the circle
            float wallWidth = 2 * radius * Mathf.Sin(angleStepRadians / 2);
            float padding = wallWidth * wallWidthPaddingPercentage; //Add the padding to ensure no gaps
            wall.transform.localScale = new Vector3(wallWidth + padding, wall.transform.localScale.y, wall.transform.localScale.z);

            // Rotate the wall to face the centerPoint
            wall.transform.LookAt(transform.position);
        }
    }
}

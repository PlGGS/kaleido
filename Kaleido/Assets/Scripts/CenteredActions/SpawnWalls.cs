using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWalls : MonoBehaviour
{
    public GameObject wallPrefab;
    public int numberOfWalls;
    public float radius;

    //public List<Vector3> walls = new List<GameObject>();
    public List<Vector3> vertices;

    private float wallWidthPadding = 0.579f;

    void Start()
    {
        vertices = DefineInfinityPattern(numberOfWalls, radius);
        
        SpawnWallsAtPositions();
    }

    void SpawnWallsAtPositions()
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            Vector3 nextVertex = vertices[(i + 1) % vertices.Count]; // Wrap around to the first point// Calculate the position and rotation for the wall
            Vector3 wallPosition = (vertex + nextVertex) / 2f;
            Quaternion wallRotation = Quaternion.LookRotation(nextVertex - vertex);

            // Calculate the length of the wall
            float wallLength = Vector3.Distance(vertex, nextVertex);
            //float padding = wallLength * wallWidthPaddingPercentage;

            // Instantiate the wall prefab
            GameObject wall = Instantiate(wallPrefab, wallPosition, wallRotation);
            wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, wallLength);
        }
    }

    List<Vector3> DefineCircularPattern(int amtVertices, float radius)
    {
        List<Vector3> vertices = new List<Vector3>();

        float angleStep = 360.0f / amtVertices;
        float angleStepRadians = angleStep * Mathf.Deg2Rad;

        for (int i = 0; i < amtVertices; i++)
        {
            Vector3 position = new Vector3(
                transform.position.x + Mathf.Cos(i * angleStepRadians) * radius,
                0,
                transform.position.z + Mathf.Sin(i * angleStepRadians) * radius
            );
            vertices.Add(position);
        }

        return vertices;
    }

    List<Vector3> DefineInfinityPattern(int amtVertices, float radius)
    {
        List<Vector3> vertices = new List<Vector3>();

        float angleStep = 2 * Mathf.PI / amtVertices;

        for (int i = 0; i < amtVertices; i++)
        {
            float t = i * angleStep;
            float x = (Mathf.Sin(t) * radius) / (1 + Mathf.Pow(Mathf.Cos(t), 2));
            float z = (Mathf.Sin(t) * Mathf.Cos(t) * radius * 1.5f) / (1 + Mathf.Pow(Mathf.Cos(t), 2));

            Vector3 position = new Vector3(x, 0, z);
            vertices.Add(position);
        }

        return vertices;
    }
}

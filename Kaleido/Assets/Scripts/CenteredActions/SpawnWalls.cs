using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWalls : MonoBehaviour
{
    public GameObject wallPrefab;

    [HideInInspector]
    public List<GameObject> walls;

    public int initialAmtWalls;
    public int currAmtWalls
    {
        get
        {
            if (walls != null && walls.Count > 2)
                return walls.Count;

            return initialAmtWalls;
        }
        set
        {
            currAmtWalls = value;
        }
    }
    [HideInInspector]
    public int prevAmtWalls;

    public float currRadius;
    [HideInInspector]
    public float prevRadius;

    public VertexPattern.Patterns currPatternDefinition;
    [HideInInspector]
    public VertexPattern.Patterns prevPatternDefinition;
    

    //public List<Vector3> walls = new List<GameObject>();
    public List<Vector3> vertices;

    private float wallWidthPadding = 0.579f;

    void Start()
    {
        vertices = SpawnWallsAtPositions(MapToCurrentVertexPattern());



    }

    void Update()
    {
        if (currRadius != prevRadius || currAmtWalls != prevAmtWalls || currPatternDefinition != prevPatternDefinition)
        {
            vertices = TranslateWallsToNewPositions(MapToCurrentVertexPattern(), 1000f, 1000f);
            
            prevRadius = currRadius;
            prevAmtWalls = currAmtWalls;
            prevPatternDefinition = currPatternDefinition;
        }

        if (walls.Count != currAmtWalls)
        {
            currAmtWalls = walls.Count;
        }
    }

    List<Vector3> MapToCurrentVertexPattern()
    {
        List<Vector3> vertices;

        switch (currPatternDefinition)
        {
            case VertexPattern.Patterns.Circle:
                currPatternDefinition = VertexPattern.Patterns.Circle;
                vertices = VertexPattern.Circle(currAmtWalls, currRadius);
                break;
            case VertexPattern.Patterns.Infinity:
                currPatternDefinition = VertexPattern.Patterns.Infinity;
                vertices = VertexPattern.Infinity(currAmtWalls, currRadius);
                break;
            default:
                throw new Exception("Invalid Vertex Pattern Definition");
        }

        return vertices;
    }

    List<Vector3> SpawnWallsAtPositions(List<Vector3> vertexPattern)
    {
        for (int i = 0; i < vertexPattern.Count; i++)
        {
            Vector3 currVertex = vertexPattern[i];
            Vector3 nextVertex = vertexPattern[(i + 1) % vertexPattern.Count]; // Wrap around to the first point
            Vector3 wallPosition = (currVertex + nextVertex) / 2f;
            Quaternion wallRotation = Quaternion.LookRotation(nextVertex - currVertex);

            // Calculate the length of the wall
            float wallLength = Vector3.Distance(currVertex, nextVertex);
            //float padding = wallLength * wallWidthPadding;

            // Instantiate the wall prefab
            GameObject wall = Instantiate(wallPrefab, wallPosition, wallRotation);
            wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, wallLength);

            // Add wall to list of walls
            walls.Add(wall);
        }

        return vertexPattern;
    }

    /// <summary>
    /// Moves walls to new positions based on the currently defined pattern
    /// </summary>
    /// <returns>New vertex pattern</returns>
    List<Vector3> TranslateWallsToNewPositions(List<Vector3> vertexPattern, float moveSpeed, float rotationSpeed)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 currVertex = vertexPattern[i];
            Vector3 nextVertex = vertexPattern[(i + 1) % vertexPattern.Count]; // Wrap around to the first point
            GameObject currWall = walls[i];

            if (currWall != null)
            {
                Vector3 currWallPosition = currWall.transform.position;
                Vector3 nextWallPosition = (new Vector3(currVertex.x, currWallPosition.y, currVertex.z) + new Vector3(nextVertex.x, currWallPosition.y, nextVertex.z)) / 2f;

                currWall.transform.position = Vector3.Lerp(currWallPosition, nextWallPosition, Time.deltaTime * moveSpeed);

                Quaternion wallRotation = Quaternion.LookRotation(nextVertex - currVertex);
                currWall.transform.rotation = Quaternion.Slerp(currWall.transform.rotation, wallRotation, Time.deltaTime * rotationSpeed);

                float newWallLength = Vector3.Distance(currVertex, vertexPattern[(i + 1) % vertexPattern.Count]);
                currWall.transform.localScale = new Vector3(currWall.transform.localScale.x, currWall.transform.localScale.y, newWallLength);
            }
        }

        return vertexPattern;
    }
}

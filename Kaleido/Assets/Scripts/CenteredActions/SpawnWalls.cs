using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWalls : MonoBehaviour
{
    public GameObject wallPrefab;

    [HideInInspector]
    public List<GameObject> walls;

    public int currAmtWalls;
    [HideInInspector]
    public int prevAmtWalls;

    public float currRadius;
    [HideInInspector]
    public float prevRadius;

    public VertexPattern.Patterns currPatternDefinition;
    [HideInInspector]
    public VertexPattern.Patterns prevPatternDefinition;

    public List<Vector3> vertices;
    
    void Start()
    {
        prevRadius = currRadius;
        prevAmtWalls = currAmtWalls;
        prevPatternDefinition = currPatternDefinition;

        vertices = SpawnWallsAtPositions(MapToCurrentVertexPattern());
    }

    void Update()
    {
        if (currRadius != prevRadius || currAmtWalls != prevAmtWalls || currPatternDefinition != prevPatternDefinition)
        {
            vertices = TranslateWallsToNewPositions(MapToCurrentVertexPattern(), 1000f); //TODO figure out what to make this value
            
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
        List<Vector3> newVertices;

        switch (currPatternDefinition)
        {
            case VertexPattern.Patterns.Circle:
                currPatternDefinition = VertexPattern.Patterns.Circle;
                newVertices = VertexPattern.Circle(currAmtWalls, currRadius);
                break;
            case VertexPattern.Patterns.Infinity:
                currPatternDefinition = VertexPattern.Patterns.Infinity;
                newVertices = VertexPattern.Infinity(currAmtWalls, currRadius);
                break;
            default:
                throw new Exception("Invalid Vertex Pattern Definition");
        }

        return newVertices;
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
    List<Vector3> TranslateWallsToNewPositions(List<Vector3> vertexPattern, float moveSpeed)
    {
        int i = 0;
        for (; i < vertexPattern.Count; i++)
        {
            Vector3 currVertex = vertexPattern[i];
            Vector3 nextVertex = vertexPattern[(i + 1) % vertexPattern.Count]; // Wrap around to the first point
            
            //In case we're dynamically adding wall(s)
            //This will always equal the Count since we add one to the list in this case
            if (i == walls.Count)
            {
                Vector3 wallPosition = (currVertex + nextVertex) / 2f;
                Quaternion wallRotation = Quaternion.LookRotation(nextVertex - currVertex);

                Debug.Log("Ay I'm instantiating a new wall here");

                walls.Add(Instantiate(wallPrefab, wallPosition, wallRotation));
            }

            GameObject currWall = walls[i];

            if (currWall != null)
            {
                StartCoroutine(MoveAndRotateWall(currWall, currVertex, nextVertex, moveSpeed));
            }
        }

        //In case we're dynamically removing walls, delete the extra ones
        if (walls.Count > vertexPattern.Count)
        {
            //Debug.Log($"Walls: {walls.Count}, Verts: {vertices.Count}");
            
            for (int o = walls.Count - 1; o >= i; o--)
            {
                //Debug.Log("Ay I'm destroying walls here");

                Destroy(walls[i]);
                walls.RemoveAt(i);
            }
        }

        return vertexPattern;
    }

    private IEnumerator MoveAndRotateWall(GameObject currWall, Vector3 currVertex, Vector3 nextVertex, float moveSpeed)
    {
        Vector3 currWallPosition = currWall.transform.position;
        Vector3 nextWallPosition = (new Vector3(currVertex.x, currWallPosition.y, currVertex.z) + new Vector3(nextVertex.x, currWallPosition.y, nextVertex.z)) / 2f;

        Quaternion startRotation = currWall.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(nextVertex - currVertex);

        Vector3 startScale = currWall.transform.localScale;
        float newWallLength = Vector3.Distance(currVertex, nextVertex);
        Vector3 targetScale = new Vector3(startScale.x, startScale.y, newWallLength);

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;

            // Lerp position over time
            currWall.transform.position = Vector3.Lerp(currWallPosition, nextWallPosition, elapsedTime);

            // Slerp rotation over time
            currWall.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);

            // Lerp scale over time
            currWall.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime);

            yield return null;
        }

        // Final adjustments to ensure accuracy
        currWall.transform.position = nextWallPosition;
        currWall.transform.rotation = targetRotation;
        currWall.transform.localScale = targetScale;
    }
}

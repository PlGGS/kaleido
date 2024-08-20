using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPointController : MonoBehaviour
{
    public GameObject wallPrefab;

    [HideInInspector]
    public List<GameObject> walls;

    public int currAmtWalls;
    [HideInInspector]
    public int prevAmtWalls;

    [Range(10f, 1000f)]
    public float currRadius;
    [HideInInspector]
    [Range(10f, 1000f)]
    public float prevRadius;

    public VertexPattern.Patterns currPatternDefinition;
    [HideInInspector]
    public VertexPattern.Patterns prevPatternDefinition;

    public List<Vector3> vertices;

    private List<Coroutine> runningCoroutines = new List<Coroutine>();

    public const float defaultWallMoveSpeed = 30f;
    public const float defaultWallMoveDuration = 0.5f;

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
            StopAllWallCoroutines();

            vertices = TranslateWallsToNewPositions(MapToCurrentVertexPattern());

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

    /// <summary>
    /// Returns a wall that's not being eaten along with its index
    /// </summary>
    /// <returns>Returns (null, -1) if there are no walls available to be eaten</returns>
    public GameObject GetRandomWall()
    {
        //TODO maybe get walls that are being eaten and just move the enemies currently eating them up a bit to create a line of defence for the enemies on that wall

        if (walls.Count > 0)
        {
            // List to keep track of indices that are being checked or already checked
            List<int> checkedIndices = new List<int>();

            int index;
            do
            {
                index = GetRandomWallIndex();

                if (!walls[index].GetComponentInChildren<WallController>().isBeingEaten && !checkedIndices.Contains(index))
                {
                    return walls[index];
                }

                checkedIndices.Add(index);

            } while (checkedIndices.Count < walls.Count);
        }

        return null;
    }

    public int GetRandomWallIndex() {
        return UnityEngine.Random.Range(0, walls.Count - 1);
    }

    public GameObject GetWall(int index)
    {
        return walls[index];
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

            SpawnWallAtPosition(currVertex, nextVertex);
        }

        return vertexPattern;
    }

    Vector3 SpawnWallAtPosition(Vector3 currVertex, Vector3 nextVertex)
    {
         // Wrap around to the first point
        Vector3 wallPosition = (currVertex + nextVertex) / 2f;
        Quaternion wallRotation = Quaternion.LookRotation(nextVertex - currVertex);

        // Calculate the length of the wall
        float wallLength = Vector3.Distance(currVertex, nextVertex);

        // Instantiate the wall prefab
        GameObject wall = Instantiate(wallPrefab, wallPosition, wallRotation);
        Transform pivot = wall.transform.GetChild(0);
        pivot.transform.localScale = new Vector3(pivot.transform.localScale.x, pivot.transform.localScale.y, wallLength);

        // Add wall to list of walls
        walls.Add(wall);

        return currVertex;
    }

    /// <summary>
    /// Moves walls to new positions based on the currently defined pattern
    /// </summary>
    /// <returns>New vertex pattern</returns>
    List<Vector3> TranslateWallsToNewPositions/*TODO AtSpeed / OverDuration*/(List<Vector3> vertexPattern, float moveSpeed = defaultWallMoveSpeed, float moveDuration = defaultWallMoveDuration)
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
                Debug.Log("Ay I'm instantiating a new wall here");

                SpawnWallAtPosition(currVertex, nextVertex);
            }

            GameObject currWall = walls[i];

            if (currWall != null)
            {
                Coroutine wallCoroutine = StartCoroutine(TranslateWallOverDuration(currWall, currVertex, nextVertex, moveDuration));
                runningCoroutines.Add(wallCoroutine);
            }
        }

        //In case we're dynamically removing walls, delete the extra ones
        if (walls.Count > vertexPattern.Count)
        {
            //Debug.Log($"Walls: {walls.Count}, Verts: {vertices.Count}");
            
            for (int o = walls.Count - 1; o >= i; o--)
            {
                //Debug.Log("Ay I'm destroying walls here");

                RemoveWallAt(i);
            }
        }

        return vertexPattern;
    }

    public void RemoveWallAt(int index)
    {
        Destroy(walls[index]);
        walls.RemoveAt(index);
    }
    public void RemoveWall(GameObject wall)
    {
        walls.Remove(wall);
        Destroy(wall);
    }

    private IEnumerator TranslateWallAtSpeed(GameObject currWall, Vector3 currVertex, Vector3 nextVertex, float moveSpeed = defaultWallMoveSpeed)
    {
        // Wait until the next frame to ensure Start has been called
        yield return null;

        // Get the pivot of the wall
        Transform pivot = currWall.transform.GetChild(0);

        // Get the WallController script from the wall
        WallController wallController = pivot.GetComponent<WallController>();

        //Exit the coroutine if the attached script is missing (meaning the wall was destroyed)
        if (wallController == null) yield break;

        var (startPosition, targetPosition, startRotation, targetRotation, startScale, targetScale) = GetWallTransformationData(currWall, pivot, wallController, currVertex, nextVertex);

        //Use the wall's moveSpeed if we don't define one ourselves
        if (wallController.moveSpeed == defaultWallMoveSpeed)
            moveSpeed = wallController.moveSpeed;

        float distance = Vector3.Distance(startPosition, targetPosition);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            //Check again if the attached script is missing (meaning the wall was destroyed)
            if (wallController == null) yield break;

            // Lerp position over time
            currWall.transform.position = Vector3.Lerp(startPosition, targetPosition, 1 - (remainingDistance / distance));

            // Slerp rotation over time
            currWall.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, 1 - (remainingDistance / distance));

            // Lerp scale over time
            pivot.transform.localScale = Vector3.Lerp(startScale, targetScale, 1 - (remainingDistance / distance));

            remainingDistance -= Time.deltaTime * moveSpeed;

            yield return null;
        }

        //Check again if the attached script is missing (meaning the wall was destroyed)
        if (wallController == null) yield break;

        // Final adjustments to ensure accuracy
        currWall.transform.position = targetPosition;
        currWall.transform.rotation = targetRotation;
        pivot.transform.localScale = targetScale;
    }

    private IEnumerator TranslateWallOverDuration(GameObject currWall, Vector3 currVertex, Vector3 nextVertex, float moveDuration = defaultWallMoveDuration)
    {
        // Wait until the next frame to ensure Start has been called
        yield return null;

        // Get the pivot of the wall
        Transform pivot = currWall.transform.GetChild(0);

        // Get the WallController script from the wall
        WallController wallController = pivot.GetComponent<WallController>();

        //Exit the coroutine if the attached script is missing (meaning the wall was destroyed)
        if (wallController == null) yield break;

        var (startPosition, targetPosition, startRotation, targetRotation, startScale, targetScale) = GetWallTransformationData(currWall, pivot, wallController, currVertex, nextVertex);

        //Use the wall's moveSpeed if we don't define one ourselves
        //if (wallController.moveSpeed == defaultWallMoveSpeed)
        //    moveSpeed = wallController.moveSpeed;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            // Check again if the attached script is missing (meaning the wall was destroyed)
            if (wallController == null) yield break;

            // Lerp position over time
            currWall.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);

            // Slerp rotation over time
            currWall.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);

            // Lerp scale over time
            pivot.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime);

            elapsedTime += Time.deltaTime * (1 / moveDuration);
            yield return null;
        }

        //Check again if the attached script is missing (meaning the wall was destroyed)
        if (wallController == null) yield break;

        // Final adjustments to ensure accuracy
        currWall.transform.position = targetPosition;
        currWall.transform.rotation = targetRotation;
        pivot.transform.localScale = targetScale;
    }

    private (Vector3 startPosition, Vector3 targetPosition, Quaternion startRotation, Quaternion targetRotation, Vector3 startScale, Vector3 targetScale) GetWallTransformationData(GameObject wall, Transform pivot, WallController wallController, Vector3 currVertex, Vector3 nextVertex)
    {
        Vector3 startPosition = wall.transform.position;

        //gets the position directly between the two edges of the wall
        Vector3 targetPosition = (new Vector3(currVertex.x, wallController.originalPosition.y, currVertex.z) + new Vector3(nextVertex.x, wallController.originalPosition.y, nextVertex.z)) / 2f;

        Quaternion startRotation = wall.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(nextVertex - currVertex);

        Vector3 startScale = pivot.transform.localScale;
        float newWallLength = Vector3.Distance(currVertex, nextVertex);
        Vector3 targetScale = new Vector3(startScale.x, startScale.y, newWallLength);

        return (startPosition, targetPosition, startRotation, targetRotation, startScale, targetScale);
    }

    void StopAllWallCoroutines()
    {
        foreach (var coroutine in runningCoroutines)
        {
            StopCoroutine(coroutine);
        }

        runningCoroutines.Clear();
    }
}

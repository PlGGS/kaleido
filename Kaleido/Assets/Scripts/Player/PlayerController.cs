using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject centerPoint;
    private CenterPointController centerPointController;
    private int currentWallIndex;

    public float moveInterval = 0.05f; // Time in seconds to move to the next wall
    private float timeSinceLastMove = 1f;

    private bool wasCrawling = false; //Was the player crawling on the last update (Allows for delay to feel like you're slowing down)
    private bool isCrawling = false;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    // Start is called before the first frame update
    void Start()
    {
        if (centerPoint != null)
        {
            centerPointController = centerPoint.GetComponent<CenterPointController>();
            if (centerPointController != null)
            {
                StartCoroutine(InitializeAndMoveToFirstWall());
            }
            else
            {
                Debug.LogError("CenterPointController component not found");
            }
        }
        else
        {
            Debug.LogError("CenterPoint is not assigned");
        }
    }

    // Coroutine to wait until walls are initialized
    IEnumerator InitializeAndMoveToFirstWall()
    {
        // Wait until the walls are initialized
        yield return new WaitUntil(() => centerPointController.walls.Count > 0);

        // Move to the first wall
        currentWallIndex = 0;
        MoveToWall(currentWallIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (centerPointController != null && centerPointController.walls.Count > 1)
        {
            isCrawling = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightControl);
            isMovingLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            isMovingRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

            if (isCrawling == false)
            {
                walk();
            }
            else
            {
                if (wasCrawling == false)
                {
                    walk();
                }
                else
                {
                    crawl();
                }
            }
        }

        //Debug.Log($"Time since last move: {timeSinceLastMove}");
        timeSinceLastMove += Time.deltaTime;

        void walk()
        {
            // Check if enough time has passed since the last move
            if (timeSinceLastMove >= moveInterval)
            {
                if (isMovingLeft)
                {
                    MoveToPreviousWall();
                    timeSinceLastMove = 0;

                    if (isCrawling)
                        wasCrawling = true;
                    else
                        wasCrawling = false;
                }
                if (isMovingRight)
                {
                    MoveToNextWall();
                    timeSinceLastMove = 0;

                    if (isCrawling)
                        wasCrawling = true;
                    else
                        wasCrawling = false;
                }
            }
        }

        void crawl()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                MoveToPreviousWall();
                wasCrawling = true;
            }

            // Check for right arrow key or 'D' key (optional, to move forward as well)
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                MoveToNextWall();
                wasCrawling = true;
            }
        }
    }

    void MoveToWall(int wallIndex)
    {
        if (wallIndex >= 0 && wallIndex < centerPointController.walls.Count)
        {
            GameObject wall = centerPointController.walls[wallIndex];
            if (wall != null)
            {
                // Set player position to the center of the wall
                transform.position = new Vector3(wall.transform.position.x, 0.5f, wall.transform.position.z);
                transform.rotation = wall.transform.rotation;

                /*
                Vector3 currentWallRotation = wall.transform.rotation.eulerAngles;
                currentWallRotation.y -= 90f;
                transform.rotation = Quaternion.Euler(currentWallRotation);
                */
            }
        }
        else
        {
            Debug.LogError("Wall index out of range");
        }
    }

    void MoveToPreviousWall()
    {
        if (centerPointController.walls.Count > 0)
        {
            currentWallIndex = (currentWallIndex - 1 + centerPointController.walls.Count) % centerPointController.walls.Count;
            MoveToWall(currentWallIndex);
        }
    }

    void MoveToNextWall()
    {
        if (centerPointController.walls.Count > 0)
        {
            currentWallIndex = (currentWallIndex + 1) % centerPointController.walls.Count;
            MoveToWall(currentWallIndex);
        }
    }
}

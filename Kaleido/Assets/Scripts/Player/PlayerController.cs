using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject centerPoint;
    private CenterPointController centerPointController;
    public GameObject standardBulletPrefab;

    public int currentWallIndex;

    public float moveInterval = 0.05f; // Time in seconds to move to the next wall
    private float timeSinceLastMove = 0f;

    public bool movementLockedByCenterPoint = false;

    public float defaultShotCharge = 1f; // Scale of the current shot, 1 when walking, higher when charging a shot
    public float shotCharge = 1f; // Scale of the current shot, 1 when walking, higher when charging a shot
    public float maxShotCharge = 10f;
    public float shotChargeMultiplier = 1.02f; //Multiplier for how quickly shots charge when holding Shoot while crawling
    public float defaultShotChargeDelay = 1f; //Delay in seconds after the player shoots a charged shot
    public float currShotChargeDelay = 1f; //Delay in seconds after the player shoots a charged shot
    public float chargeCoolDownMultiplier = 1f; //Multiplier for how quickly the player can shoot again after a charged shot

    public float shotInterval = 0.25f; // Time in seconds to move to the next wall
    private float timeSinceLastShot = 0f;

    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private bool wasMovingLeft = false;
    private bool wasMovingRight = false;
    private bool isWalking = false; //Is walking is just isMovingLeft or isMovingRight
    private bool wasWalking = false; //Was walking is just wasMovingLeft or wasMovingRight
    private bool isCrawling = false;
    private bool wasCrawling = false; //Was the player crawling on the last update (Allows for delay to feel like you're slowing down)
    
    private bool isHoldingShoot = false;
    private bool wasHoldingShoot = false; //Was the player shooting on the last update (Allows for charged shots to be held after crawling)

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

        // Move to the random wall
        MoveToWall(centerPointController.GetRandomWallIndex());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"Shot charge: {shotCharge}, shot charge delay: {currShotChargeDelay}");
        //Debug.Log($"movement locked: {movementLockedByCenterPoint}");

        if (currShotChargeDelay > 0)
            currShotChargeDelay -= Time.deltaTime * chargeCoolDownMultiplier;

        if (currShotChargeDelay < 0)
            currShotChargeDelay = 0;

        if (centerPointController != null && centerPointController.walls.Count > 1)
        {
            //check if we were walking or crawling on the previous frame
            if (isMovingLeft)
            {
                wasMovingLeft = true;
                wasWalking = true;
            }
            if (isMovingRight)
            {
                wasMovingRight = true;
                wasWalking = true;
            }
            if (isCrawling)
                wasCrawling = true;
            if (isHoldingShoot)
                wasHoldingShoot = true;

            //check if we're walking or crawling on the current frame
            isMovingLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            isMovingRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
            isWalking = (isMovingLeft || isMovingRight);
            isCrawling = Input.GetKey(KeyCode.LeftShift);
            isHoldingShoot = Input.GetKey(KeyCode.Space);

            if (isCrawling)
            {
                //crawling
                if (wasCrawling == false)
                {
                    //we were NOT crawling on the last frame, now we are

                    //One final frame of walking when we first press Crouch
                    //Subject to removal if it ruins the charged shooting mechanic
                    Walk();
                }

                if (isHoldingShoot)
                {
                    ChargeShot();
                }
                else if (wasHoldingShoot)
                {
                    if (shotCharge > defaultShotCharge)
                    {
                        Shoot();
                        ResetShotCharge();
                    }
                }

                Crawl();
            }
            else
            {
                //not crawling
                if (isHoldingShoot)
                {
                    if (timeSinceLastShot >= shotInterval)
                    {
                        if (shotCharge > defaultShotCharge || currShotChargeDelay == 0)
                        {
                            Shoot();
                            ResetShotCharge();
                        }
                    }
                }
                else
                {
                    if (shotCharge > defaultShotCharge)
                    {
                        ResetShotCharge();
                    }
                }

                Walk();
            }

            wasMovingLeft = false;
            wasWalking = false;
            wasMovingRight = false;
            wasWalking = false;
            wasCrawling = false;
            wasHoldingShoot = false;
        }

        timeSinceLastShot += Time.deltaTime;
        timeSinceLastMove += Time.deltaTime;

        void ChargeShot()
        {
            if (shotCharge < maxShotCharge)
            {
                //charge shot
                shotCharge *= shotChargeMultiplier;
            }
        }

        void ResetShotCharge()
        {
            shotCharge = defaultShotCharge;
        }

        void Shoot()
        {
            GameObject bullet = Instantiate(standardBulletPrefab, transform.position, Quaternion.identity);
            StandardBulletController bulletController = bullet.GetComponent<StandardBulletController>();
            bulletController.bulletCharge = shotCharge;

            //Apply the delay after shooting a charged shot
            if (shotCharge > defaultShotCharge)
                currShotChargeDelay = defaultShotChargeDelay;

            timeSinceLastShot = 0;
        }

        void Walk()
        {
            if (movementLockedByCenterPoint == false)
            {
                // Check if enough time has passed since the last move
                if (timeSinceLastMove >= moveInterval)
                {
                    if (isMovingLeft)
                    {
                        //walking left
                        MoveToPreviousWall();
                        timeSinceLastMove = 0;
                    }
                    if (isMovingRight)
                    {
                        //walking right
                        MoveToNextWall();
                        timeSinceLastMove = 0;
                    }
                }
            }
        }

        void Crawl()
        {
            if (movementLockedByCenterPoint == false)
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
    }

    void MoveToWall(int wallIndex)
    {
        if (wallIndex >= 0 && wallIndex < centerPointController.walls.Count)
        {
            GameObject wall = centerPointController.walls[wallIndex];
            if (wall != null)
            {
                // Set player current wall index
                currentWallIndex = wallIndex;

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

    public void MoveToClosestWallByIndex()
    {
        List<GameObject> walls = centerPointController.walls;

        if (walls == null || walls.Count == 0) return;
        if (walls.Count == 1)
            MoveToWall(0);

        int leftIndex = (currentWallIndex - 1 + walls.Count) % walls.Count;
        int rightIndex = (currentWallIndex + 1) % walls.Count;

        float distanceToLeft = Vector3.Distance(transform.position, walls[leftIndex].transform.position);
        float distanceToRight = Vector3.Distance(transform.position, walls[rightIndex].transform.position);

        if (distanceToLeft < distanceToRight)
        {
            currentWallIndex = leftIndex;
            transform.position = walls[leftIndex].transform.position;
        }
        else
        {
            currentWallIndex = rightIndex;
            transform.position = walls[rightIndex].transform.position;
        }
    }

    void MoveToPreviousWall()
    {
        if (centerPointController.walls.Count > 0)
        {
            MoveToWall((currentWallIndex - 1 + centerPointController.walls.Count) % centerPointController.walls.Count);
        }
    }

    void MoveToNextWall()
    {
        if (centerPointController.walls.Count > 0)
        {
            MoveToWall((currentWallIndex + 1) % centerPointController.walls.Count);
        }
    }
}

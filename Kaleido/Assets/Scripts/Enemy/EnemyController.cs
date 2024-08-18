using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject centerPoint;
    private CenterPointController centerPointController;
    private int currentWallIndex;

    public float moveInterval = 0.05f; // Time in seconds to move to the next wall
    private float timeSinceLastMove = 1f;

    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    [Range(0, 100)]
    [SerializeField]
    private int health = 100;
    public int Health
    {
        get { return health; }
        set { health = Mathf.Clamp(value, 0, 100); }
    }

    [Range(1, 100)]
    [SerializeField]
    private int speed = 1;
    public int Speed
    {
        get { return speed; }
        set { speed = Mathf.Clamp(value, 1, 100); }
    }

    public Types type;
    public enum Types
    {
        Red = 1,
        Blue = 2,
        Cyan = 3,
        Orange = 4
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (centerPoint != null)
        {
            centerPointController = centerPoint.GetComponent<CenterPointController>();
            if (centerPointController != null)
            {
                //StartCoroutine(InitializeAndMoveToFirstWall());
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

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}

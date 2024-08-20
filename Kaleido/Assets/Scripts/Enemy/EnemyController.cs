using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject centerPoint;

    public GameObject currWall;
    public GameObject prevWall;

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
    private float moveSpeed = 1f;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = Mathf.Clamp(value, 1f, 100f); }
    }

    public Vector3 targetPosition;

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
        switch (type)
        {
            case Types.Red:
                health = 50;
                moveSpeed = 5;
                break;
            case Types.Blue:
                health = 50;
                moveSpeed = 6;
                break;
            case Types.Cyan:
                health = 75;
                moveSpeed = 4;
                break;
            case Types.Orange:
                health = 100;
                moveSpeed = 3;
                break;
            default:
                break;
        }

        prevWall = currWall;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}

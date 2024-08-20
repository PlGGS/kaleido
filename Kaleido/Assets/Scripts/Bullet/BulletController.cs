using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    public GameObject centerPoint;
    public GameObject player;

    public GameObject currWall;
    public GameObject prevWall;

    public Vector3 originalPosition;
    public Vector3 shotDirection = Vector3.down;
    public float despawnDistance = 500;

    [Range(1, 100)]
    [SerializeField]
    private float moveSpeed = 1f;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = Mathf.Clamp(value, 1f, 1000f); }
    }

    public Types type;
    public enum Types
    {
        Standard = 1,
        Charged = 2
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        switch (type)
        {
            case Types.Standard:
                moveSpeed = 100;
                break;
            case Types.Charged:
                moveSpeed = 60;
                break;
            default:
                break;
        }

        originalPosition = transform.position;

        prevWall = currWall;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Move the bullet in the shot direction
        transform.position += shotDirection * MoveSpeed * Time.deltaTime;

        // Calculate the distance traveled from the spawn position
        float distanceTraveled = Vector3.Distance(originalPosition, transform.position);

        // Check if the bullet has traveled beyond the despawn distance
        if (distanceTraveled >= despawnDistance)
        {
            // Despawn the bullet
            Destroy(gameObject);
        }
    }
}

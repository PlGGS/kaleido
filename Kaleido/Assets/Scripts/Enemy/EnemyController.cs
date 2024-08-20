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
        prevWall = currWall;

        switch (type)
        {
            case Types.Red:
                health = 50;
                moveSpeed = 1;
                break;
            case Types.Blue:
                break;
            case Types.Cyan:
                break;
            case Types.Orange:
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected IEnumerator TranslateEnemyAtMoveSpeed(Vector3 targetPosition)
    {
        // Wait until the next frame to ensure Start has been calledG
        yield return null;

        Vector3 startPosition = transform.position;

        float distance = Vector3.Distance(transform.position, targetPosition);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            //Check again if the attached script is missing (meaning the wall was destroyed)
            if (this.gameObject == null) yield break;

            WallController wallController = transform.parent.gameObject.GetComponentInChildren<WallController>();
            wallController.Elongate(remainingDistance);
            //wallController.currElongationFactor = remainingDistance;


            // Lerp position over time
            transform.position = Vector3.Lerp(startPosition, targetPosition, 1 - (remainingDistance / distance));

            remainingDistance -= Time.deltaTime * moveSpeed;

            yield return null;
        }

        //Check again if the attached script is missing (meaning the wall was destroyed)
        if (this.gameObject == null) yield break;

        // Final adjustments to ensure accuracy
        transform.position = targetPosition;
        GetCenterPointController().RemoveWall(currWall);
    }

    protected CenterPointController GetCenterPointController()
    {
        if (centerPoint != null)
        {
            return centerPoint.GetComponent<CenterPointController>();
        }

        Debug.LogError("CenterPoint is not assigned");
        return null;
    }
}

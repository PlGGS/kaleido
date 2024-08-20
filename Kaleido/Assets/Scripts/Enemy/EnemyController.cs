using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject centerPoint;
    public DifficultyManager difficultyManager;

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

    public float bulletScaleAmt = 0.1f; // Scale amount per bullet hit
    public float chargedBulletScaleAmt = 0.2f; // Scale amount per charged bullet hit
    public float maxScaleAmt = 5f; // Max scale amount before enemy dies

    private Vector3 originalScale;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        DifficultyManager difficultyManager = centerPoint.GetComponent<DifficultyManager>();

        switch (type)
        {
            case Types.Red:
                Health = 25;
                MoveSpeed = 3f;
                break;
            case Types.Blue:
                Health = 10;
                MoveSpeed = 6f;
                break;
            case Types.Cyan:
                Health = 40;
                MoveSpeed = 4f;
                break;
            case Types.Orange:
                Health = 50;
                MoveSpeed = 3f;
                break;
            default:
                break;
        }

        float healthMultiplier = 1 + ((difficultyManager.currentLevel * 0.1f) * 2.5f); // Increase health by 10% per level
        float speedMultiplier = 1 + ((difficultyManager.currentLevel * 0.1f) * 2.5f); // Increase speed by 5% per level

        health = Mathf.RoundToInt(Health * healthMultiplier);
        moveSpeed = MoveSpeed * speedMultiplier;

        prevWall = currWall;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        WallController wallController = transform.parent.gameObject.GetComponentInChildren<WallController>();

        float distance = Vector3.Distance(transform.position, transform.parent.GetChild(0).position);
        float remainingDistance = distance;
        //If the enemy is below the 
        if (remainingDistance > 0 && transform.position.y < 0)
        {
            wallController.Elongate(remainingDistance);

            transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);

            remainingDistance -= Time.deltaTime * MoveSpeed;
        }
        else
        {
            // Final adjustments to ensure accuracy
            transform.position = targetPosition;

            //TODO maybe remove the enemy from the wall first before removing the wall in order to have the enemy blink to be captured
            //if the player captures the enemy before they fully dissappear, the wall spawns back in
            centerPoint.GetComponent<CenterPointController>().RemoveWall(transform.parent.gameObject); //The enemy should be a child of the wall
        }
    }

    public void OnBulletHit(float bulletCharge)
    {
        float scaleIncrement = bulletCharge > 0 ? bulletCharge * chargedBulletScaleAmt : bulletScaleAmt;
        Vector3 newScale = transform.localScale + new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);

        if (newScale.x >= maxScaleAmt * originalScale.x)
        {
            Die();
        }
        else
        {
            transform.localScale = newScale;
        }
    }

    private void Die()
    {
        // TODO make the enemy explode or something

        Destroy(gameObject);
    }
}

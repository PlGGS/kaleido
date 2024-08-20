using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RedEnemyController : EnemyController
{
    public float remainingDistance;

    // Start is called before the first frame update
    protected override void Start()
    {
        type = Types.Red;
        base.Start();


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

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
}

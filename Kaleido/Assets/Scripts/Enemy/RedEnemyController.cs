using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RedEnemyController : EnemyController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        type = Types.Red;


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //StartCoroutine(TranslateEnemyAtMoveSpeed(GetCurrWall().GetComponent<WallController>().GetWallTopCenterPosition())); //TODO this is shit but for now it's fine

        //Vector3 startPosition = transform.position;

        WallController wallController = transform.parent.gameObject.GetComponentInChildren<WallController>();

        float distance = Vector3.Distance(transform.position, transform.parent.GetChild(0).position);
        float remainingDistance = distance;
        if (remainingDistance > 0)
        {
            wallController.Elongate(remainingDistance);

            transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
                //.position = Vector3.MoveTowards(startPosition, targetPosition, 1 - (remainingDistance / distance));

            remainingDistance -= Time.deltaTime * MoveSpeed;
        }
        else
        {
            // Final adjustments to ensure accuracy
            transform.position = targetPosition;
            GetCenterPointController().RemoveWall(transform.parent.gameObject); //The enemy should be a child of the wall
        }
    }
}

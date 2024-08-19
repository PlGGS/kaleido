using System.Collections;
using System.Collections.Generic;
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

        if (isMoving == false)
        {
            StartCoroutine(TranslateEnemyAtMoveSpeed(GetCurrWall().GetComponent<WallController>().GetWallTopCenterPosition())); //TODO this is shit but for now it's fine
            isMoving = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemyController : EnemyController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        type = Types.Blue;
        base.Start();


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();


    }
}

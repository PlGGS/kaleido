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

        
    }
}

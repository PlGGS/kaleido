using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StandardBulletController : BulletController
{
    protected override void Start()
    {
        type = Types.Standard;
        base.Start();


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        
    }
}

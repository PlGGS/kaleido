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

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        // Check if the other collider is an enemy
        if (other.CompareTag("Enemy"))
        {
            // Get the EnemyController from the collided enemy
            EnemyController enemyController = other.GetComponentInParent<EnemyController>();

            if (enemyController != null)
            {
                // Call OnBulletHit() on the enemy, passing the bulletCharge
                enemyController.OnBulletHit(bulletCharge);

                // Optionally, destroy the bullet or disable it
                Destroy(gameObject);
            }
        }
    }
}

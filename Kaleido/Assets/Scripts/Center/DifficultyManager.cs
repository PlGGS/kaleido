using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DifficultyManager : MonoBehaviour
{
    public GameManager gameManager;

    public float enemySpeedMultiplier = 1.1f;
    public float enemyHealthMultiplier = 1.1f;

    void Start()
    {
        UpdateDifficulty();
    }

    public void IncreaseLevel()
    {
        //TODO display text with currentLevel number
        //TODO play level-up sound effect

        gameManager.currentLevel++;
        UpdateDifficulty();
    }

    private void UpdateDifficulty()
    {
        //float newSpeed = gameManager.UpdateAllEnemySpeeds() * Mathf.Pow(enemySpeedMultiplier, gameManager.currentLevel - 1);
        //float newHealth = gameManager.UpdateAllEnemyMaxHealth() * Mathf.Pow(enemyHealthMultiplier, gameManager.currentLevel - 1);

        // Apply new difficulty settings to enemies
        // Example: enemyController.SetSpeed(newSpeed);
        // Example: enemyController.SetHealth(newHealth);
    }
}

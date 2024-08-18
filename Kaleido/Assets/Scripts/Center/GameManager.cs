using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CenterPointController centerPointController;
    public DifficultyManager difficultyManager;

    public GameObject redEnemyPrefab;
    public GameObject pinkEnemyPrefab;
    public GameObject cyanEnemyPrefab;
    public GameObject orangeEnemyPrefab;

    public int currentLevel = 1;

    public float baseSpawnRate = 1.0f;
    public int baseEnemyCount = 5;
    
    void Start()
    {   
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        float spawnRate = baseSpawnRate / currentLevel; // Decrease spawn rate as level increases
        int enemyCount = baseEnemyCount + currentLevel; // Increase number of enemies with level

        for (int i = 0; i < enemyCount; i++)
        {
            // Instantiate enemies at random positions or predefined points
            //TODO get random wall position from centerPointController
            //Instantiate(redEnemyPrefab, GetRandomWallPosition(), Quaternion.identity);
        }

        // Optionally, schedule next spawn or adjust other parameters
    }
    public void LevelUp()
    {
        currentLevel++;
        SpawnEnemies();
    }
}

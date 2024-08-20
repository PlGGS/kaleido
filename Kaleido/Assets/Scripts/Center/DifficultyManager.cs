using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Assuming you use UI Text for displaying level up text

/// <summary>
/// Manages enemy spawning
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    public CenterPointController centerPointController;

    public GameObject redEnemyPrefab;
    public GameObject blueEnemyPrefab;
    public GameObject cyanEnemyPrefab;
    public GameObject orangeEnemyPrefab;

    private float weightRed = 4f;
    private float weightBlue = 3f;
    private float weightCyan = 2f;
    private float weightOrange = 1f;

    private GameObject[] enemyPrefabs;
    private float[] weights;

    public Text levelUpText; // UI Text for level up display
    public AudioClip levelUpSound; // Sound effect for level up
    public AudioSource audioSource; // AudioSource to play the sound

    private float baseEnemySpawnRate = 1.0f; //Seconds between enemy spawns during a spawn wave
    private float spawnRateVariance = 1.0f; //Max time in seconds we might add between two enemy spawn times
    private float extraEnemyVariance = 1.0f; //Max time in seconds we might add between two enemy spawn times
    private float baseWaveSpawnRate = 5.0f; //Seconds to wait before beginning to spawn another wave of enemies
    private int baseWaveCount = 5;
    private int baseEnemyCount = 5;

    public float levelUpDelay = 3.0f;

    public int maxLevel = 4; //Final level goes on forever
    public int currentLevel = 0;
    public float[] levelDurations = { 10, 20, 30, 45, 60 }; // Time to wait before leveling up

    void Start()
    {
        enemyPrefabs = new GameObject[] { redEnemyPrefab, blueEnemyPrefab, cyanEnemyPrefab, orangeEnemyPrefab };
        weights = new float[] { weightRed, weightBlue, weightCyan, weightOrange };

        //level0
        baseEnemyCount = 3;
        baseEnemySpawnRate = 5.0f;
        spawnRateVariance = 0.5f;
        extraEnemyVariance = 1.0f;
        baseWaveCount = 3;
        baseWaveSpawnRate = 10.0f;

        centerPointController.currPatternDefinition = VertexPattern.Patterns.Circle;
        centerPointController.currAmtWalls = 6;
        centerPointController.currRadius = 10;

        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            // Start spawning enemies for the current level
            StartCoroutine(SpawnEnemies());

            // Wait for the level duration
            yield return new WaitForSeconds(levelDurations[currentLevel]);

            // Call LevelUp to transition to the next level
            LevelUp();

            // Wait for LevelUp sequence to complete before starting the next cycle
            yield return new WaitUntil(() => !IsInvoking("SpawnEnemies"));
        }
    }

    IEnumerator SpawnEnemies()
    {
        // Wait until the next frame to ensure walls have been created
        yield return null;

        int waveCount = (baseWaveCount + /*I DONT HAVE TIME JUST CAST IT*/(int)GetRandomExtraEnemies()) + (currentLevel + 1);

        while (waveCount > 0)
        {
            float spawnRate = (baseEnemySpawnRate + GetRandomExtraTimeBetweenEnemySpawns()) / (currentLevel + 1); // Decrease spawn rate as level increases
            int enemyCount = (baseEnemyCount + /*I DONT HAVE TIME JUST CAST IT*/(int)GetRandomExtraEnemies()) + (currentLevel + 1); // Increase number of enemies with level

            for (int i = 0; i < enemyCount; i++)
            {
                GameObject wall = centerPointController.GetRandomWall();

                if (wall != null)
                {
                    WallController wallController = wall.GetComponentInChildren<WallController>();
                    //TODO only set the wall to being eaten if we spawn a red enemy
                    wallController.isBeingEaten = true;

                    GameObject enemy = Instantiate(GetWeightedRandomEnemyPrefab(), wallController.GetRectBottomCenterPosition(), Quaternion.identity);
                    enemy.transform.SetParent(wall.transform, false);
                    //enemy.transform.position = wallController.GetWallBottomCenterPosition();
                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    enemyController.centerPoint = this.gameObject;
                }

                yield return new WaitForSeconds(spawnRate);
            }

            yield return new WaitForSeconds(baseWaveSpawnRate);
        }
    }

    public GameObject GetWeightedRandomEnemyPrefab()
    {
        // Calculate the total weight
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        // Generate a random value between 0 and totalWeight
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        // Determine which prefab to return based on the random value
        float cumulativeWeight = 0f;
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                return enemyPrefabs[i];
            }
        }

        // Fallback, should never reach here if weights are correct
        return enemyPrefabs[0];
    }

    public float GetRandomExtraTimeBetweenEnemySpawns()
    {
        return UnityEngine.Random.Range(0, spawnRateVariance);
    }
    public float GetRandomExtraEnemies()
    {
        return UnityEngine.Random.Range(0, extraEnemyVariance);
    }

    public void LevelUp()
    {
        StopCoroutine(SpawnEnemies());
        StartCoroutine(LevelUpSequence());
    }

    IEnumerator LevelUpSequence()
    {
        // TODO Display text and play level up sound
        //levelUpText.text = "Level " + currentLevel;
        //levelUpText.gameObject.SetActive(true);
        //audioSource.PlayOneShot(levelUpSound);
        Debug.Log($"Level {currentLevel + 1} Complete!!");

        // Wait for the defined delay
        yield return new WaitForSeconds(levelUpDelay);

        // Hide the text and begin next level
        //levelUpText.gameObject.SetActive(false);

        /*
        baseEnemySpawnRate = 1.0f; //Seconds between enemy spawns during a spawn wave
        spawnRateVariance = 1.0f; //Max time in seconds we might add between two enemy spawn times
        extraEnemyVariance = 1.0f; //Max time in seconds we might add between two enemy spawn times
        baseWaveSpawnRate = 5.0f; //Seconds to wait before beginning to spawn another wave of enemies
        baseWaveCount = 5;
        baseEnemyCount = 5;
        */

        switch (currentLevel)
        {
            case 0:
                //in Start function
                break;
            case 1:
                baseEnemyCount = 3;
                baseEnemySpawnRate = 5.0f;
                spawnRateVariance = 0.5f;
                extraEnemyVariance = 1.0f;
                baseWaveCount = 3;
                baseWaveSpawnRate = 3.0f;

                centerPointController.currPatternDefinition = VertexPattern.Patterns.Circle;
                centerPointController.currAmtWalls = 6;
                centerPointController.currRadius = 12;
                break;
            case 2:
                baseEnemyCount = 4;
                baseEnemySpawnRate = 3.0f;
                spawnRateVariance = 1.0f;
                extraEnemyVariance = 1.0f;
                baseWaveCount = 5;
                baseWaveSpawnRate = 3.0f;

                centerPointController.currPatternDefinition = VertexPattern.Patterns.Circle;
                centerPointController.currAmtWalls = 8;
                centerPointController.currRadius = 10;
                break;
            case 3:
                baseEnemyCount = 5;
                baseEnemySpawnRate = 2.5f;
                spawnRateVariance = 1.0f;
                extraEnemyVariance = 2.0f;
                baseWaveCount = 5;
                baseWaveSpawnRate = 3.0f;

                centerPointController.currPatternDefinition = VertexPattern.Patterns.Circle;
                centerPointController.currAmtWalls = 24;
                centerPointController.currRadius = 12;
                break;
            case 4:
                baseEnemyCount = 6;
                baseEnemySpawnRate = 2.0f;
                spawnRateVariance = 1.0f;
                extraEnemyVariance = 3.0f;
                baseWaveCount = 5;
                baseWaveSpawnRate = 3.0f;

                centerPointController.currPatternDefinition = VertexPattern.Patterns.Circle;
                centerPointController.currAmtWalls = 36;
                centerPointController.currRadius = 15;
                break;

        }

        //From 0-4, 4 is max level
        if (currentLevel < 4)
            currentLevel++;

        //TODO set difficulty for next level here when we call SpawnEnemies ????
        StartCoroutine(SpawnEnemies());
    }
}

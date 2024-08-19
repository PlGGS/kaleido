using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Assuming you use UI Text for displaying level up text

public class GameManager : MonoBehaviour
{
    public CenterPointController centerPointController;
    public DifficultyManager difficultyManager;

    public GameObject redEnemyPrefab;
    public GameObject pinkEnemyPrefab;
    public GameObject cyanEnemyPrefab;
    public GameObject orangeEnemyPrefab;

    public Text levelUpText; // UI Text for level up display
    public AudioClip levelUpSound; // Sound effect for level up
    public AudioSource audioSource; // AudioSource to play the sound

    public int currentLevel = 1;

    public float baseSpawnRate = 1.0f;
    public float baseWaveSpawnRate = 5.0f;
    public int baseEnemyCount = 5;
    public float levelUpDelay = 2.0f;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        // Wait until the next frame to ensure walls have been created
        yield return null;

        while (true)
        {
            float spawnRate = baseSpawnRate / currentLevel; // Decrease spawn rate as level increases
            int enemyCount = baseEnemyCount + currentLevel; // Increase number of enemies with level

            for (int i = 0; i < enemyCount; i++)
            {
                (GameObject wall, int index) = centerPointController.GetRandomWall();

                if (wall != null)
                {
                    WallController wallController = wall.GetComponent<WallController>();
                    //TODO only set the wall to being eaten if we spawn a red enemy
                    wallController.isBeingEaten = true;

                    GameObject enemy = Instantiate(redEnemyPrefab, wallController.GetWallBottomCenterPosition(), Quaternion.identity);
                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    enemyController.centerPoint = this.gameObject;
                    enemyController.SetCurrWall(wall, index);
                }

                yield return new WaitForSeconds(spawnRate);
            }

            yield return new WaitForSeconds(baseWaveSpawnRate);
        }
    }

    public void LevelUp()
    {
        currentLevel++;
        StopCoroutine(SpawnEnemies());
        StartCoroutine(LevelUpSequence());
    }

    IEnumerator LevelUpSequence()
    {
        // TODO Display text and play level up sound
        //levelUpText.text = "Level " + currentLevel;
        //levelUpText.gameObject.SetActive(true);
        //audioSource.PlayOneShot(levelUpSound);

        // Wait for the defined delay
        yield return new WaitForSeconds(levelUpDelay);

        // Hide the text and restart spawning enemies
        levelUpText.gameObject.SetActive(false);
        StartCoroutine(SpawnEnemies());
    }
}

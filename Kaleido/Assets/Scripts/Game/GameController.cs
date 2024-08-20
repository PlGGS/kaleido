using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public List<SceneAsset> scenes = new List<SceneAsset>();
    public string currSceneName;

    public TextMeshProUGUI bestTimeTMP;
    public float bestTime = 0;
    public float currTime = 0;

    public bool isGameOver = false;

    void Awake()
    {
        // Prevent this GameObject from being destroyed on scene load
        DontDestroyOnLoad(this.gameObject);

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // Log the current scene name when the script starts
        Debug.Log(currSceneName);
    }

    void Update()
    {
        switch (currSceneName)
        {
            case "MainMenu":
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    LoadScene("Level1");
                }

                bestTimeTMP.text = $"Best Time: {TimeConverter.ConvertSecondsToTime(bestTime)}";
                break;
            case "Level1":
                if (isGameOver == false)
                {
                    currTime += Time.deltaTime;
                }
                else
                {
                    if (currTime > bestTime)
                        bestTime = currTime;


                }
                break;
            default:
                break;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Log the scene name whenever a new scene is loaded
        currSceneName = scene.name;
    }

    void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when the GameObject is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        foreach (var scene in scenes)
        {
            if (scene.name == sceneName)
            {
                SceneManager.LoadScene(sceneName);
                return;
            }
        }

        Debug.LogError("Scene " + sceneName + " not found in the scenes list.");
    }
}

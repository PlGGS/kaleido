using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public List<SceneAsset> scenes = new List<SceneAsset>();
    public string currSceneName;
    
    private TextMeshProUGUI mainMenuBestTimeTMP;
    private TextMeshProUGUI currTimeTMP;

    private GameObject player;
    private TextMeshProUGUI shotChargeTMP;
    
    private TextMeshProUGUI gameOverBestTimeTMP;
    private TextMeshProUGUI gameOverYourTimeTMP;
    public float bestTime = 0;
    public float currTime = 0;

    public bool isGameOver = false;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        // Check if there's already an instance of this object
        if (Instance != null)
        {
            Destroy(gameObject); // Destroy the duplicate
            return; // Just to be sure
        }
        
        Instance = this; // Set this as the instance

        DontDestroyOnLoad(gameObject); // Make it persist across scenes
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
    }

    void Start()
    {
        // Log the current scene name when the script starts
        Debug.Log(currSceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currSceneName = scene.name;

        switch (currSceneName)
        {
            case "MainMenu":
                mainMenuBestTimeTMP = GameObject.Find("Best Time").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                mainMenuBestTimeTMP.text = $"Best Time: {TimeConverter.ConvertSecondsToTime(bestTime)}";
                break;
            case "Level1":
                currTime = 0f;
                player = GameObject.Find("player");
                shotChargeTMP = GameObject.Find("Shot Charge").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                currTimeTMP = GameObject.Find("Current Time").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                break;
            case "GameOver":
                isGameOver = true;

                if (currTime > bestTime)
                    bestTime = currTime;

                gameOverBestTimeTMP = GameObject.Find("Best Time").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                gameOverYourTimeTMP = GameObject.Find("Your Time").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                gameOverBestTimeTMP.text = $"Best Time: {TimeConverter.ConvertSecondsToTime(bestTime)}";
                gameOverYourTimeTMP.text = $"Your Time: {TimeConverter.ConvertSecondsToTime(currTime)}";
                break;
            default:
                break;
        }
    }

    void Update()
    {
        switch (currSceneName)
        {
            case "MainMenu":
                if (Input.GetKeyDown(KeyCode.Return))
                    LoadScene("Level1");
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
                break;
            case "Level1":
                shotChargeTMP.text = $"Shot Charge: {(int)player.GetComponent<PlayerController>().shotCharge}";
                currTimeTMP.text = TimeConverter.ConvertSecondsToTime(currTime);
                currTime += Time.deltaTime;
                break;
            case "GameOver":
                if (Input.GetKeyDown(KeyCode.Return))
                    LoadScene("Level1");
                else if (Input.GetKeyDown(KeyCode.Escape))
                    LoadScene("MainMenu");
                break;
            default:
                break;
        }
    }

    void OnDestroy()
    {
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

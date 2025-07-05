
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool isPaused = false;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject PauseMenu;
    private GameObject[] UiSceneObjects;
    [SerializeField] private int Score = 0;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private GameObject WinPanel;
    private bool hasWon = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        StartCoroutine(setActiveScene());
        isPaused = false;
        Time.timeScale = 1;
        var UiScene = SceneManager.GetSceneByBuildIndex(0);
        if (UiScene.isLoaded)
        {
            UiSceneObjects = UiScene.GetRootGameObjects();
            // foreach (var obj in UiSceneObjects)
            // {
            //     obj.SetActive(false);
            // }
        }

        if (PlayerPrefs.GetInt("Continue", 0) == 1)
        {
            Score = PlayerPrefs.GetInt("Score", 0);
            ScoreText.text = $"{Score}";
            // Debug.Log("Load enemy");
        }
    }
    void save()
    {
        PlayerPrefs.SetInt("Score", Score);
        PlayerPrefs.Save();
    }

    IEnumerator setActiveScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }



    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            if (Player != null)
                Player.GetComponent<PlayerInput>().enabled = false;
            foreach (var obj in UiSceneObjects)
            {
                obj.SetActive(true);
            }
            WinPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {

            if (hasWon)
            {
                WinPanel.SetActive(true);
                return;
            }
            if (Player != null)
                Player.GetComponent<PlayerInput>().enabled = true;
            foreach (var obj in UiSceneObjects)
            {
                obj.SetActive(false);
            }
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }

    }

    public EnemyData GetEnemyData()
    {
        return GetComponent<JsonReader>().GetEnemyData();
    }




    public void IncreaseScore()
    {
        Score++;
        ScoreText.text = $"{Score}";
        if (Score >= 6)
        {
            if (Player != null)
                Player.GetComponent<PlayerInput>().enabled = false;
            Time.timeScale = 0;
            WinPanel.SetActive(true);
            hasWon = true;
        }
        save();
    }
}

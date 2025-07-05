using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject UiCamera;
    AudioManager audioManager;
    public Slider progressBar;
    private void Start()
    {
        audioManager = AudioManager.Instance;
    }
    public void LoadScene()
    {
        audioManager.StopAllAudio();
        // SceneManager.LoadScene(1);
        // SceneManager.LoadScene(2, LoadSceneMode.Additive);
        StartCoroutine(LoadSceneAsync());
    }

    public void Continue()
    {
        PlayerPrefs.SetInt("Continue", 1);
        PlayerPrefs.Save();
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
    }


    private IEnumerator LoadSceneAsync()
    {
        // Activate loading screen
        loadingScreen.SetActive(true);



        float totalProgress = 0f;
        int scenesLoaded = 0;
        // Loop through all scenes to load them one by one
        for (int i = 1; i <= 1; i++)
        {
            if (SceneManager.GetSceneByBuildIndex(i).isLoaded)
            {
                SceneManager.UnloadSceneAsync(i);
            }
            AsyncOperation operation = SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive);
            operation.allowSceneActivation = false;

            // Track the progress of the current scene
            while (!operation.isDone)
            {
                totalProgress = (scenesLoaded + Mathf.Clamp01(operation.progress / 0.9f)) / 2;

                if (progressBar != null)
                {
                    progressBar.value = totalProgress;
                }

                // Activate the scene when it’s fully loaded
                if (operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                    break;
                }

                yield return null;
            }

            scenesLoaded++;
        }

        loadingScreen.SetActive(false);
        // SceneManager.UnloadSceneAsync(0);
        MainMenu.SetActive(false);
        UiCamera.SetActive(false);
        // Deactivate the loading screen when done
    }
}

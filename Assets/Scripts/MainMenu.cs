using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Button continueButton;


    // Update is called once per frame

    void Start()
    {
        audioManager = AudioManager.Instance;
        audioManager.PlayAudioClip("Music", "MainMenu", true);
        musicSlider.onValueChanged.AddListener(value => { audioManager.SetVolumeOfCategory("Music", value); });
        SFXSlider.onValueChanged.AddListener(value => { audioManager.SetVolumeOfCategory("SFX", value); });
        int Health = PlayerPrefs.GetInt("Health");
        int Score = PlayerPrefs.GetInt("Score");
        Debug.Log(Health == default);
        if (Health == default || Health <= 0 || Score == 8)
        {
            continueButton.gameObject.SetActive(false);
        }
        else
        {
            continueButton.gameObject.SetActive(true);
        }
    }
    void Update()
    {
        int Health = PlayerPrefs.GetInt("Health");
        if (Health == default || Health <= 0)
        {
            continueButton.gameObject.SetActive(false);
        }
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     mainMenu.SetActive(true);

        //     settingsMenu.SetActive(false);
        // }
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void OpenSettings()
    {
        mainMenu.SetActive(false);

        settingsMenu.SetActive(true);
    }
    public void GoBack()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
}

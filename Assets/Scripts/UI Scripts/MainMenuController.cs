using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;

    [Header("Settings")]
    public GameObject settingsPanel;
    public string gameSceneName = "GameScene";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStart);
        
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettings);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuit);

        // Hide settings panel initially
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OnStart()
    {
        //SceneManager.LoadScene("tmpScene"); 
        //update this for when we have the start scene

        Debug.Log("Start Button");
    }

    public void OnQuit()
    {
        Application.Quit();
        Debug.Log("Quit Button");
    }

    public void OnSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        Debug.Log("Settings");
    }

    void OnSettingsClose()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    
}

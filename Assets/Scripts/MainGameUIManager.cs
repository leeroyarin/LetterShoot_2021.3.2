using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameUIManager : MonoBehaviour
{
    [SerializeField] GameObject levelFailureUI;
    [SerializeField] GameObject levelSuccessUI;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsUI;
    [SerializeField] GameObject pauseIcon;
    private void Awake()
    {
        EventManager.GameCompleted += EnableGameConclusionMenu;
    }
    private void OnDestroy()
    {
        EventManager.GameCompleted -= EnableGameConclusionMenu;

    }
    void EnableGameConclusionMenu(bool complete)
    {
        if(levelSuccessUI == null) print(null);
        if (complete)
        {
            levelSuccessUI.SetActive(true);
            pauseIcon.SetActive(false);

        }
        else
        {
            pauseIcon.SetActive(false);
        }
    }

    public void ButtonFunction(string functionName)
    {
        
        switch (functionName.ToUpper())
        {
            case "RESTART":
                GameSceneManager.SceneManagerInstance.ReloadScene();
                Time.timeScale = 1;
                break;
            case "EXIT":
            case "EXITTOMENU":
                Time.timeScale = 1;
                GameSceneManager.SceneManagerInstance.ChangeSceneOnName("MainMenu");
                break;
            case "NEXT":
            case "NEXT LEVEL":
                Time.timeScale = 1;
                GameSceneManager.SceneManagerInstance.ChangeNextLevelScene();
                break;
        }
    }

    void FullScreen(bool enable)
    {

    }

    public void ShowSettings(bool enable)
    {
        settingsUI.SetActive(enable);
    }

    public void ShowPauseMenu(bool enable)
    {
        Time.timeScale = enable? 0f:1f;
        pauseMenu.SetActive(enable);
        pauseIcon.SetActive(!enable);

    }
}



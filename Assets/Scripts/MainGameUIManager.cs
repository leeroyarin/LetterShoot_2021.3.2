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
        Curtain.CurtainInstance.SetPermanentCurtain();
        if (complete)
        {
            levelSuccessUI.SetActive(true);
            pauseIcon?.SetActive(false);

        }
        else
        {
            levelFailureUI.SetActive(true);
            pauseIcon?.SetActive(false);
        }
    }

    public void ButtonFunction(string functionName)
    {
        Time.timeScale = 1;

        switch (functionName.ToUpper())
        {
            case "RESTART":
                GameSceneManager.SceneManagerInstance.ReloadScene();
                break;
            case "EXIT":
            case "EXITTOMENU":
            case "STARTMENU":
                GameSceneManager.SceneManagerInstance.ChangeSceneOnName("StartMenu");
                break;
            case "NEXT":
            case "NEXT LEVEL":
            case "NEXTLEVEL":
                GameSceneManager.SceneManagerInstance.ChangeNextLevelScene();
                break;
            case "LEVELSELECTION":
            case "LEVEL SELECTION":
                GameSceneManager.SceneManagerInstance.ChangeSceneOnName("LevelSelection");
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
        AudioManager.Instance.OnPause(enable);
        pauseMenu.SetActive(enable);
        pauseIcon?.SetActive(!enable);
        InputManager.InputManagerInstance.enabled = !enable;

    }

    public void OnPointerUpThePauseButton(bool IsPointerUp)
    {
        InputManager.InputManagerInstance.enabled = !IsPointerUp;

    }
}



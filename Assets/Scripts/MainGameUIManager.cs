using UnityEngine;

/// <summary>
/// Manages the UI elements and user interactions during the main game.
/// </summary>
public class MainGameUIManager : MonoBehaviour
{
    [SerializeField] GameObject levelFailureUI;
    [SerializeField] GameObject levelSuccessUI;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsUI;
    [SerializeField] GameObject pauseIcon;

    /// <summary>
    /// Subscribes to the GameCompleted event when this script is enabled.
    /// </summary>
    private void Awake()
    {
        EventManager.GameCompleted += EnableGameConclusionMenu;
    }
    /// <summary>
    /// Unsubscribes from the GameCompleted event when this script is disabled.
    /// </summary>
    private void OnDestroy()
    {
        EventManager.GameCompleted -= EnableGameConclusionMenu;
    }

    /// <summary>
    /// Enables the appropriate game conclusion UI based on whether the game was successfully completed or not.
    /// </summary>
    /// <param name="p_complete">True if the game was successfully completed, false otherwise.</param>

    void EnableGameConclusionMenu(bool p_complete)
    {
        Curtain.CurtainInstance.SetPermanentCurtain();
        if (p_complete)
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
    /// <summary>
    /// Executes the function associated with the button clicked by the user.
    /// </summary>
    /// <param name="functionName">The name of the button function to execute.</param>
    public void ButtonFunction(string functionName)
    {
        //called from Unity Event button on click
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
            case "INSTRUCTION":
                GameSceneManager.SceneManagerInstance.ChangeSceneOnName("Instructions");

                break;
        }
    }

    void FullScreen(bool enable)
    {

    }
    /// <summary>
    /// Shows or hides the settings menu.
    /// </summary>
    /// <param name="p_enable">True to show the settings menu, false to hide.</param>
    public void ShowSettings(bool p_enable)
    {
        settingsUI.SetActive(p_enable); 

    }

    /// <summary>
    /// Shows or hides the pause menu and pauses or resumes the game.
    /// </summary>
    /// <param name="p_enable">True to show the pause menu and pause the game, false to hide the pause menu and resume the game.</param>

    public void ShowPauseMenu(bool p_enable)
    {
        Time.timeScale = p_enable? 0f:1f;
        AudioManager.Instance.OnPause(p_enable);
        pauseMenu.SetActive(p_enable);
        pauseIcon?.SetActive(!p_enable);
        InputManager.InputManagerInstance.enabled = !p_enable;
    }

    /// <summary>
    /// Enables or disables user input
    /// </summary>
    /// <param name="p_IsPointerUp"></param>
    public void OnPointerUpThePauseButton(bool p_IsPointerUp)
    {
        InputManager.InputManagerInstance.enabled = !p_IsPointerUp;

    }
}



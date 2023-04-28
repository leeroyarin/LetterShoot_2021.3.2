using UnityEngine;
using UnityEngine.SceneManagement;

class GameSceneManager : MonoBehaviour
{
    static GameSceneManager _sceneManager;

    /// <summary>
    /// Singleton instance of the GameSceneManager.
    /// </summary>
    public static GameSceneManager SceneManagerInstance
    {
        get 
        { 
            if (_sceneManager == null)
            {
                _sceneManager = FindObjectOfType<GameSceneManager>();
                if (_sceneManager == null)
                {
                    _sceneManager = Instantiate(_sceneManager.gameObject,AudioManager.Instance.transform).AddComponent<GameSceneManager>();
                }
            }
            return _sceneManager;
        }
    }

    private void Awake()
    {
        _sceneManager = this;
    }

    /// <summary>
    /// Changes the scene based on the provided scene name.
    /// </summary>
    /// <param name="sceneName">The name of the scene to be loaded.</param>
    public void ChangeSceneOnName(string sceneName)
    {
        AudioManager.Instance.StopAllSoundAtOnce();

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Changes the level based on the provided level number.
    /// </summary>
    /// <param name="levelNumber">The number of the level to be loaded.</param>
    public void ChangeLevel(int levelNumber)
    {
        AudioManager.Instance.StopAllSoundAtOnce();

        SceneManager.LoadScene(levelNumber);
    }

    /// <summary>
    /// Loads the next scene in the build order.
    /// </summary>
    public void ChangeNextLevelScene() => ChangeLevel(SceneManager.GetActiveScene().buildIndex + 1);

    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public void ReloadScene() {
        AudioManager.Instance.StopAllSoundAtOnce();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Called when the player successfully completes a level.
    /// </summary>
    public void OnSuccessfulGameComplete()
    {
        if (SceneManager.GetActiveScene().name == "Instructions")
        {
            return;
        }
        if (SceneManager.GetActiveScene().buildIndex >= PlayerData.PlayerCurrentLevel) PlayerData.PlayerCurrentLevel++;
    }

    /// <summary>
    /// Returns the name of the current scene.
    /// </summary>
    /// <returns>The name of the current scene.</returns>
    public string GetCurrentSceneName()=> SceneManager.GetActiveScene().name;
}

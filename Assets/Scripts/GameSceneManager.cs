using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

class GameSceneManager : MonoBehaviour
{
    static GameSceneManager _sceneManager;
    public static GameSceneManager SceneManagerInstance
    {
        get 
        { 
            if (_sceneManager == null) _sceneManager = FindObjectOfType<GameSceneManager>();
            return _sceneManager;
        }
    }

    private void Awake()
    {
        if (_sceneManager == null)
        {
            _sceneManager = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void ChangeSceneOnName(string sceneName)
    {
        AudioManager.Instance.StopAllSoundAtOnce();

        SceneManager.LoadScene(sceneName);
    }

    public void ChangeLevel(int levelNumber)
    {
        AudioManager.Instance.StopAllSoundAtOnce();

        SceneManager.LoadScene(levelNumber);
    }
    public void ChangeNextLevelScene() => ChangeLevel(SceneManager.GetActiveScene().buildIndex + 1);
    public void ReloadScene() {
        AudioManager.Instance.StopAllSoundAtOnce();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnSuccessfulGameComplete()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerData.PlayerCurrentLevel) PlayerData.PlayerCurrentLevel++;
    }
}

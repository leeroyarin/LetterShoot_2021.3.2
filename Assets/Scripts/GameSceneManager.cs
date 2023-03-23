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
        _sceneManager = this;
 //       DontDestroyOnLoad(this.gameObject);
    }
    public void ChangeSceneOnName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }
    public void ChangeNextLevelScene() => ChangeLevel(SceneManager.GetActiveScene().buildIndex + 1);
    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

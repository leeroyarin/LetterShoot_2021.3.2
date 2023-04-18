using UnityEngine;
using UnityEngine.SceneManagement;

class GameSceneManager : MonoBehaviour
{
    static GameSceneManager _sceneManager;
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
        if (SceneManager.GetActiveScene().name == "Instruction") return;
        if (SceneManager.GetActiveScene().buildIndex >= PlayerData.PlayerCurrentLevel) PlayerData.PlayerCurrentLevel++;
    }

    public string GetCurrentSceneName()=> SceneManager.GetActiveScene().name;
}

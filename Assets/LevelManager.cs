using PathCreation;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int currentSelectionLevel;
    int availableLevelsCount;
    [SerializeField] PlayerLevelLocator playerLevelLocator;
    [SerializeField] PathCreator path;
    [SerializeField] GameObject[] levelPoints;
    // Start is called before the first frame update

    void Start()
    {
        availableLevelsCount = PlayerData.PlayerCurrentLevel;

        levelPoints = GameObject.FindGameObjectsWithTag("Level");
        currentSelectionLevel = PlayerData.PlayerCurrentLevel;

        SetPlayerLevelLocatorPosition();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) OnPreviousLevelClick();
        else if (Input.GetKeyDown(KeyCode.D)) OnNextLevelClick();
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) OnPreviousLevelClick();
        else if (Input.GetKeyDown(KeyCode.RightArrow)) OnNextLevelClick();
        else if (Input.GetKeyDown(KeyCode.KeypadEnter)) PlayTheSelectedLevel();
        else if (Input.GetKeyDown(KeyCode.Space)) PlayTheSelectedLevel();
        else if (Input.GetKeyDown(KeyCode.Escape)) GameSceneManager.SceneManagerInstance.ChangeSceneOnName("StartMenu");


    }

    public void OnNextLevelClick()
    {
        if (currentSelectionLevel >= 20 || currentSelectionLevel>=availableLevelsCount) return;
        if(!playerLevelLocator.OnMoveToNext(true)) return ;
        currentSelectionLevel+=1;
        print(currentSelectionLevel);

    }
    public void OnPreviousLevelClick()
    {
        if (currentSelectionLevel <= 1) return;
        if(!playerLevelLocator.OnMoveToNext(false)) return;
        currentSelectionLevel-=1;
        print(currentSelectionLevel);

    }

    void SetPlayerLevelLocatorPosition()
    {
        if (currentSelectionLevel == 1)
        {
            playerLevelLocator.SetPositionOfPlayerLevelLocator(0);
            return;
        }
        print(currentSelectionLevel);
        playerLevelLocator.SetPositionOfPlayerLevelLocator(path.path.GetClosestDistanceAlongPath(levelPoints[availableLevelsCount - 1].transform.position + new Vector3(0, 0.68f, 0)));
    }

    public void PlayTheSelectedLevel()
    {
        GameSceneManager.SceneManagerInstance.ChangeLevel(currentSelectionLevel);
    }
}

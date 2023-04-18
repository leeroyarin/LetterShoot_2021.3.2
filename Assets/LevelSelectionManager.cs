using PathCreation;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    int currentSelectedLevel;
    int availableLevelsCount;
    int totalLevelCount;

    float distanceBetweenPointsInPath;
    [SerializeField] PlayerLevelLocator playerLevelLocator;
    [SerializeField] PathCreator mapTrackPath;
    [SerializeField] Transform levelPointCollector;
    [SerializeField] List<Transform> levelPoints;

    private void Awake()
    {
        SetLevelPointOnMap();
        SetPlayerLevelLocatorPosition();

        void SetLevelPointOnMap()
        {
            //Get all the reference of the available transforms in child
            GameObject[] levelObjects = GameObject.FindGameObjectsWithTag("Level");
            foreach (GameObject levels in levelObjects) levelPoints.Add(levels.transform);
            
            //gets values from the path
            int levelPointsCount = levelPoints.Count;
            float pathLength = mapTrackPath.path.length;

            totalLevelCount = levelPointsCount;
            
            //finds the exact distance placement for each levelPoints
            distanceBetweenPointsInPath = pathLength / levelPointsCount;
            
            //counter for the loop
            float pathDistance = 0;
            int level = 1;
            foreach (Transform levelPoint in levelPoints)
            {
                levelPoint.position = mapTrackPath.path.GetPointAtDistance(pathDistance);
                levelPoint.GetComponentInChildren<TextMeshPro>().text = ("Level " + level);
                pathDistance += distanceBetweenPointsInPath;
                level++;
            }
        }
    }

    public void OnNextLevelClick()
    {
        if (currentSelectedLevel >= totalLevelCount || currentSelectedLevel >= availableLevelsCount) return;
        if (!playerLevelLocator.OnMoveToNextPointOnMapPath(true,distanceBetweenPointsInPath)) return;
        currentSelectedLevel += 1;


    }
    public void OnPreviousLevelClick()
    {
        if (currentSelectedLevel <= 1) return;
        if (!playerLevelLocator.OnMoveToNextPointOnMapPath(false, distanceBetweenPointsInPath)) return;
        currentSelectedLevel -= 1;

    }

    void SetPlayerLevelLocatorPosition()
    {
       
        //sets the current available levels count to players current level
        availableLevelsCount = PlayerData.PlayerCurrentLevel;

        //incase the current available level of player exceeds more that total available level, available level becomes as much as total level
        if(availableLevelsCount > totalLevelCount) availableLevelsCount = totalLevelCount;

        //then finally the player's current level count is set according to current available count
        currentSelectedLevel = availableLevelsCount;
        int l_index = currentSelectedLevel - 1;
        float currentDistance = l_index * distanceBetweenPointsInPath;

        playerLevelLocator.SetPositionOfPlayerLevelLocator(currentDistance);
    }

    public void PlayTheSelectedLevel()
    {
        GameSceneManager.SceneManagerInstance.ChangeLevel(currentSelectedLevel);
    }
}

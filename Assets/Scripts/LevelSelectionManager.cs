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

    // Sets up the level points on the map and sets the player's starting position
    private void Awake()
    {
        SetLevelPointOnMap();
        SetPlayerLevelLocatorPosition();

        // Sets the position of each level point on the map based on the distance between points
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

            // Set the position of each level point on the path and set its label text
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

    // Called when the "Next Level" button is clicked
    public void OnNextLevelClick()
    {
        // If the player has reached the last level or the last unlocked level, do nothing
        if (currentSelectedLevel >= totalLevelCount || currentSelectedLevel >= availableLevelsCount) return;

        // Move the player to the next level point on the map
        if (!playerLevelLocator.OnMoveToNextPointOnMapPath(true,distanceBetweenPointsInPath)) return;

        // Increment the current selected level
        currentSelectedLevel += 1;


    }

    // Called when the "Previous Level" button is clicked
    public void OnPreviousLevelClick()
    {
        // If the player is on the first level, do nothing
        if (currentSelectedLevel <= 1) return;

        // Move the player to the previous level point on the map
        if (!playerLevelLocator.OnMoveToNextPointOnMapPath(false, distanceBetweenPointsInPath)) return;

        // Decrement the current selected level
        currentSelectedLevel -= 1;

    }

    // Sets the player's starting position based on their current level progress
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

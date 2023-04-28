using PathCreation;
using System.Collections;
using UnityEngine;

public class PlayerLevelLocator: MonoBehaviour
{
    [SerializeField] PathCreator pathCreator;
    [SerializeField] float speed;
    bool moveable = false;
    [SerializeField] Vector3 offset;
    float currentDistance = 0;

    /// <summary>
    /// Sets the position of the player level locator at the specified distance along the path.
    /// </summary>
    /// <param name="distance">The distance along the path where the player level locator should be placed.</param>
    public void SetPositionOfPlayerLevelLocator(float distance)
    {
        currentDistance = distance;
        transform.position = pathCreator.path.GetPointAtDistance(currentDistance) +offset;
    }

    /// <summary>
    /// Moves the player level locator to the next point on the path.
    /// </summary>
    /// <param name="right">Whether the player should move to the right or left along the path.</param>
    /// <param name="distanceToMove">The distance the player should move along the path.</param>
    /// <returns>Returns true if the player is able to move, false otherwise.</returns>
    public bool OnMoveToNextPointOnMapPath(bool right,float distanceToMove)
    {
        //if it si moveing returns false
        if (moveable) return false;

        // Set the moveable flag to true
        moveable = true;

        // Calculate the speed of the player based on the current speed value and direction
        speed = Mathf.Sqrt(speed * speed);
        speed *= right ? 1 : -1;

        // Stop all coroutines and start the MoveLocatorOfDistance coroutine
        StopAllCoroutines();
        StartCoroutine(MoveLocatorOfDistance());

        // Return true since the player is able to move
        return true;

        // Calculate the speed of the player based on the current speed value and direction
        IEnumerator MoveLocatorOfDistance()
        {
            // Define a time difference based on the fixed delta time
            float timeDifference = Time.fixedDeltaTime;

            // Set a temporary distance value to the current distance of the player level locator
            float l_currentTempDistance = currentDistance;

            // Set the position of the player level locator based on the current distance and offset
            transform.position = pathCreator.path.GetPointAtDistance(l_currentTempDistance) + offset;

            // Continue moving the player level locator while the moveable flag is set to true
            while (moveable)
            {
                // Wait for a fixed amount of time before continuing the loop
                yield return new WaitForSeconds(timeDifference);

                // Update the temporary distance based on the current speed and time difference
                l_currentTempDistance += speed * timeDifference;

                // If the player has moved past the desired distance and direction, set the temporary distance to the desired distance and stop moving
                if (l_currentTempDistance >= distanceToMove + currentDistance && right)
                {

                    l_currentTempDistance = currentDistance + distanceToMove;
                    moveable = false;
                }
                // If the player has moved past the desired distance in the opposite direction, set the temporary distance to the desired distance and stop moving
                if (l_currentTempDistance <= currentDistance - distanceToMove && !right)
                {

                    l_currentTempDistance = currentDistance - distanceToMove;
                    moveable = false;
                }
                //if the temp distance is less than 0 it gets setted to 0 again and movement will be falsed
                if (l_currentTempDistance <= 0)
                {
                    l_currentTempDistance = 0;
                    moveable = false;
                }
                transform.position = pathCreator.path.GetPointAtDistance(l_currentTempDistance) + offset;
            }
            currentDistance = l_currentTempDistance;
        }
    }
}
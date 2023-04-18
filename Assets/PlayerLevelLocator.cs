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
    public void SetPositionOfPlayerLevelLocator(float distance)
    {
        currentDistance = distance;
        transform.position = pathCreator.path.GetPointAtDistance(currentDistance) +offset;
    }

   
    public bool OnMoveToNextPointOnMapPath(bool right,float distanceToMove)
    {
        //if it si moveing returns false
        if (moveable) return false;

        //movable sets true incase it is false
        moveable = true;
        speed = Mathf.Sqrt(speed * speed);
        speed *= right ? 1 : -1;
        StopAllCoroutines();
        StartCoroutine(MoveLocatorOfDistance());
        return true;

        IEnumerator MoveLocatorOfDistance()
        {
            float timeDifference = Time.fixedDeltaTime;
            float l_currentTempDistance = currentDistance;
            transform.position = pathCreator.path.GetPointAtDistance(l_currentTempDistance) + offset;
            while (moveable)
            {
                yield return new WaitForSeconds(timeDifference);
                l_currentTempDistance += speed * timeDifference;
                if(l_currentTempDistance >= distanceToMove + currentDistance && right)
                {

                    l_currentTempDistance = currentDistance + distanceToMove;
                    moveable = false;
                }
                if (l_currentTempDistance <= currentDistance - distanceToMove && !right)
                {

                    l_currentTempDistance = currentDistance - distanceToMove;
                    moveable = false;
                }
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
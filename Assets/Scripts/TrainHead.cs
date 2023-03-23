using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHead : MonoBehaviour
{
    public float movementSpeed;
    float movementDistance = 0;
    public bool stop = false;

    private void OnEnable()
    {
        movementDistance = 0;
    }
    public IEnumerator MoveTrainOnPath(PathCreator trainPath)
    {
        while (!stop)
        {
            movementDistance += movementSpeed * Time.deltaTime;
            transform.position = trainPath.path.GetPointAtDistance(movementDistance);
            Quaternion rotation = trainPath.path.GetRotationAtDistance(movementDistance);
            transform.rotation = new Quaternion(0, 0, -rotation.x, rotation.w);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

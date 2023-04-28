using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TrainHead : MonoBehaviour
{
    public float movementSpeed;
    float movementDistance = 0;

    /// <summary>
    /// To make the train stop incase of gameComplete or destruction oafter reacing beyond the train track
    /// </summary>
    public bool stopped = false;
    [SerializeField] Light2D headLight;
    PathCreator trainPath;
    
    /// <summary>
    /// subcribe destoryTrainhead to wordCompleted event
    /// </summary>
    private void Awake()
    {
        EventManager.wordCompleted+=DestroyTrainHead;
    }
    /// <summary>
    /// When enabled movementDistance is set to 0 and lights of the train is deactivated after few seconds of scene starting or object's spawn
    /// </summary>
    private void OnEnable()
    {
        movementDistance = 0;
        StartCoroutine(DeactivateLight());

    }
    /// <summary>
    /// if train path reference is not set then the movement is stopped
    /// if the train is not stopped then the train head moves along the train path and if the trainHead reaches far than the train's path length the train is destroyed
    /// </summary>
    private void FixedUpdate()
    {
        if (trainPath == null) return;
        if (!stopped)
        {
            movementDistance += movementSpeed * Time.deltaTime;
            transform.position = trainPath.path.GetPointAtDistance(movementDistance);
            Quaternion rotation = trainPath.path.GetRotationAtDistance(movementDistance);
            transform.rotation = new Quaternion(0, 0, -rotation.x, rotation.w);
            if (movementDistance > trainPath.path.length)
            {
                stopped = true;
                Destroy(gameObject);
            }
        }
    }
    /// <summary>
    /// Waits for few secs and if the phase is night then the headlight is switched on
    /// and all coroutines are stopped
    /// </summary>
    /// <returns></returns>
    IEnumerator DeactivateLight()
    {
        yield return new WaitForSeconds(0.3f);
        if (GameManager.Instance.currentGamePhase == GameManager.GamePhase.Night)
        {
            headLight.enabled = true;
        }
        else headLight.enabled = false;
        StopAllCoroutines();
    }
    /// <summary>
    /// starts coroutine DestroyAfterFewSeconds
    /// </summary>
    void DestroyTrainHead()
    {
        StartCoroutine(DestroyAfterFewSeconds());
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnDestroy()
    {
        EventManager.wordCompleted-=DestroyTrainHead;
    }
    /// <summary>
    /// sets reference of train path
    /// </summary>
    /// <param name="path">train oath to take reference </param>
    public void SetTrainHeadOnTrack(PathCreator path)
    {

        trainPath = path;
    }
    /// <summary>
    /// waits for while and destroyes the TrainHead so that the train is destroyed only when the screen is covered with blank curtain
    /// And all ccoroutines are stopped
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyAfterFewSeconds()
    {
        yield return new WaitForSeconds(1);
        stopped = true;
        Destroy(this.gameObject);
        StopAllCoroutines();

    }
}

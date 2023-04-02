using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TrainHead : MonoBehaviour
{
    public float movementSpeed;
    float movementDistance = 0;
    public bool stop = false;
    [SerializeField] Light2D headLight;
    PathCreator trainPath;
    Coroutine runningCoroutine;
    

    private void Awake()
    {
        EventManager.wordCompleted+=DestroyTrainHead;
    }
    private void OnEnable()
    {
        movementDistance = 0;
        StartCoroutine(DeactivateLight());

    }

 /*   private void FixedUpdate()
    {
        if (!stop)
        {
            movementDistance += movementSpeed * Time.deltaTime;
            transform.position = trainPath.path.GetPointAtDistance(movementDistance);
            Quaternion rotation = trainPath.path.GetRotationAtDistance(movementDistance);
            transform.rotation = new Quaternion(0, 0, -rotation.x, rotation.w);
            if (movementDistance > trainPath.path.length)
            {
                stop = true;
                Destroy(gameObject);
            }
        }
    }*/
    public IEnumerator MoveTrainOnPath(PathCreator trainPath)
    {
        while (!stop)
        {
            movementDistance += movementSpeed * Time.deltaTime;
            transform.position = trainPath.path.GetPointAtDistance(movementDistance);
            Quaternion rotation = trainPath.path.GetRotationAtDistance(movementDistance);
            transform.rotation = new Quaternion(0, 0, -rotation.x, rotation.w);
            if (movementDistance > trainPath.path.length)
            {
                stop = true;
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    IEnumerator DeactivateLight()
    {
        yield return new WaitForSeconds(0.3f);
        if (GameManager.Instance.currentGamePhase == GameManager.GamePhase.Night)
        {
            headLight.enabled = true;
        }
        else headLight.enabled = false;
    }

    void DestroyTrainHead()
    {
        StartCoroutine(DestroyAfterFewSeconds());
    }
    private void OnDestroy()
    {
        EventManager.wordCompleted-=DestroyTrainHead;
    }

    public void SetTrainHeadOnTrack(PathCreator path)
    {

        runningCoroutine =StartCoroutine(MoveTrainOnPath(path));
    }

    IEnumerator DestroyAfterFewSeconds()
    {
        yield return new WaitForSeconds(1);
        StopCoroutine(runningCoroutine);

        Destroy(this.gameObject);
        StopAllCoroutines();

    }
}

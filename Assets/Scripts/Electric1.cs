using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Electric1 : MonoBehaviour
{
    [SerializeField]private LineRenderer lineRenderer;
//    public Transform transformPointA;
//    public Transform transformPointB;
//    private readonly int pointsCount = 5;
    private readonly int half = 2;
    private float randomness;
    private List<Vector3> points;

    private readonly string mainTexture = "_MainTex";
    private Vector2 mainTextureScale = Vector2.one;
    private Vector2 mainTextureOffset = Vector2.one;

    [SerializeField]private float timerTimeOut = 0.05f;
    [SerializeField]List<Transform> targetTransform;

    [SerializeField] GameObject[] targetTransformsForTest;
    private void Start()
    {
        if(lineRenderer==null)lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        SetReferenceToAllTargetPoints(targetTransformsForTest);
        StartCoroutine(Electrify());
    }



    private IEnumerator CalculatePoints()
    {
        while(lineRenderer.positionCount > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime*2);
       
            float distance = Vector3.Distance(transform.position, targetTransform[0].position) / targetTransform.Count;
            mainTextureScale.x = distance;
            mainTextureOffset.x = Random.Range(-randomness, randomness);
            lineRenderer.material.SetTextureScale(mainTexture, mainTextureScale);
            lineRenderer.material.SetTextureOffset(mainTexture, mainTextureOffset);
        }
    }

    private Vector3 SetRandomness(int index)
    {
        Vector3 temp = points[index];
        temp.x += Random.Range(-randomness, randomness);
        temp.y += Random.Range(-randomness, randomness);
        temp.z += Random.Range(-randomness, randomness);
        return points[index] = temp;
    }
    Vector3 GetRandomNearPositionOf(Vector3 p_position)
    {
        return new Vector3(p_position.x+Random.Range(-randomness,randomness), p_position.y + Random.Range(-randomness, randomness), p_position.z + Random.Range(-randomness, randomness));   
    }
    private Vector3 GetCenter(Vector3 a, Vector3 b)
    {
        return (a + b) / half;
    }

    void SetLinePoints(Vector2 sorcePosition,Vector2[] linePointPositions)
    {
        points.Add(sorcePosition);
        points.Add(GetCenter(sorcePosition, linePointPositions[0]));
        foreach(Vector3 point in linePointPositions)points.Add(point);

    }void SetReferenceToAllTargetPoints(GameObject[] p_gameObjects)
    {
        ///<summary>
        ///
        /// Reference of origin transform setted
        /// Reference of all transforms of targeted gameObjects
        /// 
        /// </summary>

        targetTransform = new List<Transform>(p_gameObjects.Length+1);
        foreach (GameObject l_gameObj in p_gameObjects) targetTransform.Add(l_gameObj.transform);
    }

    IEnumerator Electrify()
    {
        int pointsReached = 0;
        Vector3[] l_targetPositions = new Vector3[(targetTransform.Count *2) +1];
        l_targetPositions[pointsReached] = transform.position;
        l_targetPositions[pointsReached + 1] = transform.position + new Vector3(0,0,4);
        lineRenderer.enabled = true;
        lineRenderer.positionCount = targetTransform.Count;
        lineRenderer.SetPositions(l_targetPositions);
        StartCoroutine(CalculatePoints());

        yield return new WaitForSeconds(timerTimeOut);
        l_targetPositions[pointsReached + 1] = GetCenter(l_targetPositions[pointsReached], targetTransform[pointsReached].position);
        yield return new WaitForSeconds(timerTimeOut/2);
        pointsReached++;
        do
        {
            l_targetPositions[0] = transform.position;
            l_targetPositions[pointsReached + 1] = targetTransform[pointsReached].position;
            pointsReached++;
            lineRenderer.SetPositions(l_targetPositions);
            print(pointsReached);
            yield return new WaitForSeconds(timerTimeOut);
        } while (pointsReached < targetTransform.Count);
        yield return new WaitForSeconds(0.75f);
        StopCoroutine(CalculatePoints());
        lineRenderer.enabled = false;
        print("Erased");
        EventManager.EraseAllLetters?.Invoke();
    }
}

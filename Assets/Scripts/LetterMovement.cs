using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using TMPro;

public class LetterMovement : MonoBehaviour
{
    PathCreator pathCreator;
    public float movementSpeed;
    float movementDistance = 0;
    [SerializeField] RectTransform TextRect;
    Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }
    void OnEnable()
    {
        movementDistance = 0;
        StartCoroutine(MoveAlongPath());
    }

    private IEnumerator MoveAlongPath()
    {
        while (gameObject.activeInHierarchy)
        {
            movementDistance += movementSpeed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(movementDistance);
            Quaternion rotation = pathCreator.path.GetRotationAtDistance(movementDistance);
            transform.rotation = new Quaternion(0, 0, rotation.x, rotation.w);

            TextRect.LookAt( TextRect.position + cameraTransform.forward);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void SetPathReference(PathCreator path)
    {
        pathCreator= path;
    }
}

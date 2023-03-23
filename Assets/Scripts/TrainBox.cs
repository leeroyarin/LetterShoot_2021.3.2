using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBox : MonoBehaviour
{
    PathCreator trainPath;
    public LetterBehaviour letterContainier;
    float movementDistance = 0;
    Coroutine currentCoroutine;

    [SerializeField] float movementSpeed;

    void OnEnable()
    {
        movementDistance = 0;
        currentCoroutine = StartCoroutine(MoveAlongPath());
    }

    public void SetPathReference(PathCreator path) => trainPath = path;

    public IEnumerator MoveAlongPath()
    {
        yield return new WaitWhile(() => (trainPath == null));
        while (gameObject.activeInHierarchy)
        {
            ///<summary> 
            ///
            /// Train Box Movement
            ///
            /// sets position of train box acccording to the float moveDistance of the path
            /// and also sets rotation according to the track
            /// 
            /// -----------------------------------
            /// 
            ///</summary>
            movementDistance += movementSpeed * Time.deltaTime;
            transform.position = trainPath.path.GetPointAtDistance(movementDistance);
            Quaternion rotation = trainPath.path.GetRotationAtDistance(movementDistance);
            transform.rotation = new Quaternion(0, 0, -rotation.x, rotation.w);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void OnDisable() => StopCoroutine(currentCoroutine);
}

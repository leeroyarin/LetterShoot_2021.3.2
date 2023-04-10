using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBox : MonoBehaviour
{
    PathCreator trainPath;
    public LetterBehaviour letterContainier;
    float movementDistance = 0;

    [SerializeField] float movementSpeed;

    void OnEnable()
    {
        movementDistance = 0;
        //currentCoroutine = StartCoroutine(MoveAlongPath());
    }

    public void SetPathReference(PathCreator path) => trainPath = path;


    private void FixedUpdate()
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

        if (trainPath == null) return;
        movementDistance += movementSpeed * Time.deltaTime;
        transform.position = trainPath.path.GetPointAtDistance(movementDistance);
        Quaternion rotation = trainPath.path.GetRotationAtDistance(movementDistance);
        transform.rotation = new Quaternion(0, 0, -rotation.x, rotation.w);
        if (movementDistance > trainPath.path.length)
        {
            transform.localPosition = Vector3.zero;
            if (!letterContainier.gameObject.activeInHierarchy) letterContainier.gameObject.SetActive(true);
            LetterManager.LetterManagerInstance.AddTrainBoxToTheList(this.gameObject, letterContainier);
        }
    }

}

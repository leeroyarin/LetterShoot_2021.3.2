using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterChangingVisualizer : MonoBehaviour
{
    RectTransform position;
    Vector3 _destinationPosition;
    GameObject _gameObjectToActive;
    LetterHolder letterHolder;
    public void MovementAction(Vector2 originPosition, Vector2 targetPosition,GameObject gameObject)
    {
        this.gameObject.SetActive(true);
        _destinationPosition = targetPosition;
        position.position = originPosition;
       _gameObjectToActive = gameObject;
        _gameObjectToActive.SetActive(false);
        StartCoroutine(MoveTowardsDestination());
    }

    IEnumerator MoveTowardsDestination()
    {
        while(Vector2.Distance(position.position,_destinationPosition) < 0.5)
        {
            Vector3 moveDirection = _destinationPosition - position.position;
            position.position += moveDirection.normalized;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        letterHolder.DestinationReached(gameObject);

    }
}

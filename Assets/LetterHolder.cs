using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterHolder : MonoBehaviour
{
    LetterChangingVisualizer letterVisualizer;
    List<GameObject> list = new List<GameObject>();
    GameObject letterUi;
    Canvas canvas;
    public void AddLetters(char letter, Vector2 screenPosition)
    {
        GameObject letterObject = Instantiate(letterUi, this.transform);
        Vector3 letterPosition = letterUi.GetComponent<RectTransform>().transform.position;
        letterObject.SetActive(false);
        letterVisualizer.MovementAction(screenPosition,letterPosition, letterObject);
    }

    public void DestinationReached(GameObject letterObject)
    {
        letterVisualizer.gameObject.SetActive(false);
        letterObject.SetActive(true);

    }
}

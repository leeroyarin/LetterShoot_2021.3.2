using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using TMPro;
using UnityEngine.UIElements;

public class LetterMovement : MonoBehaviour
{
    PathCreator pathCreator;
    public float movementSpeed;
    float movementDistance = 0;
    [SerializeField] RectTransform TextRect;
    Transform cameraTransform;
    Coroutine currentCoroutine;
    Transform m_letterHolderPosition;
    UILetters uiLetter;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }
    void OnEnable()
    {
        movementDistance = 0;
        currentCoroutine = StartCoroutine(MoveAlongPath());
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

    public void GetTheLetterToLetterHolder(UILetters letter)
    {
        if (letter == null)
        {
            print("Null Exception");
            return;
        }

        StopCoroutine(currentCoroutine);
        letter.gameObject.SetActive(true);
        letter.OnLetterLoad();
        m_letterHolderPosition = letter.rectTransform.transform;
        uiLetter = letter;
        letter.activated = true;
        currentCoroutine = StartCoroutine(MoveTowardsLetterUI());
    }

    IEnumerator MoveTowardsLetterUI()
    {
       
        while (Vector2.Distance(m_letterHolderPosition.position, transform.position) >= 0.2)
        {
            transform.localRotation.SetLookRotation(m_letterHolderPosition.position);
            transform.position += (m_letterHolderPosition.position-transform.position).normalized* Time.deltaTime * movementSpeed*25;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        gameObject.SetActive(false);
        uiLetter.OnLetterReached();
        StopCoroutine(currentCoroutine);
    }

    private void OnDisable()
    {
        StopCoroutine(currentCoroutine);
    }
}

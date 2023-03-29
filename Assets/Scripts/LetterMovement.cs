using System.Collections;
using UnityEngine;
using PathCreation;

public class LetterMovement : MonoBehaviour
{
    public float movementSpeed;
    [SerializeField] RectTransform TextRect;
    Transform cameraTransform;
    Coroutine currentCoroutine;
    Transform hookTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }
    void Update()
    {
        TextRect.LookAt(TextRect.position + cameraTransform.forward);
    }


    /*  public IEnumerator MoveAlongTrainBox()
      {
          //to let buffer
          yield return new WaitWhile(() => (targetTransform == null));

          while (gameObject.activeInHierarchy)
          {
              transform.position = targetTransform.position;
              transform.rotation = targetTransform.rotation;

              TextRect.LookAt( TextRect.position + cameraTransform.forward);
              yield return new WaitForSeconds(Time.deltaTime);
          }
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
          hookTransform = letter.rectTransform.transform;
          letter.activated = true;
          currentCoroutine = StartCoroutine(MoveAlongHook());
      }
    */
    public IEnumerator MoveAlongHook()
    {
       
        while (Vector2.Distance(hookTransform.position, transform.position) >= 2)
        {
            transform.localRotation.SetLookRotation(hookTransform.position);
            Vector3 moveDirection = hookTransform.position;
            moveDirection.z = 0;
            transform.position += (moveDirection - transform.position).normalized* Time.deltaTime * movementSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        print("das");
        StopCoroutine(currentCoroutine);
    }

    private void OnDisable()
    {
        transform.localPosition = Vector3.zero;
        if(currentCoroutine != null) StopCoroutine(currentCoroutine);

    }

    public void GetLetterContainerMoveAlongHook(UILetters letter)
    {
        if (letter == null)
        {
            print("Null Exception");
            return;
        }

        StopCoroutine(currentCoroutine);
        letter.activated = true;
        currentCoroutine = StartCoroutine(MoveAlongHook());
    }
}

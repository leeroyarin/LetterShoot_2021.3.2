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
        StopCoroutine(currentCoroutine);
    }

    private void OnDisable()
    {
        transform.localPosition = Vector3.zero;
        if(currentCoroutine != null) StopCoroutine(currentCoroutine);

    }
}

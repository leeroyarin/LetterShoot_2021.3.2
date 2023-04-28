using System.Collections;
using UnityEngine;
using PathCreation;

public class LetterMovement : MonoBehaviour
{
    public float movementSpeed;
    [SerializeField] RectTransform TextRect;

    Transform _cameraTransform;
    Coroutine _currentCoroutine;
    Transform _hookTransform;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }
    void Update()
    {
        TextRect.LookAt(TextRect.position + _cameraTransform.forward);
    }

    /// <summary>
    /// Coroutine to move the letter towards the hook
    /// </summary>
    public IEnumerator MoveAlongHook()
    {
       
        while (Vector2.Distance(_hookTransform.position, transform.position) >= 2)
        {
            transform.localRotation.SetLookRotation(_hookTransform.position);
            Vector3 moveDirection = _hookTransform.position;
            moveDirection.z = 0;
            transform.position += (moveDirection - transform.position).normalized* Time.deltaTime * movementSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        StopCoroutine(_currentCoroutine);
    }

    private void OnDisable()
    {
        transform.localPosition = Vector3.zero;
        if(_currentCoroutine != null) StopCoroutine(_currentCoroutine);

    }
}

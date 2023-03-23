using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HookBehaviour : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform _hookSpawnPointTransform;
    [SerializeField] Transform _cannonTopTransform;
    [SerializeField] Rigidbody2D _hookRigidBody;
    [SerializeField] float _revertingSpeed = 20f;
    [SerializeField] BoxCollider2D hookCollider;

    LetterBehaviour letterToGrab;
    Vector2 _launchDirection;
    [SerializeField]Enum_HookStates _hookStates = Enum_HookStates.resting;
    enum Enum_HookStates
    {
        resting,
        lauching,
        reverting
    }
    private void Start()
    {
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        switch (_hookStates)
        {
            case Enum_HookStates.resting:                
                break;

            case Enum_HookStates.lauching:
                float distance = (_hookSpawnPointTransform.position - this.transform.position).magnitude;
                if (distance > 13)
                {
                    StopAllCoroutines();
                    RevertHook();
                }
                lineRenderer.SetPosition(0,this.transform.position);
                lineRenderer.SetPosition(1,_hookSpawnPointTransform.position);
                break;
            case Enum_HookStates.reverting:                
                transform.position += (_hookSpawnPointTransform.position - this.transform.position).normalized *_revertingSpeed* Time.deltaTime;
                if(Vector2.Distance(_hookSpawnPointTransform.position,this.transform.position)<0.3f)
                {
                    //to check if the letter is the correct ones or not
                    RestHook();

                    if (letterToGrab != null)
                    {

                        letterToGrab.CheckTheContainer();
                        letterToGrab = null;
                    }
                    return;
                }
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, _hookSpawnPointTransform.position);
                break;
        }
    }

    private void RestHook()
    {
        this.transform.position = _hookSpawnPointTransform.position;
        _hookStates = Enum_HookStates.resting;
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 0;
        _hookRigidBody.angularVelocity = 0;
        _hookRigidBody.inertia = 0;
        _hookRigidBody.velocity = Vector2.zero;
    }

    public void LaunchHook(float p_firingPower)
    {
        this.transform.position = _hookSpawnPointTransform.transform.position;
        _launchDirection = _hookSpawnPointTransform.right;
        this._hookRigidBody.velocity = _launchDirection * p_firingPower;
        hookCollider.enabled = true;
        _hookStates = Enum_HookStates.lauching;
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;

        StartCoroutine(RevertHookAfterSeconds(1.5f));
    }

    IEnumerator RevertHookAfterSeconds(float timer)
    {
        yield return new WaitForSeconds(timer);
        RevertHook();

    }

    private void RevertHook()
    {
        _launchDirection *= -1;
        _hookStates = Enum_HookStates.reverting;
        _hookRigidBody.angularVelocity = 0;
        _hookRigidBody.inertia = 0;
        _hookRigidBody.velocity = Vector2.zero;
        hookCollider.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_hookStates == Enum_HookStates.lauching)
        {
            StopAllCoroutines();
            RevertHook();
            return;
        }
        //if the hook gets collided with the shooter then the hook stops moving 
        if (collision.gameObject.CompareTag("Shooter")&& _hookStates == Enum_HookStates.reverting)
        {
            _hookStates = Enum_HookStates.resting;
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
            return;
        }
    }

    public bool CheckIfHookIsLaunched()=> _hookStates == Enum_HookStates.lauching;


    public bool CheckIfHookIsResting() => _hookStates == Enum_HookStates.resting;

    public void SetLetterToHook(LetterBehaviour letter) => letterToGrab = letter;
}

using System.Collections;
using UnityEngine;

public partial class HookBehaviour : MonoBehaviour
{
    #region Reference
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform _hookSpawnPointTransform;
    [SerializeField] Transform _cannonTopTransform;
    [SerializeField] Rigidbody2D _hookRigidBody;
    [SerializeField] BoxCollider2D hookCollider;
    [SerializeField] Enum_HookStates _hookStates = Enum_HookStates.Resting;
    AudioManager audioManager;
    LetterBehaviour letterToGrab;
    #endregion
    [Space(20)]
    
    [SerializeField] float _revertingSpeed = 20f;
    Vector2 _launchDirection;
    enum Enum_HookStates
    {
        Resting,
        Launching,
        Reverting
    }

    private void Start()
    {
 //       Application.targetFrameRate = 7;

        //Sets Reference of the AudioManager
        audioManager = AudioManager.Instance;
        SetLineRendererValues();

        void SetLineRendererValues()
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
        }





    }

    private void Update()
    {
        switch (_hookStates)
        {
            case Enum_HookStates.Resting:
                break;
            case Enum_HookStates.Launching:
                LaunchAction();
                break;
            case Enum_HookStates.Reverting:
                RevertAction();
                break;
        }

        void LaunchAction()
        {

            //Measures Distance
            float distance = (_hookSpawnPointTransform.position - this.transform.position).magnitude;

            //Checks if the distance exceeds more than 13
            if (distance > 13)
            {
                //Stops allcorroutine in the script and Reverts the hook
                StopAllCoroutines();
                RevertHook();
            }

            //Sets the lineRenderer's Firstpoint according to its currentPosition
            lineRenderer.SetPosition(0, this.transform.position);

            //Sets the lineRenderer's LastPoint according to its hookSpawnPoint
            lineRenderer.SetPosition(1, _hookSpawnPointTransform.position);
        }

        void RevertAction()
        {
            //Reverts the hook at constant speed according to the hook spawn point position
            transform.position += (_hookSpawnPointTransform.position - this.transform.position).normalized * _revertingSpeed * Time.fixedDeltaTime;

            //if the letter to grab i.w. LetterBox is not null then the letterBox's position is set according to the hook's position
            if (letterToGrab != null) letterToGrab.transform.position = transform.position;

            //if the hook gets close to the hook 
            if (Vector2.Distance(_hookSpawnPointTransform.position, this.transform.position) < 1f)
            {
                //Action
                RestHook();

                //Empties the reference of Letter To Grab
                CheckTheContainerIfGrabbed();
                return;
            }

            //LineRenderer
            lineRenderer.SetPosition(0, this.transform.position);
            lineRenderer.SetPosition(1, _hookSpawnPointTransform.position);
        }
    }

    public void CheckTheContainerIfGrabbed()
    {
        if (letterToGrab != null)
        {
            letterToGrab.CheckTheContainer();
            letterToGrab = null;
        }
    }

    /// <summary>
    /// reverts the position of the hook according to hookSpawnPoint position
    /// Changes the state of the hook to resting statef
    /// lineRenderer gets disabled and its point count is setted to zero
    /// Rests the hook by nullifying all physics elements
    /// Plpays Hook RestSound
    /// </summary>
    public void RestHook()
    {
        //position
        this.transform.position = _hookSpawnPointTransform.position;

        //State
        _hookStates = Enum_HookStates.Resting;

        //lineRenderer
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 0;

        //physics
        _hookRigidBody.angularVelocity = 0;
        _hookRigidBody.inertia = 0;
        _hookRigidBody.velocity = Vector2.zero;

        //Sound
        audioManager.PlaySound(SoundNames.HookHalt);

    }

    /// <summary>
    /// Initially sets the hook position at hookSpawnPoint position
    /// LaunchDirection gets set according to the hookSpawnPoint's directioin
    /// sets velocity of the hook
    /// collider gets enabled
    /// hoook state gets changed to launching state
    /// linerenderer gets activated
    /// lineRenderer's points count gets setted to 2
    /// And starts coroutine RevertHookAfterSeconds to revert  the hook on time
    /// </summary>
    /// <param name="p_firingPower"> Force to apply on the Hook to launch</param>
    public void LaunchHook(float p_firingPower)
    {
        if (this.enabled == false) return;
        //Postion
        this.transform.position = _hookSpawnPointTransform.transform.position;

        //Direction
        _launchDirection = _hookSpawnPointTransform.right;

        //physics
        this._hookRigidBody.velocity = _launchDirection * p_firingPower;

        //collider
        hookCollider.enabled = true;

        //State
        _hookStates = Enum_HookStates.Launching;

        //lineRenderer
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;

        //Sound
        audioManager.PlaySound(SoundNames.HookLaunch);

        StartCoroutine(RevertHookAfterSeconds(1.5f));
    }


    /// <summary> Initializes reverting of the hook back to the Harpoon 
    /// 
    /// lauch direction is reverted
    /// the hook state is changed to reverting state
    /// Removal of all the physics inertia, velocity to reduce thte chances of conflict in reverting the hook
    /// Collider also get disabled so that even if the hook gets by other letterBox it wont collide with it
    /// and Hook revert sound is played
    /// 
    /// </summary>
    /// 
    private void RevertHook()
    {
        //Direction
        _launchDirection *= -1;
        
        //State
        _hookStates = Enum_HookStates.Reverting;

        //Physics
        _hookRigidBody.angularVelocity = 0;
        _hookRigidBody.inertia = 0;
        _hookRigidBody.velocity = Vector2.zero;

        //Collider
        hookCollider.enabled = false;

        //Sound
        audioManager.PlaySound(SoundNames.HookRevert, 0.2f);

    }


    /// <summary>
    /// This function is used to limit the hook launching distance based on time
    /// After the time stated the hook gets reverted
    /// </summary>
    /// <param name="timer"></param>
    /// <returns>Nothing</returns>
    IEnumerator RevertHookAfterSeconds(float timer)
    {
        yield return new WaitForSeconds(timer);
        RevertHook();
    }



    ///<summary> 
    ///
    ///Checks if the hook is in launching state
    /// <param name="collision">Collided Object</param> 
    ///the checks if the collided object is LetterBox
    ///then gets the letterbox attacked to the hook
    ///then all the coroutine gets stopped and reverting starts
    ///Hook Collide sound plays
    ///</summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (_hookStates == Enum_HookStates.Launching)
        {
            //if the collided object is letterBox
            if (collision.gameObject.CompareTag("Letter"))
            {
                collision.gameObject.GetComponent<IOnCollisionWithHook>().GetHooked(this);
            }

            //Sound
            audioManager.PlaySound(SoundNames.HookCollide);

            //Action
            StopAllCoroutines();
            RevertHook();
            return;
        }        
    }

    /// <returns>if the hook state is in lauching state</returns>
    public bool CheckIfHookIsLaunched()=> _hookStates == Enum_HookStates.Launching;

    /// <returns>if the hook state is in resting state</returns>
    public bool CheckIfHookIsResting() => _hookStates == Enum_HookStates.Resting;

    /// <summary>
    /// Sets letter to hook according to the parameter so that it keeps on reference of the LetterBox it collides with and grabs along it
    /// </summary>
    /// <param name="letter">LetterBehaviour</param>
    public void SetLetterToHook(LetterBehaviour letter) => letterToGrab = letter;
}

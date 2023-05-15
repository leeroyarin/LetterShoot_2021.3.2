using System.Collections;
using UnityEngine;

/// <summary>
/// This class controls the Shooter object and implements the IInteractableShooter interface.
/// </summary>
public class Shooter : MonoBehaviour,IInteractableShooter
{
    //Public variables
    public GameObject hookGameObject;
    public float firingPower = 20f;

    //Serialized Fields
    [SerializeField] HookBehaviour hook;
    [SerializeField] Transform harpoonShooterBase;
    [SerializeField] GameObject harpoonLight;
    [SerializeField] float repairTime;

    //Private Variables
    bool _IsActivated;

    public Animator harpoonMuzzle;
    private void Awake()
    {
        // Subscribe to CorrectLetterHit event
        EventManager.CorrectLetterHit += LetterRecieved; 
    }

    #region InterfaceFunction
    /// <summary>
    /// Fires the hook if the Shooter is activated.
    /// </summary>
    public void Fire()
    {
        if (!_IsActivated) return;
        LaunchHook();
    }

    /// <summary>
    /// Rotates the Harpoon shooter's base to look at the target position.
    /// </summary>
    /// <param name="p_targetPosition">The position to look at.</param>
    public void LookAtPosition(Vector2 p_targetPosition)
    {
        if (!hook.CheckIfHookIsResting()||!_IsActivated) return;
        Vector3 l_aimDirection = (p_targetPosition - new Vector2(transform.position.x, transform.position.y));
        float l_angle = Mathf.Atan2(l_aimDirection.y, l_aimDirection.x) * Mathf.Rad2Deg;
        harpoonShooterBase.eulerAngles = new Vector3(0, 0, l_angle);
    }

    /// <summary>
    /// Enables or disables the Harpoon light based on the current game phase and the enable parameter.
    /// </summary>
    /// <param name="p_enable">Whether to enable or disable the Harpoon light.</param>
    public void EnableHarpoonLights(bool p_enable)
    {
        _IsActivated = p_enable;
        if (GameSceneManager.SceneManagerInstance.GetCurrentSceneName() == "StartMenu") return;
        if (GameManager.Instance.currentGamePhase == GameManager.GamePhase.Night)
        {
            harpoonLight.SetActive(p_enable);
        }
    }

    /// <summary>
    /// Enables or disables the cannon light based on the current game phase and the enable parameter.
    /// </summary>
    /// <param name="enable">Whether to enable or disable the cannon light.</param>
    public void EnableLightAfterSecondsCoroutine(float time, bool enable)
    {
        StartCoroutine(EnableLightAfterSeconds(time));
        IEnumerator EnableLightAfterSeconds(float time)
        {
            yield return new WaitForSeconds(time);
            EnableHarpoonLights(enable);
        }
    }
    #endregion

    /// <summary>
    /// Launches the hook if it is not already launched.
    /// </summary>
    private void LaunchHook()
    {
        //checks if the bullet has already coroutine working.
        //if yes the function is stopped
        if (!hook.CheckIfHookIsResting()) return;
        hook.LaunchHook(firingPower);
    }

    /// <summary>
    /// Disables the harpoon for a set amount of time.
    /// </summary>
    public void DisableHarpoonForWhile()
    {
        StartCoroutine(DisableHarpoon());
    }

    /// <summary>
    /// Coroutine that disables the harpoon for a set amount of time.
    /// </summary>
    IEnumerator DisableHarpoon()
    {
        harpoonMuzzle.Play("DamagedAnimation");
        hook.CheckTheContainerIfGrabbed();
        hook.RestHook();
        EnableHarpoonLights(false);
        yield return new WaitForSeconds(repairTime);
        EnableHarpoonLights(true);
        harpoonMuzzle.Play("FineAnimation");

    }

    /// <summary>
    /// Called when a correct or incorrect letter is hit.
    /// If the harpoon is not activated, the function returns.
    /// If the letter is incorrect, the harpoon is disabled for a set amount of time.
    /// </summary>
    /// <param name="p_IsCorrect">Whether the letter hit was correct or not.</param>
    void LetterRecieved(bool p_IsCorrect)
    {
        if (!_IsActivated) return;
        if (!p_IsCorrect)
        {
            DisableHarpoonForWhile();
        }
    }


    /// <summary>
    /// Declares shooter to be activated
    /// </summary>
    /// <param name="p_enable">decides if the shooter should be activated or not</param>
    public void EnableAction(bool p_enable) => _IsActivated = p_enable;

    void OnDestroy()
    {
        // Unsubscribe from CorrectLetterHit event
        EventManager.CorrectLetterHit -= LetterRecieved;
    } 
}

/*
Public Variables
GameObject _hookGameObject: A reference to the hook game object.
Transform hookLaunchPoint: The position where the hook will be launched from.
float firingPower: The force with which the hook is fired.

Serialized Fields
HookBehaviour hook: A reference to the HookBehaviour component attached to the hook.
Transform harpoonShooterBase: The base of the shooter.
GameObject harpoonLight: The light that is turned on when the shooter is activated.
float repairTime: The amount of time it takes for the shooter to be repaired.

Private Variable
bool _IsActivated: A flag indicating whether the shooter is currently activated or not.

Private Methods
void LaunchHook(): Launches the hook with the specified firing power.
IEnumerator DisableHarpoon(): Disables the harpoon for a certain amount of time.
Interface Implementations
void Fire(): Fires the hook if the shooter is activated.
void LookAtPosition(Vector2 targetPosition): Rotates the shooter to aim at a specified position if the hook is currently resting.
void EnableCannonLights(bool enable): Enables or disables the cannon light depending on the current game phase and whether the shooter is activated.
void EnableAction(bool enable): Enables or disables the shooter.

Event Handlers
void LetterRecieved(bool IsCorrect): Disables the harpoon for a certain amount of time if the letter received is incorrect.

Public Methods
void EnableLightAfterSecondsCoroutine(float time,bool enable): Enables or disables the cannon light after a certain amount of time.

Unity Events
void Awake(): Subscribes to the CorrectLetterHit event.
void OnDestroy(): Unsubscribes from the CorrectLetterHit event.
*/

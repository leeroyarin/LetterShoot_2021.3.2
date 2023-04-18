using UnityEngine;

/// <summary>
/// The 'InputManager' class manages the player input for the harpoon shooter game. It is responsible for handling the different input types for mobile and computer devices and changing the active harpoon shooter.
/// </summary>
public class InputManager : MonoBehaviour
{   
    /// <summary>
    /// The InputManager class is a MonoBehaviour that has a static instance for easy access throughout the game.
    /// It also has a private variable _currentInteractableShooter that references the currently active harpoon shooter.
    /// The state of the input type is stored in _currentInputType, which can either be MobileInput or ComputerInput. 
    /// The hasActiveHarpoonShooter variable is used to check if there is an active harpoon shooter, and the harpoonLayer is used to specify which layer the harpoons are on.
    /// </summary>
    //Static Instance of Input Manager
    private static InputManager _inputManager;
    public static InputManager InputManagerInstance
    {
        get
        {
            if (_inputManager == null)
            {
                _inputManager = FindObjectOfType<InputManager>();
            }
            return _inputManager;
        }
    }

    //Interface of Interactable shooter to get access to the selected shooter
    IInteractableShooter _currentInteractableShooter;

    //State refernce
    InputType _currentInputType;

    //options
    MobileInput _mobileInput = new MobileInput();
    ComputerInput _computerInput = new ComputerInput();


    public bool hasActiveHarpoonShooter = false;
    public LayerMask harpoonLayer;



    /// <summary>
    /// In the Awake function, the instance of the InputManager is set to the current instance. 
    /// If there is no active harpoon shooter, the hasActiveHarpoonShooter variable is set to false.
    /// The input type is set based on whether the game is being played on a mobile device or computer.
    /// </summary>
    private void Awake()
    {
        _inputManager = this;


        if (_currentInteractableShooter == null) hasActiveHarpoonShooter = false;
        if (SettingsData.IsMobileDevice) _currentInputType = _mobileInput;
        else _currentInputType = _computerInput;

    }

    private void Start()
    {
        //subscribing
        EventManager.GameCompleted += DisableInputs;
    }

    /// <summary>
    /// In the Update function, the _currentInputType is used to check for player input. 
    /// If there is an active harpoon shooter, the player input is handled based on the device type.
    /// </summary>
    void Update()
    {
        /*
         * Checks if the Interface has reference or not
         * Then input action is done
         */
        hasActiveHarpoonShooter = (_currentInteractableShooter == null)? false:true;
        _currentInputType.InputAction(this);
    }

    private void OnDestroy()
    {
        //unsubscribe
        EventManager.GameCompleted -= DisableInputs;
    }

    /// <summary>
    /// The DisableInputs function disables the InputManager when the game is completed.
    /// </summary>
    /// <param name="p_complete">represents if the game is complete or not, BUT IS USELESS !!!!!  </param>
    public void DisableInputs(bool p_complete)
    {
        this.enabled = false;
    }

    /// <summary>
    /// This function is called when a player interacts with a harpoon shooter object. 
    /// This function first disables the harpoon lights on the current interactable shooter by calling EnableHarpoonLights(false) on the _currentInteractableShooter. 
    /// Then it updates the _currentInteractableShooter variable to the new shooter object by getting the IInteractableShooter component from the p_shooterCollider parameter. 
    /// Finally, it enables the harpoon lights on the new shooter object by calling EnableHarpoonLights(true) on the _currentInteractableShooter.
    /// </summary>
    /// <param name="p_shooterCollider">It takes a Collider2D parameter that represents the harpoon shooter object that the player is interacting with. </param>
    public void ChangeCurrentlyActiveHarpoonShooter(Collider2D p_shooterCollider)
    {
        _currentInteractableShooter?.EnableHarpoonLights(false);
        //gets the interface that is interactable to fire, look at
        _currentInteractableShooter = p_shooterCollider.GetComponent<IInteractableShooter>();
        _currentInteractableShooter.EnableHarpoonLights(true);

    }

    /// <summary>
    /// This function is called to make the current harpoon shooter look towards the target position. 
    /// This function calls the LookAtPosition method on the _currentInteractableShooter and passes p_targetPosition as the parameter.
    /// </summary>
    /// <param name="p_targetPosition">It takes a Vector2 parameter p_targetPosition that represents the position that the shooter should look at.</param>
    public void GetHarpoonShooterToLookAt(Vector2 p_targetPosition)
    {
        //makes the currentInteractableShooter to look towards the targetPosition received as parameter
        _currentInteractableShooter.LookAtPosition(p_targetPosition);
    }

    /// <summary>
    /// This function is called to make the current harpoon shooter fire a hook. This function calls the Fire method on the _currentInteractableShooter.
    /// </summary>
    public void GetHarpoonShooterToLaunchHook()
    {
        //makes the currentINteractableShooter to fire at the diraction the shooter is currently aiming at
        _currentInteractableShooter.Fire();
    }

    /// <summary>
    /// This function is called to activate the lights on the current harpoon shooter after a delay of 2 seconds. 
    /// It calls the EnableLightAfterSecondsCoroutine method on the _currentInteractableShooter and passes 2 and true as the parameters.
    /// </summary>
    public void ActivateLights() => _currentInteractableShooter?.EnableLightAfterSecondsCoroutine(2, true);

    /// <summary>
    /// This function is called to enable or disable the current harpoon shooter. 
    /// This function calls the EnableAction method on the _currentInteractableShooter and passes p_enable as the parameter. 
    /// If _currentInteractableShooter is null, then this method does nothing.
    /// </summary>
    /// <param name="p_enable">It takes a boolean parameter p_enable which represents whether to enable or disable the shooter.</param>
    public void EnableCurrentShooter(bool p_enable) => _currentInteractableShooter?.EnableAction(p_enable);

}

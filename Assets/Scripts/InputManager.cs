using UnityEngine;

public class InputManager : MonoBehaviour
{
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
    IInteractableShooter currentInteractableShooter;
    InputType currentInputType;
    MobileInput mobileInput = new MobileInput();
    ComputerInput computerInput = new ComputerInput();
    public bool HasShooter = false;
    private void Awake()
    {
        _inputManager = this;


        if (currentInteractableShooter == null) HasShooter = false;
        if (SettingsData.IsMobileDevice) currentInputType = mobileInput;
        else currentInputType = computerInput;

    }
    private void Start()
    {
        EventManager.GameCompleted += DisableInputs;
    }
    void Update()
    {
        /*
         * Checks if the Interface has reference or not
         * Then input action is done
         */
        HasShooter = (currentInteractableShooter == null)? false:true;
        currentInputType.InputAction(this);
    }
    private void OnDestroy()
    {
        EventManager.GameCompleted -= DisableInputs;
    }
    public void DisableInputs(bool complete)
    {
        this.enabled = false;
    }
    public void ChangeShooter(Collider2D shooterCollider)
    {
        currentInteractableShooter?.EnableCannonLights(false);
        //gets the interface that is interactable to fire, look at
        currentInteractableShooter = shooterCollider.GetComponent<IInteractableShooter>();
        currentInteractableShooter.EnableCannonLights(true);

    }
    public void ChangeShooter(Shooter shooter)
    {
        currentInteractableShooter?.EnableCannonLights(false);
        //gets the interface that is interactable to fire, look at
        currentInteractableShooter = shooter.GetComponent<IInteractableShooter>();
        currentInteractableShooter.EnableCannonLights(true);

    }

    public void GetShooterToLookAt(Vector2 targetPosition)
    {
        //makes the currentInteractableShooter to look towards the targetPosition received as parameter
        currentInteractableShooter.LookAtPosition(targetPosition);
    }

    public void GetShooterToFire()
    {
        //makes the currentINteractableShooter to fire at the diraction the shooter is currently aiming at
        currentInteractableShooter.Fire();
    }

    public void SetReferences()
    {
        //helps to set reference of interface IInteractableShooter of any shooter 
        currentInteractableShooter = FindObjectOfType<Shooter>().GetComponent<IInteractableShooter>();
    }

    public void ActivateLights()
    {
        print("CAllED");
        currentInteractableShooter?.EnableLightAfterSecondsCoroutine(2,true);
    }


    public void EnableCurrentShooter(bool enable)
    {
        currentInteractableShooter?.EnableAction(enable);
    }
}

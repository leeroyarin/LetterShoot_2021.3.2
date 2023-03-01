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
                if( _inputManager == null)
                {
                    //create new gameobject in the scene then add input manager component
                    GameObject gameObject = Instantiate(new GameObject());
                    _inputManager = gameObject.AddComponent<InputManager>();
                    _inputManager.SetReferences();
                }
            }
            return _inputManager;
        }
    }

    //Interface of Interactable shooter to get access to the selected shooter
    IInteractableShooter currentInteractableShooter;
    InputType currentInputType;
    MobileInput mobileInput = new MobileInput();
    ComputerInput computerInput = new ComputerInput();
    public bool HasShooter;
    private void Awake()
    {

        if (SettingsData.IsMobileDevice) currentInputType = mobileInput;
        else currentInputType = computerInput;
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

    public void ChangeShooter(Collider2D shooterCollider)
    {
        currentInteractableShooter?.EnableCannonLights(false);
        //gets the interface that is interactable to fire, look at
        currentInteractableShooter = shooterCollider.GetComponent<IInteractableShooter>();
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
}

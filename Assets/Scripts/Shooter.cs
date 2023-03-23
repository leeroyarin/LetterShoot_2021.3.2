using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour,IInteractableShooter
{
    public GameObject _hookGameObject;
    public Transform bulletSpawnPoint;
    public int bulletCapacity = 10;
    public float firingPower = 20f;
    [SerializeField]HookBehaviour hook;
    [SerializeField] Transform shooterBase;
    [SerializeField] GameObject cannonLight;

    private void LaunchHook()
    {
        //checks if the bullet has already coroutine working.
        //if yes the function is stopped

        
        if (hook.CheckIfHookIsLaunched()) return;

        hook.LaunchHook(firingPower);
    }

    #region InterfaceFunction
    public void Fire()
    {
        LaunchHook();
    }

    public void LookAtPosition(Vector2 targetPosition)
    {
        if (!hook.CheckIfHookIsResting()) return;
        Vector3 aimDirection = (targetPosition - new Vector2(transform.position.x, transform.position.y));

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        shooterBase.eulerAngles = new Vector3(0, 0, angle);
    }

    public void EnableCannonLights(bool enable)
    {
        cannonLight.SetActive(enable);
    }

    #endregion
}

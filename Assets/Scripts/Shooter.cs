using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class Shooter : MonoBehaviour,IInteractableShooter
{
    public GameObject _hookGameObject;
    public Transform bulletSpawnPoint;
    public int bulletCapacity = 10;
    public float firingPower = 20f;
    [SerializeField]HookBehaviour hook;
    [SerializeField] Transform shooterBase;
    [SerializeField] GameObject cannonLight;
    [SerializeField] float repairTime;
    [SerializeField] bool activated;

    private void Awake()
    {
        EventManager.CorrectLetterHit += LetterRecieved; 

    }
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
        if (!activated) return;
        LaunchHook();
    }

    public void LookAtPosition(Vector2 targetPosition)
    {
        if (!hook.CheckIfHookIsResting()||!activated) return;
        Vector3 aimDirection = (targetPosition - new Vector2(transform.position.x, transform.position.y));

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        shooterBase.eulerAngles = new Vector3(0, 0, angle);
    }

    public void EnableCannonLights(bool enable)
    {
        activated = enable; 
        
        if (GameManager.Instance.currentGamePhase == GameManager.GamePhase.Night)
        {
            cannonLight.SetActive(enable);
        }
    }
    void LetterRecieved(bool IsCorrect)
    {
        if (!activated) return;
        if (!IsCorrect)
        {
            DisableHarpoonForWhile();
        }
    }
    public void DisableHarpoonForWhile()
    {
        StartCoroutine(DisableHarpoon());
    }

    IEnumerator DisableHarpoon()
    {
        EnableCannonLights(false);
        yield return new WaitForSeconds(repairTime);
        EnableCannonLights(true);
    }
    #endregion

    void OnDestroy()
    {
        EventManager.CorrectLetterHit -= LetterRecieved;

    }

    public void EnableLightAfterSecondsCoroutine(float time,bool enable)
    {
        StartCoroutine(EnableLightAfterSeconds(time));
        IEnumerator EnableLightAfterSeconds(float time)
        {
            yield return new WaitForSeconds(time);
            EnableCannonLights(enable);
        }
    }


 


    public void EnableAction(bool enable)
    {
        activated = enable;
    }
 
}

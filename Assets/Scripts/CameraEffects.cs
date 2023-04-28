using UnityEngine;
using Cinemachine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] CinemachineImpulseListener impulseListener;
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] float impulseValue = 0.5f;

    private void Awake()
    {
        //subscribes the OnLetterHitFunction to the correctLetterHit Event
        EventManager.CorrectLetterHit += OnLetterHit;
    }

    /// <summary>
    /// Applies camera effects when a letter is hit incorrectly
    /// </summary>
    /// <param name="p_isCorrectHit">Whether or not the hit was correct</param>
    public void OnLetterHit(bool p_isCorrectHit)
    {
        if (!p_isCorrectHit)
        {
            if(impulseListener == null)
            {
                impulseListener = GetComponent<CinemachineImpulseListener>();
                if(impulseListener == null) impulseSource = this.gameObject.AddComponent<CinemachineImpulseSource>();
            }
            impulseSource.GenerateImpulseWithForce(impulseValue);
        }
    }

    private void OnDestroy()
    {
        //unsubscribes the OnLetterHitFunction to the correctLetterHit Event
        EventManager.CorrectLetterHit-= OnLetterHit;    
    }
}

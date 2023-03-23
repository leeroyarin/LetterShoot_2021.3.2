using UnityEngine;
using Cinemachine;

public class CameraEffects : MonoBehaviour
{
   /* [SerializeField]static CameraEffects m_cameraEffects;
    public static CameraEffects Instance
    {
        get 
        { 
            if(null == m_cameraEffects) m_cameraEffects = FindObjectOfType<CameraEffects>();
            return m_cameraEffects; 
        }
    }*/
    [SerializeField] CinemachineImpulseListener impulseListener;
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] float impulseValue = 0.5f;

    private void Awake()
    {
        EventManager.CorrectLetterHit += OnLetterHit;

    }

    public void OnLetterHit(bool correctHit)
    {
        if (!correctHit)
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
        EventManager.CorrectLetterHit-= OnLetterHit;    
    }
}

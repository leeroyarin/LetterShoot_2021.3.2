using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSystemManager : MonoBehaviour
{
    [SerializeField] Light2D[] lights;
    [SerializeField] ShadowCaster2D[] shadowCaster;

    private void Awake()
    {
        EventManager.wordCompleted += SetLights;
        StartCoroutine(SetLightsAfterWhile(0.3f));
    }

    private void OnDestroy()
    {
        EventManager.wordCompleted -= SetLights;

    }
    private void SetLights()
    {
        StartCoroutine(SetLightsAfterWhile(2.5f));
    }

    IEnumerator SetLightsAfterWhile(float time)
    {
        yield return new WaitForSeconds(time);
        if (GameManager.Instance.currentGamePhase == GameManager.GamePhase.Night)
        {
            foreach (Light2D light in lights) light.enabled = true;
        }
        else
        {
            foreach (Light2D light in lights) light.enabled = false;
        }
    }
}

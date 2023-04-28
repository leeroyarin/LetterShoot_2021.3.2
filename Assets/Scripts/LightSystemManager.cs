using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSystemManager : MonoBehaviour
{
    [SerializeField] Light2D[] lights;
    [SerializeField] ShadowCaster2D[] shadowCaster;

    private void Awake()
    {
        EventManager.wordCompleted += SetLights;

        // Set the lights initially after a small delay
        StartCoroutine(SetLightsAfterWhile(0.3f));
    }

    private void OnDestroy()
    {
        EventManager.wordCompleted -= SetLights;

    }
    private void SetLights()
    {
        // Set the lights after a delay
        StartCoroutine(SetLightsAfterWhile(2.5f));
    }

    IEnumerator SetLightsAfterWhile(float time)
    {
        // Wait for the specified time
        yield return new WaitForSeconds(time);

        // Check the current game phase and enable/disable the lights accordingly
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

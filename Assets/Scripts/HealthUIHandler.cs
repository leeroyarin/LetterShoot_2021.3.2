using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HealthUIHandler : MonoBehaviour
{
    [SerializeField]int totalHealthCount;

    int currentHealthCount;
    Toggle[] healthIcons;

    private void OnEnable()
    {
        if (GameSceneManager.SceneManagerInstance.GetCurrentSceneName() == "Instructions") 
        { 
            Destroy(this);
        }
        EventManager.CorrectLetterHit += OnLetterHit;
        EventManager.wordCompleted += RestoreHealth;
    }
    private void Start()
    {
        CreateAndReferenceHealthToggleChild();
    }

    /// <summary>
    /// Initializes and instantiates the health icon toggles as children of the current game object.
    /// Sets the current health count to the total health count and creates an array of Toggle components to store the health icons.
    /// </summary>
    private void CreateAndReferenceHealthToggleChild()
    {        
        currentHealthCount = totalHealthCount;
        healthIcons = new Toggle[totalHealthCount];
        GameObject objectExample = GetComponentInChildren<Toggle>().gameObject;
        healthIcons[0] = objectExample.GetComponent<Toggle>();
        for (int i = 1; i < totalHealthCount; i++)
        {
            GameObject recentlyCreatedGameObject = Instantiate(objectExample, this.transform);
            healthIcons[i] = recentlyCreatedGameObject.GetComponent<Toggle>();
        }
    }

    /// <summary>
    /// This method is called when a letter is revealed
    /// </summary>
    /// <param name="p_correctLetter">A boolean indicating whether the hit letter was correct or not.</param>
    private void OnLetterHit(bool p_correctLetter)
    {
        if (!p_correctLetter) 
        {
            currentHealthCount--;
            if(currentHealthCount < 0)
            {
                return;
            }
            healthIcons[currentHealthCount].isOn = false;
            healthIcons[currentHealthCount].GetComponent<Light2D>().enabled = false;
            if (currentHealthCount == 0)
            {
                EventManager.GameCompleted?.Invoke(false);
            }
        }else if (currentHealthCount < totalHealthCount)
        {
            AddHealth();

        }

        void AddHealth()
        {
            AudioManager.Instance.PlaySound(SoundNames.HealthGain);
            healthIcons[currentHealthCount].isOn = true;
            healthIcons[currentHealthCount].GetComponent<Light2D>().enabled = true;
            currentHealthCount++;
        }
    }

    void RestoreHealth()
    {
        currentHealthCount = totalHealthCount;
        foreach(Toggle health in healthIcons)
        {
            health.isOn = true;
            health.GetComponent<Light2D>().enabled = true;
        }
    }

    private void OnDestroy()
    {
        EventManager.CorrectLetterHit -= OnLetterHit;
        EventManager.wordCompleted -= RestoreHealth;
    }
}

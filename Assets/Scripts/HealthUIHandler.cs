using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIHandler : MonoBehaviour
{
    int totalHealthCount;

    int currentHealthCount;
    Toggle[] healthIcons;
    private void Start()
    {
        CreateAndReferenceHealthToggleChild();
        EventManager.CorrectLetterHit += OnLetterHit;
    }

    private void CreateAndReferenceHealthToggleChild()
    {
        GameObject[] shooter = GameObject.FindGameObjectsWithTag("Shooter");
        totalHealthCount = shooter.Length;
        currentHealthCount = totalHealthCount;
        healthIcons = new Toggle[totalHealthCount];
        GameObject objectExample = GetComponentInChildren<Toggle>().gameObject;
        healthIcons[0] = objectExample.GetComponent<Toggle>();
        for (int i = 1; i < shooter.Length; i++)
        {
            GameObject recentlyCreatedGameObject = Instantiate(objectExample, this.transform);
            healthIcons[i] = recentlyCreatedGameObject.GetComponent<Toggle>();
        }
    }

    private void OnLetterHit(bool correctLetter)
    {
        if (!correctLetter) 
        {
            currentHealthCount--;
            if(currentHealthCount < 0)
            {
                return;
            }
            healthIcons[currentHealthCount].isOn = false;
            if(currentHealthCount == 0)
            {
                EventManager.GameCompleted?.Invoke(false);
            }
        }
    }

    private void OnDestroy()
    {
        EventManager.CorrectLetterHit -= OnLetterHit;

    }
}

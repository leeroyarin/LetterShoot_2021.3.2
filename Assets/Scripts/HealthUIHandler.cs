using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIHandler : MonoBehaviour
{
    [SerializeField]int totalHealthCount;

    int currentHealthCount;
    Toggle[] healthIcons;
    private void OnEnable()
    {
        EventManager.CorrectLetterHit += OnLetterHit;
    }
    private void Start()
    {
        CreateAndReferenceHealthToggleChild();
      
    }

    private void CreateAndReferenceHealthToggleChild()
    {
        /*
        //gets array of gameobjects with tag shooter
        //GameObject[] shooter = GameObject.FindGameObjectsWithTag("Shooter");

        //and sets total health count according to the number of shooter tagged gameobjects
        totalHealthCount = shooter.Length;
        */
        
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

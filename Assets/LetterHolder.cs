using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterHolder : MonoBehaviour
{
    [SerializeField] LetterChangingVisualizer letterVisualizer;
    List<UILetters> uiLetterLists = new List<UILetters>();
    [SerializeField]GameObject letterUi;
    public void ActivateLetters(char letter)
    {
        for(int i = 0; i<uiLetterLists.Count; i++)
        {
            if (uiLetterLists[i].letter == letter && !(uiLetterLists[i].activated))
            {
                uiLetterLists[i].gameObject.SetActive(true);
                uiLetterLists[i].activated = true;
                print("Caled");
                break;
            }
        }
    }

    public void DestinationReached(GameObject letterObject)
    {
        letterVisualizer.gameObject.SetActive(false);
        letterObject.SetActive(true);

    }
    public void AddLettersInLetterHolder(char[] p_correctLetters)
    {
        foreach(char letter in p_correctLetters)
        {
            UILetters l_UILetter = Instantiate(letterUi, this.transform).GetComponent<UILetters>();
            l_UILetter.SetLetter(letter);
            uiLetterLists.Add(l_UILetter);
        }
    }

}

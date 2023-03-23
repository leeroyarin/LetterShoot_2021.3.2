using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterHolder : MonoBehaviour
{
    
    #region SingletonStatic
    private static LetterHolder _letterHolder;
    public static LetterHolder LetterHolderInstance
    {
        get
        {
            if (_letterHolder == null) FindObjectOfType<LetterHolder>();
            return _letterHolder;
        }
    }
    #endregion

    #region References
    public List<UILetters> uiLetterLists = new List<UILetters>();
    [SerializeField] GameObject letterUi;
    [SerializeField] TextMeshProUGUI questionTextField;
    #endregion

    #region MonobehaviouricFunctions
    private void Awake()
    {
        _letterHolder = this;
    }
    #endregion

    /// <summary>
    /// 
    /// This class is responsible for UI Activities of the currently running Game Level
    /// that includes : Letters attained, and question(or we can say hint) 
    /// </summary>
    /* Functions Are:
     *   Adding UIletters with UILetter Class to the Letter Holder UI
     *   Setting the current Question for the running level
     *   Returning the UI letter holded in the UIHolder's UIletter list
     *   Checking if the word has been completed or not
     *   Removing all letters after completion of currently completed word for new words
     */




    public void AddLettersInLetterHolder(char[] p_correctLetters)
    {
        /*
         * Instanctiates UI For the word after removing all the UIletters
         * and adds it to the llist
         */
        RemoveAllLetters();
        foreach(char letter in p_correctLetters)
        {
            UILetters l_UILetter = Instantiate(letterUi, this.transform).GetComponent<UILetters>();
            l_UILetter.SetLetter(letter);
            l_UILetter.gameObject.SetActive(false);
            uiLetterLists.Add(l_UILetter);
        }
    }
    public void SetQuestion(string p_questionText)
    {
        questionTextField.text = p_questionText;
    }
    public void SetTheUILetterActive(char letterRecieved)
    {
        UILetters l_UILetters = GetUILetterOfChar(letterRecieved);
        l_UILetters.gameObject.SetActive(true);
        l_UILetters.OnLetterLoad();
        UILetters GetUILetterOfChar(char c)
        {
            //returns uiletters element with the parametered character and unactivated
            int index = uiLetterLists.FindIndex(x => (x.letter == c) && (x.activated == false));
            if (index >= uiLetterLists.Count || index < 0)
            {
                print(uiLetterLists[0] + " " + uiLetterLists[1]);
            }
            return uiLetterLists[index];
        }
    }
    public void RemoveAllLetters()
    {
        //Destroys all the UIletters objects in the list
        foreach(UILetters l_UILetter in uiLetterLists) Destroy(l_UILetter.gameObject);
        //clears the list
        uiLetterLists.Clear();
    }
    public void CheckIfWordIsComplete()
    {
        int count = 0;
        //if the letter in UIletterlist is active in hierarchy count increases
        foreach(UILetters letters in uiLetterLists) if (letters.activated && letters.image.gameObject.activeInHierarchy) count++;

        //checks if count has reached the number of letters of the answer word
        if (count >= uiLetterLists.Count)
        {
            //Resets the Curtains
            Curtain.CurtainInstance.ResetCurtains();

            //Invokes the Events WordCompleted
            EventManager.wordCompleted?.Invoke();
        }
    }

    #region WasteCode
    /*    
    public void ActivateLetters(char letter)
    {
        RemoveAllLetters();
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
    }*/
    #endregion
}

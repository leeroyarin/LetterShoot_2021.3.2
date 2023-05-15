using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
    /// 
    /// This class is responsible for UI Activities of the currently running Game Level
    /// that includes : Letters attained, and question(or we can say hint) 
    /// 
    /// Each correct letter after being hitted are added on its screen
    /// </summary>
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

    /* Functions Are:
     *   Adding UIletters with UILetter Class to the Letter Holder UI
     *   Setting the current Question for the running level
     *   Returning the UI letter holded in the UIHolder's UIletter list
     *   Checking if the word has been completed or not
     *   Removing all letters after completion of currently completed word for new words
     */


    /// <summary>
    /// Instanctiates UI gameObjects For the word after removing all the UIletters and adds it to the list.
    /// </summary>
    /// <param name="p_correctLetters">An array of correct letters to add to the UI.</param>
    public void AddLettersInLetterHolder(char[] p_correctLetters)
    {
       
        //removed all uiletters incase there were any before
        RemoveAllLetters();
        foreach(char letter in p_correctLetters)
        {
            //prefab gets instantiated and component gets referenced
            UILetters l_UILetter = Instantiate(letterUi, this.transform).GetComponent<UILetters>();

            l_UILetter.SetLetter(letter);
            uiLetterLists.Add(l_UILetter);
        }
    }

    /// <summary>
    /// Sets the current question for the running level.
    /// </summary>
    /// <param name="p_questionText">The text of the question.</param>
    public void SetQuestion(string p_questionText)
    {
        questionTextField.text = p_questionText;
    }

    /// <summary>
    /// Sets the UI letter corresponding to the received character to active.
    /// </summary>
    /// <param name="letterRecieved">The character corresponding to the UI letter to activate.</param>
    public void SetTheUILetterActive(char letterRecieved)
    {
        UILetters l_UILetters = GetUILetterOfChar(letterRecieved);
        l_UILetters.OnLetterLoad();

        //returns uiletters element with the parametered character and unactivated
        UILetters GetUILetterOfChar(char p_letterCharacter)
        {
            int l_index = uiLetterLists.FindIndex(x => (x.letter == p_letterCharacter) && (x.activated == false));
            if (l_index >= uiLetterLists.Count || l_index < 0)
            {
                print(uiLetterLists[0] + " " + uiLetterLists[1]);
            }
            return uiLetterLists[l_index];
        }
    }

    public void RemoveAllLetters()
    {
        //Destroys all the UIletters objects in the list
        foreach(UILetters l_UILetter in uiLetterLists) Destroy(l_UILetter.gameObject);
        //clears the list
        uiLetterLists.Clear();
    }

    /// <summary>
    /// Checks if the word has been completed.
    /// </summary>
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

            //stop all sounds
            AudioManager.Instance.StopAllSoundAtOnce();
        }
    }
}

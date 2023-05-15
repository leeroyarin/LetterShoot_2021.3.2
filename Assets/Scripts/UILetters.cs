using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILetters : MonoBehaviour
{
    #region References
    public TextMeshProUGUI letterText;
    public bool activated;
    public char letter;
    public RectTransform rectTransform;
    public Image image;
    #endregion

    #region Monobehaviouric Function
    public void OnEnable()
    {
        EventManager.wordCompleted += StopAllCoroutines;
    }

    private void OnDisable()
    {
        EventManager.wordCompleted -= StopAllCoroutines;
    }

    private void OnDestroy()
    {
        EventManager.wordCompleted -= StopAllCoroutines;
    }
    #endregion

    #region UILettersFunctionalities

    /// <summary>
    /// sets letter text
    /// </summary>
    /// <param name="p_letter"></param>
    public void SetLetter(char p_letter)
    {
        //sets letter
        letterText.text = p_letter.ToString();

        //hides letter
        letterText.enabled = false;
        letter = p_letter;
    }
    public void OnLetterLoad()
    {
        activated = true;

        //shows letter text
        letterText.enabled = true;

        StartCoroutine(InvokeWordCompleteEventCoroutine());
    }
  
    #endregion

    #region Coroutines

    //Invokes the function after certain time
    //Used to display the correct answer for th
    IEnumerator InvokeWordCompleteEventCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        LetterHolder.LetterHolderInstance.CheckIfWordIsComplete();

    }
    #endregion


    #region WasteCodes    

    void InvokeWordCompleteEvent()
    {
        LetterHolder.LetterHolderInstance.CheckIfWordIsComplete();
    }
    public void OnLetterReached()
    {
        image.gameObject.SetActive(true);
        StartCoroutine(InvokeWordCompleteEventCoroutine());

        //Invoke(nameof(InvokeWordCompleteEvent), 1.5f);
    }
    #endregion
}

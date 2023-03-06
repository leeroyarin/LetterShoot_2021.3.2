using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILetters : MonoBehaviour
{
    public TextMeshProUGUI letterText;
    public bool activated;
    public char letter;
    public RectTransform rectTransform;
    public Image image;

    public void SetLetter(char p_letter)
    {
        letterText.text = p_letter.ToString();
        letter = p_letter;
    }
    public void OnLetterLoad()
    {
        image.gameObject.SetActive(false);
    }
    public void OnLetterReached()
    {
        image.gameObject.SetActive(true);
    }
}

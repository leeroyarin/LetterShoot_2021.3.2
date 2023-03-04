using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILetters : MonoBehaviour
{
    public TextMeshProUGUI letterText;
    public bool activated;
    public char letter;
    public void SetLetter(char p_letter)
    {
        letterText.text = p_letter.ToString();
        letter = p_letter;
    }
}

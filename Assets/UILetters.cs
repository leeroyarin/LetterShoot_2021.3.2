using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILetters : MonoBehaviour
{
    TextMeshProUGUI letterText;

    public void SetLetter(char letter)
    {
        letterText.text = letter.ToString();
    }
}

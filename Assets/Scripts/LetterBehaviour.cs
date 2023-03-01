using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterBehaviour : MonoBehaviour
{
    public TextMeshPro letterText;
    char _letter;

    public void SetLetterCharacter(char letter)
    {
        _letter = letter;
        letterText.text = letter.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (LetterManager.letterManagerInstance.CheckIfTheLetterIsInWord(_letter,Camera.main.WorldToScreenPoint(transform.position)))
            {
                gameObject.SetActive(false);
            }
        }
    }
}

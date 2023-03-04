using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterBehaviour : MonoBehaviour
{
    public TextMeshPro letterText;
    char _letter;
    [SerializeField] Rigidbody2D rb;
    
    public char Letter { get { return _letter; } }  
    public void SetLetterCharacter(char letter)
    {
        _letter = letter;
        letterText.text = letter.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            EventManager.LetterRecieved(_letter,this);
        }
    }

    private void DisableLetterArmy()
    {
        rb.isKinematic = true;
    }

    public void OnCorrectLetter()
    {
        gameObject.SetActive(false );
    }

    public void OnWrongLetter()
    {
        /*//Doesnt work because of the behaviour of moving along the path creator
        rb.isKinematic = false;
        rb.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);
        Invoke("DisableLetterArmy", 4f);*/
        gameObject.SetActive(false);

    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterBehaviour : MonoBehaviour
{
    public TextMeshPro letterText;
    char _letter;
    [SerializeField] LetterMovement letterMovement;
    [SerializeField] Rigidbody2D rb;
    bool _done = false;
    
    public char Letter { get { return _letter; } }

    private void Awake()
    {
        if (letterMovement == null) letterMovement = GetComponent<LetterMovement>();
    }

    private void OnEnable()
    {
        _done = false;
    }
    public void SetLetterCharacter(char letter)
    {
        _letter = letter;
        letterText.text = letter.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")  && !_done)
        {
            EventManager.LetterRecieved(_letter,this);
            _done = true;
        }
    }

    public void OnCorrectLetter(LetterHolder letterHolder)
    {
        letterMovement.GetTheLetterToLetterHolder(letterHolder.GetUILetterOfChar(Letter));
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

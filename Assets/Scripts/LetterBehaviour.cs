using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterBehaviour : MonoBehaviour
{
    public TextMeshPro letterText;
    [SerializeField] LetterMovement letterMovement;
    [SerializeField] Rigidbody2D rb;
    [SerializeField]char _letter;
    [SerializeField]Transform objectToParentOn;

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
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Hook")  && !_done)
        {
            gameObject.transform.SetParent(LetterManager.LetterManagerInstance.transform,false);
            collidedObject.GetComponent<HookBehaviour>().SetLetterToHook(this);
            _done = true;
        }
    }
    public void CheckTheContainer()
    {
        EventManager.LetterRecieved?.Invoke(_letter, this);
        this.StartCoroutine(ShowTheItem());
        letterMovement.StopCoroutine(letterMovement.MoveAlongHook());
    }
    public void OnCorrectLetter()
    {
        //Reveal item
        if (!this.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        //letterMovement.GetTheLetterToLetterHolder(letterHolder.GetUILetterOfChar(Letter));

    }

    public void OnWrongLetter()
    {
        /*//Doesnt work because of the behaviour of moving along the path creator
        rb.isKinematic = false;
        rb.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);
        Invoke("DisableLetterArmy", 4f);*/
        if (!this.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }

    IEnumerator ShowTheItem()
    {
        letterMovement.StopAllCoroutines();
        yield return new WaitForSeconds(1);

        yield return new WaitForSeconds(1);

        gameObject.SetActive(false);
        this.transform.SetParent(objectToParentOn,false);
        this.transform.localPosition = Vector3.zero;
        //localScale if necessary
    }

    public void GetReferenceToParentObjects(Transform parentTransform) => objectToParentOn = parentTransform;
}

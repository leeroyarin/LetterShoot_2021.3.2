using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterBehaviour : MonoBehaviour,IOnCollisionWithHook
{
    public TextMeshPro letterText;
    [SerializeField] LetterMovement letterMovement;
    [SerializeField]char _letter;
    [SerializeField]Transform objectToParentOn;
    [SerializeField] Animator animator;

    public bool destroyed;
    bool _isHooked = false;
    
    public char Letter { get { return _letter; } }

    private void Awake()
    {
        if (letterMovement == null) letterMovement = GetComponent<LetterMovement>();
    }

    private void OnEnable()
    {
        _isHooked = false;
    }

    public void MakeTheObjectUndrestroyed() => destroyed = false;


    private void OnDisable()
    {
        destroyed = true;
    }
    public void SetLetterCharacter(char letter)
    {
            
        _letter = letter;
        letterText.text = letter.ToString();
    }

    public void CheckTheContainer()
    {
        EventManager.LetterRecieved?.Invoke(_letter, this);
//        this.StartCoroutine(ShowTheItem());
        letterMovement.StopCoroutine(letterMovement.MoveAlongHook());
    }
    public void PlayCorrectLetterAction()
    {
         //Reveal item
        if (!this.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        animator.Play("CorrectLetterRevealAnimation");
//        this.StartCoroutine(ShowTheItem());
        //letterMovement.GetTheLetterToLetterHolder(letterHolder.GetUILetterOfChar(Letter));

    }

    public void PlayIncorrectLetterAction()
    {
        /*//Doesnt work because of the behaviour of moving along the path creator
        rb.isKinematic = false;
        rb.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);
        Invoke("DisableLetterArmy", 4f);*/
        if (!this.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        animator.Play("WrongLetterRevealAnimation");

        //        this.StartCoroutine(ShowTheItem());
    }

    /*    IEnumerator ShowTheItem()
        {
            boxCollider.enabled = false;
            letterMovement.StopAllCoroutines();
            yield return new WaitForSeconds(1);

            yield return new WaitForSeconds(1);
            boxCollider.enabled = true;

            gameObject.SetActive(false);
            this.transform.SetParent(objectToParentOn,false);
            this.transform.localPosition = Vector3.zero;
            //localScale if necessary
        }*/

    public void GetHooked(HookBehaviour hook)
    {
        if (!this._isHooked)
        {
            gameObject.transform.SetParent(LetterManager.LetterManagerInstance.transform, false);
            hook.SetLetterToHook(this);
            _isHooked = true;
        }
    }

    public void OnWrongLetterAction()
    {        
        EventManager.CorrectLetterHit?.Invoke(false);
        AudioManager.Instance.PlaySound(SoundNames.Explosion);

    }

    public void OnCorrectLetterAction()
    {
        EventManager.CorrectLetterHit?.Invoke(true);
    }

    public void EndAnimation()
    {
        //parents to the gameObject
        this.transform.SetParent(objectToParentOn, false);
        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);
    }
}


interface IOnCollisionWithHook
{
    public void GetHooked(HookBehaviour hook);
}
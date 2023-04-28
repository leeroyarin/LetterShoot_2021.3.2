using TMPro;
using UnityEngine;

/// <summary>
/// A script attached to a letter box object that manages its behavior and interactions with the game.
/// </summary>
public class LetterBehaviour : MonoBehaviour,IOnCollisionWithHook
{
    #region Public Variables
    /// <summary>
    /// The TextMeshPro component that displays the letter
    /// </summary>
    public TextMeshPro letterText;   
    
    /// <summary>
    /// Whether or not the letter has been destroyed or collected or recieved
    /// </summary>
    public bool destroyed;
    #endregion

    #region Serialized and Unserialized Private Variables
    [SerializeField] LetterMovement letterMovement;         // The LetterMovement component that controls the letter's movement
    [SerializeField] Transform objectToParentOn;            // The transform that the letter should be parented to when it's not in use
    [SerializeField] Animator animator;                     // The Animator component that controls the letter's animations

    bool _isHooked = false;                                 // Whether or not the letter box is currently hooked by the hook
    char _letter;                                           // The character value of the letter
    #endregion

    public char Letter { get { return _letter; } }

    private void Awake()
    {
        if (letterMovement == null) letterMovement = GetComponent<LetterMovement>();
    }

    private void OnEnable()
    {
        _isHooked = false;
    }

    /// <summary>
    /// Marks the letter as not destroyed.
    /// </summary>
    public void MakeTheObjectUndrestroyed() => destroyed = false;


    private void OnDisable()
    {
        destroyed = true;
    }

    /// <summary>
    /// Sets the character value of the letter and updates the displayed text.
    /// </summary>
    /// <param name="p_letter">The character value of the letter.</param>
    public void SetLetterCharacter(char p_letter)
    {
            
        _letter = p_letter;
        letterText.text = p_letter.ToString();
    }

    /// <summary>
    /// Checks the container and invokes the LetterRecieved event.
    /// </summary>
    public void CheckTheContainer()
    {
        EventManager.LetterRecieved?.Invoke(_letter, this);
        letterMovement.StopCoroutine(letterMovement.MoveAlongHook());
    }

    /// <summary>
    /// Plays the correct letter reveal animation.
    /// </summary>
    public void PlayCorrectLetterAction()
    {
        //incase the object is not active in hierarchy the gameObject gets activated
        if (!this.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        animator.Play("CorrectLetterRevealAnimation");
    }

    /// <summary>
    /// Plays the wrong letter reveal animation.
    /// </summary>
    public void PlayIncorrectLetterAction()
    {
        if (!this.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        animator.Play("WrongLetterRevealAnimation");
    }

    /// <summary>
    /// Hooks the letter to the hook and sets its parent to the LetterManager's transform.
    /// </summary>
    /// <param name="p_hook">The HookBehaviour component that hooked the letter.</param>
    public void GetHooked(HookBehaviour p_hook)
    {
        if (!this._isHooked)
        {
            gameObject.transform.SetParent(LetterManager.LetterManagerInstance.transform, false);
            p_hook.SetLetterToHook(this);
            _isHooked = true;
        }
    }

    /// <summary>
    /// Handles the action when the wrong letter is revealed.
    /// </summary>
    public void OnWrongLetterAction()
    {     
        //from animation event
        EventManager.CorrectLetterHit?.Invoke(false);
        AudioManager.Instance.PlaySound(SoundNames.Explosion);
    }

    /// <summary>
    /// Handles the action when the correct letter is revealed.
    /// </summary>
    public void OnCorrectLetterAction()
    {
        //from animation event
        EventManager.CorrectLetterHit?.Invoke(true);
        AudioManager.Instance.PlaySound(SoundNames.CoinsEarned);
    }

    /// <summary>
    /// sets parent back to the train Box and resets its local position and gameObject is set to false so it wont show in the scene
    /// </summary>
    public void EndAnimation()
    {
        //from animation event
        //parents to the gameObject
        this.transform.SetParent(objectToParentOn, false);
        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// plays box breaking sound
    /// </summary>
    public void OpenBox()
    {
        //from animation event
        AudioManager.Instance.PlaySound(SoundNames.BreakBox);
    }
}

interface IOnCollisionWithHook
{
    public void GetHooked(HookBehaviour hook);
}
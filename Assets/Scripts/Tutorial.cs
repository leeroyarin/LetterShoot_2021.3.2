using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This script is responsible for managing the tutorial flow and displaying text to the player in a typewriter effect.
/// </summary>
public class Tutorial : MonoBehaviour
{
    // Static reference to the Tutorial instance
    static Tutorial _tutorial;
    // Public property to access the Tutorial instance
    public static Tutorial Instance
    {
        get
        {
            // If the instance is null, find the Tutorial object in the scene
            if (_tutorial == null) _tutorial = FindObjectOfType<Tutorial>();
            return _tutorial;
        }
    }
    // The different states of the tutorial
    enum TutorialScene
    {
        FirstOpening,
        Instruction,
    }

    // The current state of the tutorial
    TutorialScene _tutorialState;

    //text of tutorial board
    [SerializeField] TextMeshProUGUI text;

    //animator of tutorial
    [SerializeField] Animator animator;

    //typing speed of letters in tutorial
    [SerializeField] float typingSpeed;

    //MainGameUIManager responsible for main UI responsibility 
    [SerializeField] MainGameUIManager UIManager;

    // Whether the player can proceed to the next tutorial step
    bool _doNext = false;

    // The coroutine used to print tutorial text
    Coroutine _textPrintingCoroutine;


    private void Awake()
    {
        //subscription
        EventManager.CorrectLetterHit += AllowNext;

        // If the current scene is "Instructions", set the tutorial state to Instruction and play the tutorial
        if (GameSceneManager.SceneManagerInstance.GetCurrentSceneName() == "Instructions")
        {
            _tutorialState = TutorialScene.Instruction;
            PlayTutorial();
        }else
        {
            // If this is the player's first game, set the tutorial state to FirstOpening and play the tutorial
            if (PlayerData.FirstGame)
            {
                _tutorialState = TutorialScene.FirstOpening;
                PlayTutorial();
            }
            else
            {
                // If this is not the player's first game, destroy the tutorial object
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe the AllowNext method from the CorrectLetterHit event
        EventManager.CorrectLetterHit -= AllowNext;
    }

    // Allow the player to proceed to the next tutorial step if they hit the correct letter
    public void AllowNext(bool p_isCorrect)
    {

        // If the player can already proceed, return
        if (_doNext == true) return;

        // Stop the text printing coroutine
        if (_textPrintingCoroutine!=null)StopCoroutine(_textPrintingCoroutine);

        // Set doNext to true if the player hit the correct letter
        _doNext = p_isCorrect;
    }

    // Play the tutorial according to the current state
    public void PlayTutorial()
    {
        switch (_tutorialState)
        {
            // If the tutorial state is FirstOpening, play the FirstOpeningAnimation
            case TutorialScene.FirstOpening:
                animator.Play("FirstOpeningAnimation");
                break;
            // If the tutorial state is Instruction, start the InstructionCoroutine
            case TutorialScene.Instruction:
                StartCoroutine(InstructionCoroutine());
                break;
            // If the tutorial state is not recognized, destroy the tutorial object
            default:
                print("Destroyed");
                _tutorial = null;
                Destroy(this.gameObject);
                break;
        }
    }

    // The coroutine used to play the Instruction tutorial step by step
    IEnumerator InstructionCoroutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        animator.Play("IntroductionAnimation");

        // Wait until the doNext flag is set
        yield return new WaitUntil(() => _doNext);
        StopCoroutine(_textPrintingCoroutine);
        _doNext = false;
        animator.Play("ChooseHarpoon");

        // Wait until the player has an active harpoon shooter
        yield return new WaitUntil(() => InputManager.InputManagerInstance.hasActiveHarpoonShooter);

        // Stop the text printing coroutine and reset
        StopCoroutine(_textPrintingCoroutine);
        animator.Play("ShootTheLetter");
        _doNext = false;

        // Wait until the doNext flag is set
        yield return new WaitUntil(() => _doNext);
        animator.Play("FinalGuide");
    }

    /// <summary>
    /// prints text according to the string recieved from parameter
    /// </summary>
    /// <param name="p_textToPrint"> text to print in tutorial board </param>
    public void PrintText(string p_textToPrint)
    {        
        //used by animation event
        _textPrintingCoroutine = StartCoroutine(StartPrinting(p_textToPrint));
    }

    /// <summary>
    /// Coroutine function that prints text in the TextMeshProUGUI object one character at a time with a delay.
    /// </summary>
    /// <param name="p_textToPrint">The text to be printed.</param>
    /// <returns>Returns an IEnumerator object that can be used with StartCoroutine() to print text in a coroutine.</returns>
    IEnumerator StartPrinting(string p_textToPrint)
    {
        text.text = " ";
        Queue<char> l_textLetters = new Queue<char>(p_textToPrint.ToCharArray());
        while ( l_textLetters.Count >0)
        {
            text.text += l_textLetters.Dequeue();
            yield return new WaitForSecondsRealtime(typingSpeed);
            
        }
    }

    /// <summary>
    /// Changes the device input mode based on user's selection and plays a reverse animation.
    /// </summary>
    /// <param name="p_isMobile">A boolean indicating if the device input mode is mobile or not.</param>
    public void OnDeviceInputMobile(bool p_isMobile)
    {
        //used by unity button click event

        // Update the device input mode.
        SettingsData.IsMobileDevice = p_isMobile;
        animator.Play("FirstOpeningAnimationReverse");
    }
    /// <summary>
    /// This method is called by an animation event when the player completes the first start scene tutorial questionaire. 
    /// </summary>
    public void OnFirstDeviceInputClosure()
    {
        //used by animation event
        if(GameSceneManager.SceneManagerInstance==null) Instantiate(new GameObject(),AudioManager.Instance.transform).AddComponent<GameSceneManager>();
        GameSceneManager.SceneManagerInstance.ChangeSceneOnName("Instructions");
    }

    /// <summary>
    /// This method is called by an animation event when the player completes the tutorial. 
    /// It destroys the Tutorial game object and sets the player's FirstGame variable to false.
    /// </summary>
    public void OnCompletingTutorial()
    {
        //used by animation event
        Destroy(this.gameObject);
        PlayerData.FirstGame = false;
    }

    /// <summary>
    /// </summary>
    public void PointerExit()
    {
        if(UIManager==null) UIManager= FindObjectOfType<MainGameUIManager>();
        UIManager.OnPointerUpThePauseButton(false);
    }
}

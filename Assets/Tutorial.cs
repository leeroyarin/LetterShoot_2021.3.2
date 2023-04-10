using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    static Tutorial _tutorial;
    public static Tutorial instance
    {
        get
        {
            if (_tutorial == null) _tutorial = FindObjectOfType<Tutorial>();
            return _tutorial;
        }
    }
    enum TutorialState
    {
        FirstOpening,
        Instruction,
    }

    enum InstructionTutorialState{
        FirstIntroductionPhase = 1,
        SecondTouchTheHarpoon = 2,
        ThirdShootTheLetter = 3,
        FourthChangeTheShooter = 4,
        FifthShootTheCorrectLetter = 5,
        SixthFinishTheGame = 6
    }

    TutorialState tutorialState;
    InstructionTutorialState instructionTutorialState;

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Animator animator;
    [SerializeField] float typingSpeed;

    bool doNext = false;
    Coroutine textPrintingCoroutine;


    private void Awake()
    {
        EventManager.CorrectLetterHit += AllowNext;
        if (GameSceneManager.SceneManagerInstance.GetCurrentSceneName() == "Instructions")
        {
            tutorialState = TutorialState.Instruction;
            PlayTutorial();
        }
        else
        {
            if (PlayerData.FirstGame)
            {
                tutorialState = TutorialState.FirstOpening;
                PlayTutorial();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        EventManager.CorrectLetterHit -= AllowNext;
    }

    public void AllowNext(bool isCorrect)
    {

        //if is correct true
        if (doNext == true) return;
        if(textPrintingCoroutine!=null)StopCoroutine(textPrintingCoroutine);
        doNext = isCorrect;
    }

    public void PlayTutorial()
    {
        switch (tutorialState)
        {
            case TutorialState.FirstOpening:
                animator.Play("FirstOpeningAnimation");
                break;
            case TutorialState.Instruction:
                StartCoroutine(InstructionCoroutine());
                break;
            default:
                print("Destroyed");
                _tutorial = null;
                Destroy(this.gameObject);
                break;
        }
    }


    IEnumerator InstructionCoroutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        animator.Play("IntroductionAnimation");
        yield return new WaitUntil(() => doNext);
        StopCoroutine(textPrintingCoroutine);
        doNext = false;
        animator.Play("ChooseHarpoon");
        yield return new WaitUntil(() => InputManager.InputManagerInstance.HasShooter);
        StopCoroutine(textPrintingCoroutine);
        animator.Play("ShootTheLetter");
        doNext = false;
        yield return new WaitUntil(() => doNext);
        animator.Play("FinalGuide");
    }


    public void PrintText(string textToPrint)
    {        
        //used by animation event

        textPrintingCoroutine = StartCoroutine(StartPrinting(textToPrint));
    }

    IEnumerator StartPrinting(string textToPrint)
    {
        text.text = " ";
        Queue<char> textLetters = new Queue<char>(textToPrint.ToCharArray());
        while ( textLetters.Count >0)
        {
            text.text += textLetters.Dequeue();
            yield return new WaitForSecondsRealtime(typingSpeed);
            
        }
    }

    public void OnDeviceInputMobile(bool isMobile)
    {
        //used by animation event
        SettingsData.IsMobileDevice = isMobile;
        animator.Play("FirstOpeningAnimationReverse");
    }

    public void OnFirstDeviceInputClosure()
    {
        //used by animation event
        if(GameSceneManager.SceneManagerInstance==null) Instantiate(new GameObject(),AudioManager.Instance.transform).AddComponent<GameSceneManager>();
        GameSceneManager.SceneManagerInstance.ChangeSceneOnName("Instructions");
    }

    public void OnCompletingTutorial()
    {
        //used by animation event
        Destroy(this.gameObject);

        PlayerData.FirstGame = false;
    }
    MainGameUIManager UIManager;
    public void PointerExit()
    {
        if(UIManager==null) UIManager= FindObjectOfType<MainGameUIManager>();
        UIManager.OnPointerUpThePauseButton(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SingletonStatic
    static private GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null) _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }
    #endregion

    GamePhase currentGamePhase;

    [SerializeField]LetterHolder m_letterHolder;
    [SerializeField]LetterManager m_letterManager;

    [SerializeField] List<QuestionAnswer> questionAnswersSet;
    Queue<QuestionAnswer> m_questionAnswerList;


    [SerializeField] GameObject[] dayNightPhase;
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        SetReference();

        //initializing game
        SortQuestionsList();
        CheckIfAnyQuestionsLeft();

        void SetReference()
        {
            if(m_letterManager==null) m_letterManager = LetterManager.LetterManagerInstance;
            if(m_letterHolder==null) m_letterHolder = LetterHolder.LetterHolderInstance;
        }

        EventManager.GameCompleted += OnGameComplete;

//        AudioManager.Instance.StartPlayingSFXOnLoop(SoundNames.TrainRun,5.6f);
//        AudioManager.Instance.PlayEndingSfxAfterLoopingSfx(SoundNames.TrainRun,SoundNames.TrainEnd,5.5f);

    }

    private void OnDestroy()
    {
        EventManager.GameCompleted -= OnGameComplete;

    }

    private void SortQuestionsList()
    {
        var random = new System.Random();
        QuestionAnswer[] l_UnsortedList = questionAnswersSet.OrderBy(x => random.Next()).ToArray();
        questionAnswersSet.Clear();
        m_questionAnswerList = new Queue<QuestionAnswer>(l_UnsortedList);
        
    }

    private void CheckIfAnyQuestionsLeft()
    {
        if (m_questionAnswerList.Count == 0)
        {
            EventManager.GameCompleted?.Invoke(true);
        }else{

            QuestionAnswer questionAnswer = m_questionAnswerList.Dequeue();
            m_letterManager.SetQuestionAndAnswer(questionAnswer.Question, questionAnswer.Answer,questionAnswer.WrongLetters);
            LetterHolder.LetterHolderInstance.AddLettersInLetterHolder(questionAnswer.Answer.ToArray());
            LetterHolder.LetterHolderInstance.SetQuestion(questionAnswer.Question);
//            AudioManager.Instance.PlaySound(SoundNames.TrainStart,1f);
            AudioManager.Instance.StartPlayingSFXOnLoop(SoundNames.TrainLoop,1f);
            dayNightPhase[(int)currentGamePhase].SetActive(false);

            currentGamePhase = (GamePhase)(m_questionAnswerList.Count % 4);
            dayNightPhase[(int)currentGamePhase].SetActive(true);

        }
    }
    public void OnGameComplete(bool completed)
    {
        foreach(GameObject lights in dayNightPhase) lights.SetActive(false);
    }
    public void WaitForWhileAndChangeQuestion()
    {
        Invoke(nameof(CheckIfAnyQuestionsLeft), 1f);
    }
    #region Classes
    [System.Serializable]
    public class QuestionAnswer
    {
        [SerializeField]private string question;
        [SerializeField]private string answer;
        [SerializeField]private string wrongLetters;

        public string Question
        {
            get => question.ToUpper();
        }
        public string Answer
        {
            get => answer.ToUpper();
        }
        public string WrongLetters
        {
            get => wrongLetters.ToUpper();
        }
    }
    #endregion

    public enum GamePhase
    {
        Morning = 0,
        Day = 1,
        Evening = 2,
        Night = 3,
    }

    public enum GameSubPhase
    {
        Initialization,
        Questioning,
        Ending
    }
}



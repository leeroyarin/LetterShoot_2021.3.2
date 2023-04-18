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

    public GamePhase currentGamePhase;

    [SerializeField]LetterHolder m_letterHolder;
    [SerializeField]LetterManager m_letterManager;

    [SerializeField] List<QuestionAnswer> questionAnswersSet;
    Queue<QuestionSet> m_questionAnswerList;

    [SerializeField] int currentLevel;
    [SerializeField] GameObject[] dayNightPhase;
    private void Awake()
    {
        _instance = this;
        DeactivateAllDayNightPhase();
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
        QuestionSet[] l_UnsortedList = QuizManager.GetQuestionsByLevelAndIndex(currentLevel).OrderBy(x => random.Next()).ToArray();
        questionAnswersSet.Clear();
        m_questionAnswerList = new Queue<QuestionSet>(l_UnsortedList);
        if (m_questionAnswerList.Count < 4) print("EROR IN List");
        while(m_questionAnswerList.Count > 4)
        {
            m_questionAnswerList.Dequeue();
        }
        
    }

    private void CheckIfAnyQuestionsLeft()
    {
        if (m_questionAnswerList.Count == 0)
        {
            GameSceneManager.SceneManagerInstance.OnSuccessfulGameComplete();
            EventManager.GameCompleted?.Invoke(true);
        }else{

            QuestionSet questionAnswer = m_questionAnswerList.Dequeue();
            m_letterManager.SetQuestionAndAnswer(questionAnswer.Question, questionAnswer.AnswerWord,questionAnswer.IncorrectOption);
            LetterHolder.LetterHolderInstance.AddLettersInLetterHolder(questionAnswer.AnswerWord.ToArray());
            LetterHolder.LetterHolderInstance.SetQuestion(questionAnswer.Question);
            AudioManager.Instance.StartPlayingSFXOnLoop(SoundNames.TrainLoop,1f);


            //day night cycle control
            InputManager.InputManagerInstance.ActivateLights();
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

    public void DeactivateAllDayNightPhase()
    {
        foreach(GameObject phase in dayNightPhase) phase.SetActive(false);  
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
        Morning = 3,
        Day = 2,
        Evening = 1,
        Night = 0,
    }

    public enum GameSubPhase
    {
        Initialization,
        Questioning,
        Ending
    }
}



using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// This class manages the game by sorting the question list, checking if there are any questions left,
/// activating day/night cycles, and handling game phases.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region SingletonStatic
    // Static reference to the game manager instance
    static private GameManager _instance;
    // Property to get the game manager instance, and create it if it doesn't exist yet
    public static GameManager Instance
    {
        get
        {
            if(_instance == null) _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }
    #endregion
    // The current game phase (morning, day, evening, or night)
    public GamePhase currentGamePhase;

    // Reference to the LetterHolder script
    [SerializeField]LetterHolder m_letterHolder;
    // Reference to the LetterManager script
    [SerializeField]LetterManager m_letterManager;

    // Queue of question sets
    Queue<QuestionSet> m_questionAnswerList;

    // The current game level
    [SerializeField] int currentLevel;
    // Array of game objects for day/night cycles
    [SerializeField] GameObject[] dayNightPhase;
    private void Awake()
    {
        // Set the instance to this game manager
        _instance = this;
        // Deactivate all day/night cycles 
        DeactivateAllDayNightPhase();
    }

    private void Start()
    {
        // Set references to the LetterHolder and LetterManager scripts
        SetReference();

        // Initialize the game
        SortQuestionsList();
        CheckIfAnyQuestionsLeft();

        // Subscribe to the GameCompleted event
        EventManager.GameCompleted += OnGameComplete;


        void SetReference()
        {
            if (m_letterManager == null) m_letterManager = LetterManager.LetterManagerInstance;
            if (m_letterHolder == null) m_letterHolder = LetterHolder.LetterHolderInstance;
        }
    }

    private void OnDestroy()
    {
        //unsubscribe OnGameComplete from gameComplete
        EventManager.GameCompleted -= OnGameComplete;
    }

    /// <summary>
    /// Sorts the list of questions for the current game level in a random order and puts them into a queue.
    /// </summary>
    private void SortQuestionsList()
    {
        var random = new System.Random();
        //gets randomly sorted questions for level
        QuestionSet[] l_UnsortedList = QuizManager.GetQuestionsByLevelAndIndex(currentLevel).OrderBy(x => random.Next()).ToArray();
        m_questionAnswerList = new Queue<QuestionSet>(l_UnsortedList);

        //incase there is question's count less than 4 error is printed
        if (m_questionAnswerList.Count < 4) print("EROR IN List");
        //unless the count of the questions gets equal to 4 the questions are removed from the questionList
        while(m_questionAnswerList.Count > 4) m_questionAnswerList.Dequeue();
    }

    /// <summary>
    /// Checks if there are any questions left in the queue. If not, the game is completed.
    /// If there are questions left, it dequeues the first question from the queue, sets the question and answer in the letter manager,
    /// adds letters in the letter holder, sets the question in the letter holder, and starts playing the train loop sound.
    /// It also activates the lights and sets the day/night phase based on the number of questions left in the queue.
    /// </summary>
    private void CheckIfAnyQuestionsLeft()
    {
        //if there is no any questions left the game gets completed
        if (m_questionAnswerList.Count == 0)
        {
            GameSceneManager.SceneManagerInstance.OnSuccessfulGameComplete();
            EventManager.GameCompleted?.Invoke(true);
        }else{
            //gets the first question
            QuestionSet questionAnswer = m_questionAnswerList.Dequeue();

            //adds letters in the letter holder and sets the question
            LetterHolder.LetterHolderInstance.AddLettersInLetterHolder(questionAnswer.AnswerWord.ToArray());
            LetterHolder.LetterHolderInstance.SetQuestion(questionAnswer.Question);

            m_letterManager.SetQuestionAndAnswer(questionAnswer.Question, questionAnswer.AnswerWord, questionAnswer.IncorrectOption);

            //starts playing the train loop sound
            AudioManager.Instance.StartPlayingSFXOnLoop(SoundNames.TrainLoop,1f);

            //day night cycle control
            InputManager.InputManagerInstance.ActivateLights();
            dayNightPhase[(int)currentGamePhase].SetActive(false);
            currentGamePhase = (GamePhase)(m_questionAnswerList.Count % 4);
            dayNightPhase[(int)currentGamePhase].SetActive(true);

        }
    }
    /// <summary>
    /// This method is called when the game is completed.
    /// It deactivates all the day/night cycle phases.
    /// </summary>
    /// <param name="p_completed">A boolean value indicating whether the game is completed or not.</param>
    public void OnGameComplete(bool p_completed)
    {
        foreach(GameObject lights in dayNightPhase) lights.SetActive(false);
    }  

    /// <summary>
    /// Waits for a short period of time and changes the question by invoking CheckIfAnyQuestionsLeft method after 1 second.
    /// </summary>
    public void WaitForWhileAndChangeQuestion()
    {
        Invoke(nameof(CheckIfAnyQuestionsLeft), 1f);
    }
    /// <summary>
    /// Deactivates all the day-night phase GameObjects.
    /// </summary>
    public void DeactivateAllDayNightPhase()
    {
        foreach(GameObject phase in dayNightPhase) phase.SetActive(false);  
    }

    public enum GamePhase
    {
        Morning = 3,
        Day = 2,
        Evening = 1,
        Night = 0,
    }
}



using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LetterManager : MonoBehaviour
{
    #region StaticSingleton
    private static LetterManager _letterManager;
    public static LetterManager LetterManagerInstance
    {
        get
        {
            if(_letterManager == null) FindObjectOfType<LetterManager>();
            return _letterManager;
        }
    }
    #endregion

    [SerializeField] Transform _objectOriginPoint;

    #region New
    [SerializeField] int maximumLetters = 30;
    [SerializeField] float armySpawnInterval;

    [Space(30f)]
    
    [SerializeField] PathCreator path;
    [SerializeField] GameObject letterTrainBoxPrefab;
    [SerializeField] LetterHolder letterHolder;
    [SerializeField] GameObject trainHeadPrefab;

    char[] m_correctLetters;
    char[] m_wrongLetters;
    bool _gameCompleted = false;

    Queue<char> m_allLetters;
    Coroutine m_currentCoroutine;
    List<GameObject> letterTrainBoxesList;

    private void Awake()
    {
        if (_letterManager == null) _letterManager = this;
        else Destroy(this);
    }
    private void Start()
    {
        EventManager.LetterRecieved += CheckIfTheLetterIsInWord;
    }

    private void OnDestroy()
    {
        EventManager.LetterRecieved -= CheckIfTheLetterIsInWord;
    }


    ///<summary>
    /// Instantiates letter train box GameObjects
    /// and gets reference of every gameobjects in a list
    /// </summary>
    void StartLetterTrain()
    {
        letterTrainBoxesList = new List<GameObject>(maximumLetters);
        for (int i = 0; i < maximumLetters; i++)
        {
            InstantiateTrainBoxAndAddToTheList();
        }
    }

    /// <summary>
    /// Instantiates a new letter train box prefab and adds it to the list of train boxes.
    /// </summary>
    /// <returns>The instantiated letter train box game object.</returns>
    private GameObject InstantiateTrainBoxAndAddToTheList()
    {

        //instantiation of the gameObject to Add in the list
        GameObject lettertrainBox = Instantiate(letterTrainBoxPrefab, this.transform);
        //Set necessary references
        lettertrainBox.GetComponent<TrainBox>().SetPathReference(path);
        LetterBehaviour letterContainerBox = lettertrainBox.GetComponentInChildren<LetterBehaviour>();

        //Sets inactive incase they are active
        lettertrainBox.SetActive(false);
        letterContainerBox.gameObject.SetActive(true);

        //Added to the list
        letterTrainBoxesList.Add(lettertrainBox);

        return lettertrainBox;
    }
    /// <summary>
    /// Coroutine that sets the train on the track and handles the spawning of letter boxes on the train.
    /// </summary>
    IEnumerator SetTrainOnTrack()
    {
        // Instantiates the train head prefab and sets it on the track.
        Instantiate(trainHeadPrefab, this.transform).GetComponent<TrainHead>().SetTrainHeadOnTrack(path);
        yield return new WaitForSeconds(0.5f);

        // Continues the game loop until the game is completed.
        while (!_gameCompleted)
        {
            // Waits for a specified interval before checking the pool for available letter boxes.
            yield return new WaitForSeconds(armySpawnInterval);

            // Checks the pool for available letter boxes.
            CheckInPool();
        }


        // Local function to check the pool for available letter boxes and activate them.
        void CheckInPool()
        {
            // Iterates through the list of letter train boxes.
            for (int i = 0; i < letterTrainBoxesList.Count; i++)
            {
                // If the letter train box is not active, summon it and return from the function.
                if (!letterTrainBoxesList[i].activeInHierarchy)
                {
                    SummonLetterTrainBoxAndContainer(i);
                    return;
                }
            }
            // If no letter train box is available in the pool, instantiate a new one and activate it.
            ActivateGameObject(InstantiateTrainBoxAndAddToTheList());
        }

        // Local function to summon a letter train box and its container.
        void SummonLetterTrainBoxAndContainer(int letterArmyIndex)
        {
            ActivateGameObject(letterTrainBoxesList[letterArmyIndex]);
        }

        // Local function to activate a letter box game object.
        void ActivateGameObject(GameObject letterBox)
        {
            try
            {
                // Activates the letter box game object.
                letterBox.SetActive(true);

                // Gets a letter for the letter box from the all letters queue, or generates a random wrong letter if the queue is empty.
                char letterForArmy;

                //incase the m_allLetters gets empty
                //otherwise the m_allLetters char is taken
                if (m_allLetters.Count == 0) { letterForArmy = GetRandomWrongLetter(); }
                else letterForArmy = m_allLetters.Dequeue();
                //Set letter in the letter container i.e. child of train box
                LetterBehaviour letterBehaviour = letterBox.GetComponentInChildren<LetterBehaviour>();
                letterBehaviour.SetLetterCharacter(letterForArmy);
                letterBehaviour.MakeTheObjectUndrestroyed();
                letterBehaviour.gameObject.SetActive(true);
            }
            catch
            {
                // If there is an error, removes the letter box from the list, destroys it, and activates a new letter box.
                letterTrainBoxesList.Remove(letterBox);
                Destroy(letterBox);
                ActivateGameObject(InstantiateTrainBoxAndAddToTheList());

            }
        }
    }

    /// <summary>
    /// returns random wrong letters from wrongLetters array
    /// </summary>
    /// <returns></returns>
    char GetRandomWrongLetter()
    {
        return m_wrongLetters[Random.Range(0, m_wrongLetters.Length)];
    }

    bool CheckIfTheLetterIsInCorrectLetters(char letter)
    {
        foreach (char c in m_correctLetters) if (c == letter) return true;
        return false;
    }

    ///<summary>
    ///Sets characters of answer into arrat
    ///Creates an array with correct answer and incorrect answer sorted randomly
    ///then the array is made into a queue
    ///then coroutine starts for setting army one by one
    /// </summary>
    public void SetQuestionAndAnswer(string p_question, string p_answerWord, string p_wrongAnswerLetters)
    {
        //Instantiates letter train boxes
        StartLetterTrain();

        //sets all characters of answer word and wrongAnswerLetters
        SetCharactersInArray();
        SetOnQueue(CreateAnArrayWithCorrectAndWrongLetters());

        m_currentCoroutine = StartCoroutine(SetTrainOnTrack());

        //local function
         
        char[] CreateAnArrayWithCorrectAndWrongLetters()
        {
            var random = new System.Random();
            char[] tempCorrectLetter = new char[p_answerWord.Length];

            char[] allCharCollection = new char[maximumLetters];
            int l_index = 0;
            //inserting the correct letters first
            while (l_index < m_correctLetters.Length && l_index < maximumLetters)
            {
                tempCorrectLetter[l_index] = m_correctLetters[l_index];
                l_index++;
            }
            //randomly sorts the array
            tempCorrectLetter =tempCorrectLetter.OrderBy(x => random.Next()).ToArray();

            //passes the last char
            letterHolder.SetTheUILetterActive(tempCorrectLetter[l_index-1]);
            l_index--;
            //adds all the character to allCharCollection neglecting the last char
            for (int i = 0; i < tempCorrectLetter.Length-1; i++)
            {
                allCharCollection[i] = tempCorrectLetter[i];
            }
            //inserting all the remaining wrong letters until its full
            while (l_index < allCharCollection.Length)
            {
                char l_temLetter = GetRandomWrongLetter();
                allCharCollection[l_index] = l_temLetter;
                l_index++;
            }

            //sorting randomly
            char[] finalAllCharArray = new char[maximumLetters];
            finalAllCharArray = allCharCollection.OrderBy(x => random.Next()).ToArray();
            return finalAllCharArray;
        }
        void SetOnQueue(char[] letterSet)
        {
            foreach (char c in letterSet) m_allLetters.Enqueue(c);
        }

        //sets correctLetters array and wrongLettersArray with corresponding correct and incorrect letters from answerwords and wrongAnswerLetters
        void SetCharactersInArray()
        {
            m_wrongLetters = p_wrongAnswerLetters.ToCharArray();
            m_correctLetters = p_answerWord.ToCharArray();
            m_allLetters = new Queue<char>(maximumLetters);
        }
    }

    ///<summary>
    /// Sets LettersMovement to 0 to restart from the starting point of the path creator 
    /// gets the char from the letterGameObject and checks if the letter is present in the word or not
    /// if true then the letter is added to the queue
    /// else a new letter is added to the queue
    /// and finally the gameObject is disabled
    /// </summary>
    public void AddTrainBoxToTheList(GameObject p_trainBox,LetterBehaviour p_letterBehaviour)
    {
        //gets reference to the letter
        char l_letter = p_letterBehaviour.Letter;

        //checks if the letter recieved at the dead point was correct or not
        if (CheckIfTheLetterIsInCorrectLetters(l_letter))
        {
            //if the letter is not destroyed
            if (!p_letterBehaviour.destroyed)
            {
                //invokes the lhat the false letter hit 
                //because the player has missed the correct letter
                EventManager.CorrectLetterHit?.Invoke(false);

                //adds the character letter to the all letter list
                m_allLetters.Enqueue(l_letter);
            }
            else
            {
                //incase the correct letter has already been destroyed random incorrect or wrong letter is added to the list
                m_allLetters.Enqueue(GetRandomWrongLetter());
                p_letterBehaviour.gameObject.SetActive(true);
            }
            
        }
        else
        {
            //if the letter in the train box is incorrect, wrong letter gets added to the list
            m_allLetters.Enqueue(GetRandomWrongLetter());
        }
        //changes its position
        p_trainBox.transform.position = _objectOriginPoint.position;
        p_trainBox.SetActive(false);
    }

    ///<summary>
    ///Called After letter is recieved.
    ///Checks if the letter recieved by harpoon is correct or not.
    ///then action is played as per the answer's nature
    ///</summary>
    public void CheckIfTheLetterIsInWord(char c, LetterBehaviour letterBehaviour)
    {
        

        if (CheckIfTheLetterIsInCorrectLetters(c))
        {
            letterBehaviour.PlayCorrectLetterAction();
            letterHolder.SetTheUILetterActive(c);
            return;
        }
        letterBehaviour.PlayIncorrectLetterAction();
    }

    /// <summary>
    /// REmoves all train boxes by destroying them all
    /// </summary>
    public void RemoveAllTrainPartsFromTheScene()
    {
        StopCoroutine(m_currentCoroutine);
        foreach (GameObject gameObject in letterTrainBoxesList)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}

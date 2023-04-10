using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
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
    void StartLetterTrain()
    {

        ///<summary>
        /// * Instantiates letter army GameObjects
        /// * and gets reference of every gameobjects in a list
        /// </summary>
        letterTrainBoxesList = new List<GameObject>(maximumLetters);
        for (int i = 0; i < maximumLetters; i++)
        {
            InstantiateTrainBoxAndAddToTheList();
        }
    }

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

    IEnumerator SetTrainOnTrack()
    {

        Instantiate(trainHeadPrefab, this.transform).GetComponent<TrainHead>().SetTrainHeadOnTrack(path);
        yield return new WaitForSeconds(0.05f);

        while (!_gameCompleted)
        {
            yield return new WaitForSeconds(armySpawnInterval);
            CheckInPool();
        }


        //local functions
        void CheckInPool()
        {
            for (int i = 0; i < letterTrainBoxesList.Count; i++)
            {
                if (!letterTrainBoxesList[i].activeInHierarchy)
                {
                    SummonLetterTrainBoxAndContainer(i);
                    return;
                }
            }
            ActivateGameObject(InstantiateTrainBoxAndAddToTheList());
            maximumLetters++;
        }
        void SummonLetterTrainBoxAndContainer(int letterArmyIndex)
        {
            ActivateGameObject(letterTrainBoxesList[letterArmyIndex]);
        }
        void ActivateGameObject(GameObject letterBox)
            {
            try
            {
                letterBox.SetActive(true);
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
                letterTrainBoxesList.Remove(letterBox);
                Destroy(letterBox);
                ActivateGameObject(InstantiateTrainBoxAndAddToTheList());

            }
        }
    }

    char GetRandomWrongLetter()
    {
        return m_wrongLetters[Random.Range(0, m_wrongLetters.Length)];
    }

    bool CheckIfTheLetterIsInCorrectLetters(char letter)
    {
        foreach (char c in m_correctLetters) if (c == letter) return true;
        return false;
    }


    public void SetQuestionAndAnswer(string p_question, string p_answerWord, string p_wrongAnswerLetters)
    {
        //set question and answer
        ///<summary>
        ///
        ///Sets characters of answer into arrat
        ///Creates an array with correct answer and incorrect answer sorted randomly
        ///then the array is made into a queue
        ///then coroutine starts for setting army one by one
        /// </summary>
        StartLetterTrain();

        SetCharactersInArray();
        SetOnQueue(CreateAnArrayWithCorrectAndWrongLetters());

        m_currentCoroutine = StartCoroutine(SetTrainOnTrack());

        //local function
        char[] CreateAnArrayWithCorrectAndWrongLetters()
        {
            char[] allCharCollection = new char[maximumLetters];
            int l_index = 0;
            //inserting the correct letters first
            while (l_index < m_correctLetters.Length && l_index < maximumLetters)
            {
                allCharCollection[l_index] = m_correctLetters[l_index];
                l_index++;
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
            var random = new System.Random();
            finalAllCharArray = allCharCollection.OrderBy(x => random.Next()).ToArray();
            return finalAllCharArray;
        }
        void SetOnQueue(char[] letterSet)
        {
            foreach (char c in letterSet) m_allLetters.Enqueue(c);
        }
        void SetCharactersInArray()
        {
            m_wrongLetters = p_wrongAnswerLetters.ToCharArray();
            m_correctLetters = p_answerWord.ToCharArray();
            m_allLetters = new Queue<char>(maximumLetters);
        }
    }

    public void AddTrainBoxToTheList(GameObject p_trainBox,LetterBehaviour p_letterBehaviour)
    {
        ///<summary>
        ///
        /// * Sets LettersMovement to 0 to restart from the starting point of the path creator 
        ///*gets the char from the letterGameObject and checks if the letter is present in the word or not
        ///* if true then the letter is added to the queue
        ///* else a new letter is added to the queue
        ///*and finally the gameObject is disabled
        /// </summary>

        char l_letter = p_letterBehaviour.Letter;
        if (CheckIfTheLetterIsInCorrectLetters(l_letter))
        {
            if (!p_letterBehaviour.destroyed)
            {
                EventManager.CorrectLetterHit?.Invoke(false);
                m_allLetters.Enqueue(l_letter);
            }
            else
            {
                m_allLetters.Enqueue(GetRandomWrongLetter());
                p_letterBehaviour.gameObject.SetActive(true);
            }
            
        }
        else
        {
            m_allLetters.Enqueue(GetRandomWrongLetter());
        }
        p_trainBox.transform.position = _objectOriginPoint.position;
        p_trainBox.SetActive(false);
    }


    public void CheckIfTheLetterIsInWord(char c, LetterBehaviour letterBehaviour)
    {
        ///<summary>
        ///Called After letter is recieved
        ///
        /// 
        ///</summary>

        if (CheckIfTheLetterIsInCorrectLetters(c))
        {
            letterBehaviour.PlayCorrectLetterAction();
            letterHolder.SetTheUILetterActive(c);
            return;
        }
        letterBehaviour.PlayIncorrectLetterAction();
//        CameraEffects.Instance.OnLetterHit(false);
    }
    public void RemoveAllTrainPartsFromTheScene()
    {
        StopCoroutine(m_currentCoroutine);
        foreach (GameObject gameObject in letterTrainBoxesList)
        {
            Destroy(gameObject);
        }
    }

    #endregion
    #region TestingListRemove

    #endregion
}

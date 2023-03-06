using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LetterManager : MonoBehaviour
{
    private static LetterManager _letterManager;

    public static LetterManager letterManagerInstance
    {
        get
        {
            if(letterManagerInstance == null) FindObjectOfType<LetterManager>();
            return _letterManager;
        }
    }
    [SerializeField] PathCreator path;

    [SerializeField] GameObject letterCharactersPrefab;
    [SerializeField] List<GameObject> letterCharactersList;

    [SerializeField] string _word;
    public string Word
    {
        get => _word.ToUpper();
    }
    [SerializeField] int maximumLetters;
    [SerializeField] float armySpawnInterval;

    [SerializeField] LetterHolder letterHolder;
    char[] correctLetters;
    int correctLettersCount=0;

    Queue<char> allLetters;

    private void Awake()
    {
        EventManager.LetterRecieved += CheckIfTheLetterIsInWord;
    }
    private void Start()
    {
        SetCharactersInArray();
        InstantitateLetterArmy();
        SetOnQueue(CreateAnArrayWithCorrectAnswer());
        StartCoroutine(SetArmyOnTrack());
        letterHolder.AddLettersInLetterHolder(correctLetters);
    }

    private void InstantitateLetterArmy()
    {
        /*
         * Instantiates letter army GameObjects
         * and gets reference of every gameobjects in a list
         */
        letterCharactersList = new List<GameObject>(maximumLetters);
        for (int i = 0; i < maximumLetters; i++)
        {
            GameObject letterArmy = Instantiate(letterCharactersPrefab,transform);////--------------------------------------
            letterArmy.GetComponent<LetterMovement>().SetPathReference(path);

            letterArmy.SetActive(false);
            letterCharactersList.Add(letterArmy);
        }
    }

    void SetCharactersInArray()
    {
        correctLetters = Word.ToCharArray();
        allLetters = new Queue<char>(maximumLetters);
    }

    char GetPossibleLetters() {
        return System.Convert.ToChar(Random.Range('A', 'Z'+1));
    }

    public void CheckIfTheLetterIsInWord(char c, LetterBehaviour letterBehaviour)
    {
        foreach (char letter in correctLetters)
        {
            if(c== letter)
            {
                correctLettersCount++;
                letterBehaviour.OnCorrectLetter(letterHolder);
                return;
            }
        }
        letterBehaviour.OnWrongLetter();
    }

    IEnumerator SetArmyOnTrack()
    {
        while (correctLettersCount < correctLetters.Length)
        {
            yield return new WaitForSeconds(armySpawnInterval);
            CheckInPool();
        }
    }
    public void CheckInPool()
    {
        for (int i = 0; i < letterCharactersList.Count; i++)
        {
            if (!letterCharactersList[i].activeInHierarchy)
            {
                SummonLetterArmy(i);
                break;
            }
        }
    }
    private void SummonLetterArmy(int letterArmyIndex)
    {
        letterCharactersList[letterArmyIndex].SetActive(true);
        char letterForArmy;
        if (allLetters.Count == 0) { letterForArmy = GetRandomLetterNotInString(Word); }
        else letterForArmy = allLetters.Dequeue();
        letterCharactersList[letterArmyIndex].GetComponent<LetterBehaviour>().SetLetterCharacter(letterForArmy);
    }

    #region TestingListRemove
    /*private void RemoveFromList()
    {
        List<char> wordList = new List<char>(4);
        wordList.Add('a');
        wordList.Add('b');
        wordList.Add('c');
        wordList.Add('d');
        foreach (char c in wordList) print(c);
        print(wordList.Count);
        wordList.Remove('d');
        foreach (char c in wordList) print(c);
        print(wordList.Count);
    }*/
    #endregion

    char[] CreateAnArrayWithCorrectAnswer()
    {
        char[] allCharCollection = new char[maximumLetters];
        int l_index = 0;
        while (l_index < correctLetters.Length && l_index < maximumLetters)
        {
            allCharCollection[l_index] = correctLetters[l_index];
            l_index++;
        }
        while (l_index < allCharCollection.Length)
        {
            char l_temLetter = GetLetter();
            allCharCollection[l_index] = l_temLetter;
            l_index++;
        }
        char[] finalAllCharArray = new char[maximumLetters];
        var random = new System.Random();
        finalAllCharArray = allCharCollection.OrderBy(x => random.Next()).ToArray();
        return finalAllCharArray;

        
    }

    char GetRandomLetterNotInString(string input)
    {
        var rand = new System.Random();
        char letter;
        do
        {
            letter = (char)('A' + rand.Next(26));
        } while (input.Contains(letter.ToString()));
        return letter;
    }

    void SetOnQueue(char[] letterSet)
    {
        foreach(char c in letterSet) allLetters.Enqueue(c);
    }

    bool CheckIfTheLetterIsInCorrectLetters(char letter)
    {
        foreach (char c in correctLetters) if (c == letter) return true;
        return false;
    }

    char GetLetter()
    {
        /*
         * makes a local variable of char
         * then gets possible letter again and again until the letter is not present in word
         * and returns the letter
         */
        char l_temLetter;
        do
        {
            l_temLetter = GetPossibleLetters();
        } while ((CheckIfTheLetterIsInCorrectLetters(l_temLetter)));
        return l_temLetter;
    }

    public void AddLetterObjectToTheList(GameObject gameObject)
    {
        /*
         * Sets LettersMovement to 0 to restart from the starting point of the path creator 
         * gets the char from the letterGameObject and checks if the letter is present in the word or not
         * if true then the letter is added to the queue
         * else a new letter is added to the queue
         * and finally the gameObject is disabled
         */
        char l_letter = gameObject.GetComponent<LetterBehaviour>().Letter;
        if (CheckIfTheLetterIsInCorrectLetters(l_letter)) allLetters.Enqueue(l_letter);
        else allLetters.Enqueue(GetLetter());

        gameObject.SetActive(false);
    }
}

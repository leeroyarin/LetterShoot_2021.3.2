using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    [SerializeField] TextAsset quizAsset;


    public static QuizData quiz;

    private void Awake()
    {
        if (quiz == null)
        {
            quiz = JsonUtility.FromJson<QuizData>(quizAsset.text);
        }
    }

    public static QuestionSet[] GetQuestionsByLevelAndIndex(int l_levelNumber)
    {
        QuestionSet[] l_level;
        switch (l_levelNumber)
        {
            case 1:
                l_level = quiz.Level1;
                break;
            case 2:
                l_level = quiz.Level2;
                break;
            case 3:
                l_level = quiz.Level3;
                break;
            case 4:
                l_level = quiz.Level4;
                break;
            case 5:
                l_level = quiz.Level5;
                break;
            case 6:
                l_level = quiz.Level6;
                break;
            case 7:
                l_level = quiz.Level7;
                break;
            case 8:
                l_level = quiz.Level8;
                break;
            case 9:
                l_level = quiz.Level9;
                break;
            case 10:
                l_level = quiz.Level10;
                break;
            default:
                Debug.Log("ERROR");
                return quiz.Level1;
        }
        return l_level;
    }
}

[System.Serializable]
public class QuestionSet
{
    public int QuestionNo;
    public string Question;
    public string AnswerWord;
    public int AnswerLetters;
    public string IncorrectOption;
}
[System.Serializable]
public class QuizData
{
    public QuestionSet[] Level1;
    public QuestionSet[] Level2;
    public QuestionSet[] Level3;
    public QuestionSet[] Level4;
    public QuestionSet[] Level5;
    public QuestionSet[] Level6;
    public QuestionSet[] Level7;
    public QuestionSet[] Level8;
    public QuestionSet[] Level9;
    public QuestionSet[] Level10;

    
}

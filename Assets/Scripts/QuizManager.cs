using UnityEngine;

// This script manages the quiz questions and provides methods to get questions by level and index.
// It loads a TextAsset containing quiz data in JSON format and deserializes it into QuizData object.
// It also provides static access to QuizData object through the quiz variable.
public class QuizManager : MonoBehaviour
{
    [SerializeField] TextAsset quizAsset;

    // A static reference to the QuizData object
    public static QuizData quiz;

    private void Awake()
    {
        if (quiz == null)
        {
            // Deserialize the JSON data in quizAsset into QuizData object if it hasn't already been done
            quiz = JsonUtility.FromJson<QuizData>(quizAsset.text);
        }
    }

    /// <summary>
    /// Gets an array of QuestionSet objects by level number.
    /// </summary>
    /// <param name="l_levelNumber">The level number of questions to get.</param>
    /// <returns>An array of QuestionSet objects for the given level number.</returns>
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
                return quiz.Instruction;
        }
        return l_level;
    }
}
#region Json
[System.Serializable]
public class QuestionSet
{
    public string Question;
    public string AnswerWord;
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
    public QuestionSet[] Instruction;

    
}
#endregion
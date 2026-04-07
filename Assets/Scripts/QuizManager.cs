using System.Collections.Generic;
using UnityEngine;

// This script manages the quiz questions and provides methods to get questions by level and index.
// It now fetches data from GameContentManager instead of loading from a JSON TextAsset.
// It provides static access to get questions by level through the GetQuestionsByLevelAndIndex method.
public class QuizManager : MonoBehaviour
{
    /// <summary>
    /// Gets an array of QuestionSet objects by level number.
    /// Level 0 is treated as "Instruction" level (tutorial/intro)
    /// </summary>
    /// <param name="l_levelNumber">The level number of questions to get.</param>
    /// <returns>An array of QuestionSet objects for the given level number.</returns>
    public static QuestionSet[] GetQuestionsByLevelAndIndex(int l_levelNumber)
    {
        // Get questions from GameContentManager
        if (GameContentManager.Instance == null)
        {
            Debug.LogError("GameContentManager.Instance is null! Make sure it's initialized before calling QuizManager.");
            return new QuestionSet[0];
        }

        List<QuestionData> questionDataList = GameContentManager.Instance.GetQuestionsForLevel(l_levelNumber);
        
        if (questionDataList == null || questionDataList.Count == 0)
        {
            Debug.LogWarning($"No questions found for level {l_levelNumber}");
            return new QuestionSet[0];
        }

        // Convert QuestionData to QuestionSet
        return ConvertToQuestionSet(questionDataList);
    }

    /// <summary>
    /// Converts a list of QuestionData to an array of QuestionSet
    /// </summary>
    private static QuestionSet[] ConvertToQuestionSet(List<QuestionData> questionDataList)
    {
        QuestionSet[] questionSets = new QuestionSet[questionDataList.Count];
        
        for (int i = 0; i < questionDataList.Count; i++)
        {
            questionSets[i] = new QuestionSet
            {
                Question = questionDataList[i].Question,
                AnswerWord = questionDataList[i].AnswerWord,
                IncorrectOption = questionDataList[i].IncorrectOption
            };
        }
        
        return questionSets;
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
#endregion
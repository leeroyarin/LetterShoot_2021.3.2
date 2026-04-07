using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameContentManager : MonoBehaviour
{
    public static GameContentManager Instance { get; private set; }

    // Publicly accessible data container
    public GameDataContainer LoadedData = new GameDataContainer();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Fetches all levels, then for each level fetches questions.
    /// Calls onComplete when everything is finished.
    /// </summary>
    public void InitializeGameData(Action onComplete)
    {
        StartCoroutine(FetchAllDataRoutine(onComplete));
    }

    private IEnumerator FetchAllDataRoutine(Action onComplete)
    {
        Debug.Log("Starting Game Data Fetch...");
        bool levelsFetched = false;
        string errorMsg = "";

        // 1. Fetch Levels
        NetworkManager.Instance.GetLevels((levels) =>
        {
            LoadedData.AllLevels = levels;
            levelsFetched = true;
        }, 
        (error) => 
        {
            errorMsg = error;
            levelsFetched = true; // Set true to break wait loop, but error is recorded
        });

        // Wait for levels response
        while (!levelsFetched) yield return null;

        if (!string.IsNullOrEmpty(errorMsg))
        {
            Debug.LogError($"Failed to fetch levels: {errorMsg}");
            // Even if levels fail, we might want to callback or retry. For now, we stop.
            yield break;
        }

        Debug.Log($"Fetched {LoadedData.AllLevels.Count} levels. Fetching all questions...");

        // 2. Fetch ALL Questions at once using the new endpoint
        bool questionsFetched = false;
        errorMsg = "";

        NetworkManager.Instance.GetAllQuestionsByLevel((response) =>
        {
            // Populate the dictionary from the response
            LoadedData.PopulateQuestionsFromResponse(response);
            questionsFetched = true;
        },
        (error) =>
        {
            errorMsg = error;
            questionsFetched = true;
        });

        // Wait for questions response
        while (!questionsFetched) yield return null;

        if (!string.IsNullOrEmpty(errorMsg))
        {
            Debug.LogError($"Failed to fetch questions: {errorMsg}");
            yield break;
        }

        Debug.Log("All Game Data Fetched.");
        onComplete?.Invoke();
    }

    /// <summary>
    /// Helper to get questions for a specific level ID
    /// </summary>
    public List<QuestionData> GetQuestionsForLevel(int levelNumber)
    {
        if (LoadedData.QuestionsByLevel.ContainsKey(levelNumber))
        {
            return LoadedData.QuestionsByLevel[levelNumber];
        }
        return new List<QuestionData>();
    }
}

[Serializable]
public class GameDataContainer
{
    public List<Level> AllLevels = new List<Level>();
    // Dictionary is not serializable by Unity inspector by default, but useful for runtime access
    public Dictionary<int, List<QuestionData>> QuestionsByLevel = new Dictionary<int, List<QuestionData>>();

    /// <summary>
    /// Populates the QuestionsByLevel dictionary from AllQuestionsResponse
    /// </summary>
    public void PopulateQuestionsFromResponse(AllQuestionsResponse response)
    {
        QuestionsByLevel = new Dictionary<int, List<QuestionData>>();

        if (response.Level0 != null) QuestionsByLevel[0] = new List<QuestionData>(response.Level0);
        if (response.Level1 != null) QuestionsByLevel[1] = new List<QuestionData>(response.Level1);
        if (response.Level2 != null) QuestionsByLevel[2] = new List<QuestionData>(response.Level2);
        if (response.Level3 != null) QuestionsByLevel[3] = new List<QuestionData>(response.Level3);
        if (response.Level4 != null) QuestionsByLevel[4] = new List<QuestionData>(response.Level4);
        if (response.Level5 != null) QuestionsByLevel[5] = new List<QuestionData>(response.Level5);
        if (response.Level6 != null) QuestionsByLevel[6] = new List<QuestionData>(response.Level6);
        if (response.Level7 != null) QuestionsByLevel[7] = new List<QuestionData>(response.Level7);
        if (response.Level8 != null) QuestionsByLevel[8] = new List<QuestionData>(response.Level8);
        if (response.Level9 != null) QuestionsByLevel[9] = new List<QuestionData>(response.Level9);
        if (response.Level10 != null) QuestionsByLevel[10] = new List<QuestionData>(response.Level10);

        Debug.Log($"Populated questions for {QuestionsByLevel.Count} levels");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    private const string BaseUrl = "http://localhost/Letter-Shooter/api/index.php";

    public static NetworkManager Instance { get; private set; }

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

    // Example Start method to demonstrate usage structure or kick off initial checks
    // private void Start() {
    //     StartCoroutine(GetLevelsExample());
    // }

    #region Public API Methods

    public void Login(string username, string password, Action<LoginResponseData> onSuccess, Action<string> onError)
    {
        StartCoroutine(LoginRoutine(username, password, onSuccess, onError));
    }

    public void Register(string username, string firstName, string lastName, string password, Action onSuccess, Action<string> onError)
    {
        StartCoroutine(RegisterRoutine(username, firstName, lastName, password, onSuccess, onError));
    }

    public void GetQuestions(int level, Action<List<QuestionData>> onSuccess, Action<string> onError)
    {
        StartCoroutine(GetQuestionsRoutine(level, onSuccess, onError));
    }

    public void GetLevels(Action<List<Level>> onSuccess, Action<string> onError)
    {
        StartCoroutine(GetLevelsRoutine(onSuccess, onError));
    }

    public void GetPlayerLevel(Action<int> onSucess, Action<string> onError)
    {
        print("Getting Player Level from Server...");
        StartCoroutine(GetPlayerCurrentLevelRoutine(onSucess, onError));
    }

    public void SetPlayerLevel(int level,Action<bool> onSucess, Action<string> onError)
    {
        StartCoroutine(SetPlayerLevelRoutine(level,onSucess,onError));
    }

    public void GetAllQuestionsByLevel(Action<AllQuestionsResponse> onSuccess, Action<string> onError)
    {
        StartCoroutine(GetAllQuestionsByLevelRoutine(onSuccess, onError));
    }

    private IEnumerator SetPlayerLevelRoutine(int level, Action<bool> onSucess, Action<string> onError)
    {
        string url = BaseUrl + "?request=setLevel";
        var requestData = new SetLevelRequest { level = level };

        using (var request = CreateRequest(url, RequestType.POST, requestData))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var response = JsonUtility.FromJson<BasicResponse>(request.downloadHandler.text);
                    if (response != null && response.success)
                    {
                        onSucess?.Invoke(true);
                    }
                    else if (response != null)
                    {
                        onError?.Invoke(string.IsNullOrEmpty(response.message) ? "Failed to update level." : response.message);
                    }
                    else
                    {
                        onError?.Invoke("Invalid server response when setting player level.");
                    }
                }
                catch (Exception ex)
                {
                    onError?.Invoke("Failed to parse server response: " + ex.Message);
                }
            }
            else
            {
                // Attempt to parse server-provided error message before falling back to protocol error
                try
                {
                    var response = JsonUtility.FromJson<BasicResponse>(request.downloadHandler.text);
                    if (response != null && !string.IsNullOrEmpty(response.message))
                        onError?.Invoke(response.message);
                    else
                        onError?.Invoke(request.error);
                }
                catch
                {
                    onError?.Invoke(request.error);
                }
            }
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator LoginRoutine(string username, string password, Action<LoginResponseData> onSuccess, Action<string> onError)
    {
        string url = BaseUrl + "?request=login";
        var requestData = new LoginRequest { username = username, password = password };
        
        using (var request = CreateRequest(url, RequestType.POST, requestData))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                if (response.success)
                {
                    onSuccess?.Invoke(response.data);
                }
                else
                {
                    onError?.Invoke(response.message);
                }
            }
            else
            {
                // Try to parse error message from server if available, otherwise use protocol error
                try
                {
                    var response = JsonUtility.FromJson<BasicResponse>(request.downloadHandler.text);
                    if (response != null && !string.IsNullOrEmpty(response.message))
                    {
                        onError?.Invoke(response.message);
                    }
                    else
                    {
                        onError?.Invoke(request.error);
                    }
                }
                catch
                {
                    onError?.Invoke(request.error);
                }
            }
        }
    }

    private IEnumerator RegisterRoutine(string username, string firstName, string lastName, string password, Action onSuccess, Action<string> onError)
    {
        string url = BaseUrl + "?request=register";
        var requestData = new RegisterRequest 
        { 
            username = username, 
            firstName = firstName, 
            lastName = lastName, 
            password = password 
        };

        using (var request = CreateRequest(url, RequestType.POST, requestData))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<BasicResponse>(request.downloadHandler.text);
                if (response.success)
                {
                    onSuccess?.Invoke();
                }
                else
                {
                    onError?.Invoke(response.message);
                }
            }
            else
            {
                try
                {
                    var response = JsonUtility.FromJson<BasicResponse>(request.downloadHandler.text);
                    if (response != null && !string.IsNullOrEmpty(response.message))
                        onError?.Invoke(response.message);
                    else
                        onError?.Invoke(request.error);
                }
                catch
                {
                    onError?.Invoke(request.error);
                }
            }
        }
    }

    private IEnumerator GetQuestionsRoutine(int level, Action<List<QuestionData>> onSuccess, Action<string> onError)
    {
        string url = BaseUrl + "?request=getQuestions&level=" + level;

        using (var request = CreateRequest(url, RequestType.GET))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Since data is a list inside the object
                var response = JsonUtility.FromJson<GetQuestionsResponse>(request.downloadHandler.text);
                if (response.success)
                {
                    onSuccess?.Invoke(response.data);
                }
                else
                {
                    onError?.Invoke("Failed to retrieve questions.");
                }
            }
            else
            {
                onError?.Invoke(request.error);
            }
        }
    }

    private IEnumerator GetLevelsRoutine(Action<List<Level>> onSuccess, Action<string> onError)
    {
        string url = BaseUrl + "?request=getLevels";

        using (var request = CreateRequest(url, RequestType.GET))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<GetLevelsResponse>(request.downloadHandler.text);
                if (response.success)
                {
                    onSuccess?.Invoke(response.data);
                }
                else
                {
                    onError?.Invoke("Failed to retrieve levels.");
                }
            }
            else
            {
                onError?.Invoke(request.error);
            }
        }
    }

    private IEnumerator GetPlayerCurrentLevelRoutine(Action<int> onSuccess, Action<string> onError)
    {
        string url = BaseUrl + "?request=getPlayerLevel";
        using (var request = CreateRequest(url, RequestType.GET))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Assuming the response contains a field "CurrentLevel"
                var response = JsonUtility.FromJson<CurrentLevelResponse>(request.downloadHandler.text);
                if (response.success)
                {
                    onSuccess?.Invoke(response.data.LevelNumber);
                }
                else
                {
                    onError?.Invoke("Failed to retrieve player level.");
                }
            }
            else
            {
                onError?.Invoke(request.error);
            }
        }
    }

    private IEnumerator GetAllQuestionsByLevelRoutine(Action<AllQuestionsResponse> onSuccess, Action<string> onError)
    {
        string url = BaseUrl + "?request=getAllQuestionsByLevel";
        using (var request = CreateRequest(url, RequestType.GET))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var response = JsonUtility.FromJson<AllQuestionsResponse>(request.downloadHandler.text);
                    if (response != null)
                    {
                        onSuccess?.Invoke(response);
                    }
                    else
                    {
                        onError?.Invoke("Failed to parse all questions response.");
                    }
                }
                catch (Exception ex)
                {
                    onError?.Invoke("Failed to parse server response: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    var response = JsonUtility.FromJson<BasicResponse>(request.downloadHandler.text);
                    if (response != null && !string.IsNullOrEmpty(response.message))
                        onError?.Invoke(response.message);
                    else
                        onError?.Invoke(request.error);
                }
                catch
                {
                    onError?.Invoke(request.error);
                }
            }
        }
    }

    #endregion

    #region Helper Methods

    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    private void AttachHeader(UnityWebRequest request, string key, string value)
    {
        request.SetRequestHeader(key, value);
    }

 

    #endregion
}

public enum RequestType
{
    GET = 0,
    POST = 1,
    PUT = 2
}

#region Data Classes

[Serializable]
public class LoginRequest
{
    public string username;
    public string password;
}

[Serializable]
public class RegisterRequest
{
    public string username;
    public string firstName;
    public string lastName;
    public string password;
}

[Serializable]
public class SetLevelRequest
{
    public int level;
}

[Serializable]
public class BasicResponse
{
    public bool success;
    public string message;
}

[Serializable]
public class LoginResponse
{
    public bool success;
    public string message;
    public LoginResponseData data;
}

[Serializable]
public class CurrentLevelResponse
{
       public bool success;
    public string message;
    public Level data;
}

[Serializable]
public class LoginResponseData
{
    public string Username;
    public string FirstName;
    public string LastName;
    public int Role_ID;
    public int CurrentLevel;
    public string RoleName;
}

[Serializable]
public class Question
{
    public int ID;
    public string QuestionText; // JSON says "Question" but might collide with class name if usage is tricky, but field name must match JSON.
                                // Actually, I should check the JSON field name. It is "Question".
                                // So I must name the field "Question".
    // However, C# allows field name same as class name? No, not really recommended if class was named Question.
    // Spec says: "Question": "_____ means to..."
    // Class name is Question.
    // public string Question; // Error: Member 'Question' cannot be the same as the enclosing type
    // Workaround: Name the class QuestionData or similar.
}

[Serializable]
public class QuestionData
{
    public int ID;
    public string Question;
    public string AnswerWord;
    public string IncorrectOption;
}

[Serializable]
public class GetQuestionsResponse
{
    public bool success;
    public List<QuestionData> data;
}

[Serializable]
public class Level
{
    public int ID;
    public int LevelNumber;
    public string Difficulty;
}

[Serializable]
public class GetLevelsResponse
{
    public bool success;
    public List<Level> data;
}

[Serializable]
public class AllQuestionsResponse
{
    public QuestionData[] Level0;
    public QuestionData[] Level1;
    public QuestionData[] Level2;
    public QuestionData[] Level3;
    public QuestionData[] Level4;
    public QuestionData[] Level5;
    public QuestionData[] Level6;
    public QuestionData[] Level7;
    public QuestionData[] Level8;
    public QuestionData[] Level9;
    public QuestionData[] Level10;
}

#endregion

using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoginRegisterManager : MonoBehaviour
{
    [Header("Login UI")]
    public GameObject loginUIPanel;
    public TMP_InputField loginUsernameInput;
    public TMP_InputField loginPasswordInput;
    public Button loginButton;
    public Button goToRegisterButton;
    public TextMeshProUGUI loginStatusText;

    public UnityEvent OnLoginSuccessEvent;

    [Header("Register UI")]
    public GameObject registerPanel;
    public TMP_InputField registerUsernameInput;
    public TMP_InputField registerFirstNameInput;
    public TMP_InputField registerLastNameInput;
    public TMP_InputField registerPasswordInput;
    public Button registerButton;
    public Button backToLoginButton;
    public TextMeshProUGUI registerStatusText;

    public UnityEvent OnRegisterSuccessEvent;

    private void Start()
    {
        // Initialize UI states
        ShowLogin();

        // Add listeners
        loginButton.onClick.AddListener(OnLoginClicked);
        goToRegisterButton.onClick.AddListener(ShowRegister);
        
        registerButton.onClick.AddListener(OnRegisterClicked);
        backToLoginButton.onClick.AddListener(ShowLogin);
    }

    private void ShowLogin()
    {
        loginUIPanel.SetActive(true);
        registerPanel.SetActive(false);
        if(loginStatusText) loginStatusText.text = "";
    }

    private void ShowRegister()
    {
        loginUIPanel.SetActive(false);
        registerPanel.SetActive(true);
        if(registerStatusText) registerStatusText.text = "";
    }

    private void OnLoginClicked()
    {
        string username = loginUsernameInput.text;
        string password = loginPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            if(loginStatusText) loginStatusText.text = "Please enter username and password.";
            return;
        }

        if(loginStatusText) loginStatusText.text = "Logging in...";
        loginButton.interactable = false;

        NetworkManager.Instance.Login(username, password, OnLoginSuccess, OnLoginError);
    }

    private void OnRegisterClicked()
    {
        string username = registerUsernameInput.text;
        string fName = registerFirstNameInput.text;
        string lName = registerLastNameInput.text;
        string password = registerPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            if(registerStatusText) registerStatusText.text = "Username and Password are required.";
            return;
        }

        if(registerStatusText) registerStatusText.text = "Registering...";
        registerButton.interactable = false;

        NetworkManager.Instance.Register(username, fName, lName, password, OnRegisterSuccess, OnRegisterError);
    }

    private void OnLoginSuccess(LoginResponseData data)
    {
        loginButton.interactable = true;
        if(loginStatusText) loginStatusText.text = "Login Successful!";
        Debug.Log("Login Successful!" + data.FirstName  );
        Debug.Log($"User {data.Username} logged in. Role: {data.RoleName}");

        OnLoginSuccessEvent?.Invoke();
        // Trigger GameContentManager to fetch data
        if (GameContentManager.Instance != null)
        {
            if(loginStatusText) loginStatusText.text = "Fetching Game Data...";
            GameContentManager.Instance.InitializeGameData(() => 
            {
                // Proceed to next scene or game flow
                if(loginStatusText) loginStatusText.text = "Ready!";
                Debug.Log("Game Data Loaded. Ready to start.");
                // SceneManager.LoadScene("LevelSelect"); // Uncomment and set scene name
            });
        }
        else
        {
            Debug.LogWarning("GameContentManager instance not found.");
        }
    }

    private void OnLoginError(string error)
    {
        loginButton.interactable = true;
        if(loginStatusText) loginStatusText.text = $"Error: {error}";
    }

    private void OnRegisterSuccess()
    {
        registerButton.interactable = true;
        if(registerStatusText) registerStatusText.text = "Registration Successful! Please Login.";
        // Optionally switch back to login automatically
        OnRegisterSuccessEvent?.Invoke();
    }

    private void OnRegisterError(string error)
    {
        registerButton.interactable = true;
        if(registerStatusText) registerStatusText.text = $"Error: {error}";
    }
}

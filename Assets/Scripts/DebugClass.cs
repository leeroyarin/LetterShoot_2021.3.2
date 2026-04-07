using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class DebugClass: MonoBehaviour
{
    public async void Start()
    {
        NetworkManager.Instance.GetAllQuestionsByLevel(
            allQuestions => Debug.Log("Success"),
            error => Debug.Log("Error" + error)
        );
    }
}


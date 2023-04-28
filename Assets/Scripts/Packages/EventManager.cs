using System;
using System.Collections.Generic;
using UnityEngine;

public  class EventManager : MonoBehaviour
{
    public static Action<char, LetterBehaviour> LetterRecieved;
    public static Action wordCompleted;
    public static Action<bool> GameCompleted;
    public static Action EraseAllLetters;
    public static Action<bool> CorrectLetterHit;
    #region oldCode
    /*
        //Creating a Directory that uses string and Actions as parameter
        private Dictionary<string, Action<EventParam>> eventDictionary;

        private static EventManager _eventManager;

        public static EventManager Instance
        {
            get
            {
                if (!_eventManager)
                {
                    _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                    if (!_eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                    }
                    else
                    {
                        _eventManager.Init();
                    }
                }
                return _eventManager;
            }
        }

        void Init()
        {
            if (eventDictionary != null)
            {
                return;
            }
            eventDictionary = new Dictionary<string, Action<EventParam>>();
        }

        public static void StartListening(string eventName, Action<EventParam> listener)
        {
            if (Instance.eventDictionary.TryGetValue(eventName, out Action<EventParam> thisEvent))
            {
                //Add more event to the existing one
                thisEvent += listener;

                //Update the Dictionary
                Instance.eventDictionary[eventName] = thisEvent;
            }
            else
            {
                //Add event to the Dictionary for the first time
                thisEvent += listener;
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<EventParam> listener)
        {
            if (_eventManager == null) return;
            Action<EventParam> thisEvent;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Remove event from the existing one
                thisEvent -= listener;

                //Update the Dictionary
                Instance.eventDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, EventParam eventParam)
        {
            Action<EventParam> thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(eventParam);
                // OR USE  instance.eventDictionary[eventName](eventParam);
            }
        }*/
    #endregion
}

/*//Re-usable structure/ Can be a class to. Add all parameters you need inside it
public struct EventParam
{
    public string param1;
    public int param2;
    public float param3;
    public bool param4;
}*/


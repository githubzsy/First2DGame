using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    //这个dic用来管理需要监听的各类事件
    private Dictionary<string, UnityEvent> _eventDictionary;

    private static EventManager _eventManager;
    //单例
    public static EventManager Instance
    {
        get
        {
            if (!_eventManager)
            {
                //在Unity的Hierarchy中必须有一个名为EventManager的空物体,并挂上EventManager脚本
                _eventManager = FindObjectOfType<EventManager>();

                if (!_eventManager)
                {
                    Debug.LogError("需要挂载EventManager的一个物体");
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
        if (_eventDictionary == null)
        {
            _eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    //在需要监听某个事件的脚本中，调用这个方法来监听这个事件
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance._eventDictionary.Add(eventName, thisEvent);
        }
    }

    //在不需要监听的时候停止监听
    public static void StopListening(string eventName, UnityAction listener)
    {
        if (_eventManager == null) return;
        UnityEvent thisEvent = null;
        if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    //触发某个事件
    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
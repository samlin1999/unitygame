using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CheckerType { Event, Teleport, StateComplete}

public class ZoneChecker : MonoBehaviour {
    [Header("觸發事件設定")]
    public UnityEvent events;
    [Header("檢測類型設定")]
    public CheckerType checkerType;
    public bool once = true;
    bool active = true;
    Transform target;
    [Header("傳送事件設定")]
    public Transform teleportPoint;
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            target = other.transform;
            Action();//觸發行動
        }
    }
    
    void Action()
    {
        if (!active)
            return;
        switch (checkerType)
        {
            case CheckerType.Event:
                events.Invoke();//執行所有事件內的連結功能
                break;
            case CheckerType.Teleport:
                if (teleportPoint)
                    target.position = teleportPoint.position;
                break;
            case CheckerType.StateComplete:
                GameManager.ctrl.BackToMenu();
                break;
        }
        if (once) active = false;
    }
    
}

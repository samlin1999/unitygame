using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEvent : MonoBehaviour
{
    public string eventName;
    public ZoneChecker zoneChecker;
    public Func<bool> func;//func<回傳類型.引數>，類似Action

    public void MossionCompleteCheck()
    {
        bool allDone = true;
        foreach (Func<bool> val in func.GetInvocationList())//<---許多任務中找要判斷的任務
        {
            if (!val())
            {
                allDone = false; 
                break;
            }
        }
        zoneChecker.enabled = allDone;//勾勾開關
    }
}

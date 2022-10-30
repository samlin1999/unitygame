using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSystem : ISystem
{
    public List<MissionEvent> missionEvents;
    /// <summary>
    /// 自動抓取所有有MissionEvent的物件放入list
    /// </summary>
    public MissionSystem()
    {
        missionEvents = new List<MissionEvent>(Object.FindObjectsOfType<MissionEvent>());
    }

    public override void Inital()
    {
        Debug.Log(missionEvents.Count);
    }
    /// <summary>
    /// 比對任務資訊，判斷是否抓取成功
    /// </summary>
    /// <param name="task"></param>
    public void FindMissionEvent(Task task)
    {
        Debug.Log(task.eventName);
        foreach (MissionEvent ME in missionEvents)
        {
            if (ME.eventName == task.eventName)
            {
                ME.func += task.Complete; Debug.Log("GOT");
            }
            else
            {
                Debug.Log("NO");
            }
        }
    }
    /// <summary>
    /// 檢查任務是否完成
    /// </summary>
    public void CheckMission()
    {
        foreach (MissionEvent ME in missionEvents)
        {
            ME.MossionCompleteCheck();
        }
    }
}

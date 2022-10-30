using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class taskcontent : MonoBehaviour {

    public Text header;
    public conditionContent conditionTMP;
    public List<conditionContent> conditionList = new List<conditionContent>();//將腳本實體化
	void Start () {
		
	}
    /// <summary>
    /// 任務標題呈現
    /// </summary>
    /// <param name="task"></param>
	public void SetTask(Task task)
    {
        header.text = task.taskName;
        conditionContent obj = null;
        for (int i = 0; i < task.conditionList.Count; i++)
        {
            obj = (conditionContent)Instantiate(conditionTMP, transform);
            obj.gameObject.SetActive(true);
            task.conditionList[i].SetTmpUI(obj);
            //obj.condition.Update();
            //conditionList.Add(obj);
            //obj.Setcondition(task.conditionList[i]);
        }
    }
    /// <summary>
    /// 任務完成的文字呈現
    /// </summary>
    /// <param name="B"></param>
    public void TaskUpdate(bool B)
    {
        header.text += B ? " (完成)" : "";
        GameManager.ctrl.missionSystem.CheckMission();
    }

}

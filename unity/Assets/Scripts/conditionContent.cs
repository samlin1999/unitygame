using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class conditionContent : MonoBehaviour {

    public Text content;
    public Image checkicon;
    public Condition condition;
    public Sprite sussece;
	void Start () {
		
	}
	/// <summary>
    /// 任務進行呈現
    /// </summary>
    /// <param name="condition"></param>
	public void Setcondition(Condition C)
    {
        condition = C;
        content.text = condition.conditionDes + condition.CompleteCount();
        
    }
    /// <summary>
    /// 更新任務進度
    /// </summary>
    public void ConditionUpdate()
    {
        //condition.Update();
        content.text = condition.conditionDes + condition.CompleteCount();
        if(condition.Complete())
        {
            checkicon.sprite = sussece;
            content.color = Color.gray;
        }
    }
}

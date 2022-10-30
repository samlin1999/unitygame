using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillGroup : MonoBehaviour {
    public Text levelText;
    public List<SkillData> skillData = new List<SkillData>(6);
	void Start () {
		
	}
	
	void Update () {
		
	}
    public void setData(SkillGroupinfo SGI)
    {
        gameObject.SetActive(true);
        for(int i = 0;i < SGI.skillInfos.Count;i++)
        {
            levelText.text = "Level " + SGI.level.ToString("00");//ToString訂定格式
            skillData[i].setData(SGI.skillInfos[i]);
        }
    }
}

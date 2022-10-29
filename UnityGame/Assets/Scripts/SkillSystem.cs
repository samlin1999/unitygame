using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 紀錄技能設定
/// </summary>
[System.Serializable]
public class SkillToggleData
{
    public Image image;
    public int skillID;
    public Text Text;
}

public class SkillSystem : MonoBehaviour {
    public static SkillSystem ctrl;
    public SkillDB skillDB;//DataBase
    public Transform skillContent;//產生技能圖示的UI位置
    public SkillData skillDataTMP;//技能圖示裝載的模板(用來裝資料庫中的技能設定)
    public  List<SkillToggleData> skillToggles = new List<SkillToggleData>(3);
    public int index = 0;
    public Sprite defaultIcon;

    private void Awake()
    {
        ctrl = this;
    }
    //用Start改
    public void Initial()
    {
		for(int i = 0;i < skillDB.skillInfos.Count;i++)
        {
            Instantiate(skillDataTMP, skillContent).setData(skillDB.skillInfos[i]);
        }
        //驗證存檔
        if (GameData.ctrl.skillToggles.Count == 0) return;
        for(int j = 0;j < skillToggles.Count;j++)
        {   
            //設定ID
            skillToggles[j].skillID = GameData.ctrl.skillToggles[j].skillID;
            SkillInfo skillInfo = GameManager.ctrl.skillSystem.skillDB.SearchSkill(skillToggles[j].skillID);
            skillToggles[j].image.sprite = skillInfo.skillIcon;
            skillToggles[j].Text.text = skillInfo.skillName;
            /*for (int k = 0;k < skillDB.skillInfos.Count;k++)
            {
                //用ID比對資料庫
                if (skillToggles[j].skillID == skillDB.skillInfos[k].skillID)
                {
                    skillToggles[k].image.sprite = skillDB.skillInfos[k].skillIcon;
                    skillToggles[k].Text.text = skillDB.skillInfos[k].skillName;
                }
            }*/
        }
	}
	
	public void ToggleSwitch(int index)
    {
        this.index = index;
    }
     public void SetData(SkillInfo skillInfo)
    {
        //欄位技能相同時，禁止操作
        if (skillToggles[index].skillID == skillInfo.skillID) return;
        //技能重複設定時，取消先前設定的欄位
        for (int i = 0;i < skillToggles.Count;i++)
        {
            if (skillToggles[i].skillID == skillInfo.skillID)
            {
                skillToggles[i].skillID = -1;
                skillToggles[i].image.sprite = defaultIcon;
                skillToggles[i].Text.text = "";
            }
        }
        //資料設定至UI
        skillToggles[index].skillID = skillInfo.skillID;
        skillToggles[index].image.sprite = skillInfo.skillIcon;
        skillToggles[index].Text.text = skillInfo.skillName;

        //資料儲存
        GameData.ctrl.SkillSave(skillToggles);
    }
}

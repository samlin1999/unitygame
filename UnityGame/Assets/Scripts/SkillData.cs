using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillData : MonoBehaviour {
    public Button button;
    public SkillInfo skillInfos;
    public UISkillInfoPanelSW infoPanelSW;
    public SkillSystem skillSystem;
    public Text text;
	
    public void SendDataToSystem()
    {
        skillSystem.SetData(skillInfos);
    }

    public void ShowInfo()
    {
        infoPanelSW.setData(skillInfos);
    }
	
    public void setData(SkillInfo skillInfos)
    {
        gameObject.SetActive(true);
        this.skillInfos = skillInfos;
        button.image.sprite = this.skillInfos.skillIcon;
        text.text = this.skillInfos.skillName;
    }
}

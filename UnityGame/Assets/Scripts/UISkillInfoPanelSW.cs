using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillInfoPanelSW : MonoBehaviour {
    public Text skillText, cdText, desText;
	
    public void setData(SkillInfo skillInfo)
    {
        skillText.text = skillInfo.skillName;
        cdText.text = skillInfo.skillCD.ToString();
        desText.text = skillInfo.description;
    }
}

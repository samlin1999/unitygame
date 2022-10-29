using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillBtnPanelCtrl : MonoBehaviour {
    public List<SkillBtn> skillBtns = new List<SkillBtn>(3);
	// Use this for initialization
	void Start ()
    {
        for(int i = 0;i < skillBtns.Count;i++)
        {
            SkillSystem skillSystem = GameManager.ctrl.skillSystem;
            int ID = skillSystem.skillToggles[i].skillID;
            SkillInfo skillInfo = skillSystem.skillDB.SearchSkill(ID);
            skillBtns[i].cd = skillInfo.skillCD;
            skillBtns[i].icon.sprite = skillInfo.skillIcon;
            skillBtns[i].skillObj = skillInfo.skillObj;
        }
        
    }
        
	// Update is called once per frame
	void Update () {
		
	}
}

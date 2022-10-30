using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skillBtn : MonoBehaviour {
    /// <summary>
    /// 抓取按鈕零件
    /// </summary>
    public Button btn
    {
        get { return GetComponent<Button>(); }
    }
    public Image iconImg
    {
        get { return GetComponent<Image>(); }
    }
    public Sprite defaulticon, icon;
    public KeyCode keyCode;
    public Image cdImg;
    /*public float cdTime = 1, cd, costHp, costMp;
    public int skillNum;
    public GameObject skillObj;*/
    private skillInfo skillInfo;
	void Start () {
        cdImg.fillAmount = 0f;
	}
	
	void Update () {
        if (Input.GetKeyDown(keyCode) && skillInfo != null && skillInfo.timer.isDone)
        {
            btn.onClick.Invoke();//自動觸發執行onClick中的事件
        }
        else if (skillInfo != null)
        {
            //cd -= Time.deltaTime;
            cdImg.fillAmount = skillInfo.timer.GetPercent();
        }
	}
    public void setSkill(skillInfo skillInfo)//將資料庫資料倒入
    {
        this.skillInfo = skillInfo;
        icon = this.skillInfo.icon;
        //cdTime = skillInfo.cd;
        /*costHp = skillInfo.costHp;
        costMp = skillInfo.costMp;
        skillNum = skillInfo.skillNum;
        skillObj = skillInfo.skillObj;*/
        iconImg.sprite = icon ? icon : defaulticon;//決定技能按鍵可不可使用的鎖
        btn.onClick.AddListener(skilltrigger);//將skilltrigger的功能自動放入onClick
    }
    public void skilltrigger()
    {
        if (cdImg.fillAmount == 0f && skillInfo != null)
        {
            //判斷剩餘消耗是否可使用技能
            if (!characterInfo.ctrl.costCheck(skillInfo.costHp, skillInfo.costMp))
                return;
            characterInfo.ctrl.setHp(-skillInfo.costHp);
            characterInfo.ctrl.setMp(-skillInfo.costMp);
            skillInfo.timer.StartTimer();//cd = cdTime;
            if (skillInfo.skillObj)
                playercontrol.ctrl.Setskillobj(skillInfo.skillObj);
            playercontrol.ctrl.AniPlay("SKILL", "SKILL_NUM", skillInfo.skillNum);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour {
    public Image cdImg;
    public Text cdText;
    public Image icon;
    public float cdNow, cd = 5f;
    //技能實體
    public GameObject skillObj;
    PlayerCtrl m_playerCtrl;
    PlayerCtrl playerCtrl
    {
        get
        {
            if (m_playerCtrl == null) m_playerCtrl = PlayerCtrl.ctrl;
            return m_playerCtrl;
        }
    }
	
	void Start () {
        cdText.text = "";
        cdImg.fillAmount = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(cdNow > 0)
        {
            cdNow -= Time.deltaTime;
            cdImg.fillAmount = cdNow / cd;
            if (cdImg.fillAmount > 0)
            {
                cdText.text = cdNow.ToString("F0");
            }
            else
                cdText.text = "";
        }
	}

    public void Active()
    {
        if (cdNow > 0) return;
        cdNow = cd;
        cdImg.fillAmount = cdNow / cd;
        cdText.text = cdNow.ToString("F0");
        //技能生成位置為玩家的角色位置，及同步轉向
        Instantiate(skillObj,playerCtrl.skillSpawnPoint, playerCtrl.transform.rotation);
    }
}

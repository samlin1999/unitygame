using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillData : MonoBehaviour ,IBeginDragHandler,IDragHandler,IEndDragHandler{

    public Image skillimage
    {
        get { return GetComponent<Image>(); }
    }
    public Text skillName
    {
        get { return GetComponentInChildren<Text>(); }
    }

    private skillInfo skillInfo;
    public void OnBeginDrag(PointerEventData eventData)//選定要拖拉的技能
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        skillBtn skillBtn = eventData.pointerEnter.GetComponent<skillBtn>();
        if(GameManager.ctrl.characterInfo.skillBtns.Contains(skillBtn)) //當拖拉到的是技能欄位時，設置技能
            skillBtn.setSkill(skillInfo);
    }

    public void setData(skillInfo skillInfo)
    {

        gameObject.SetActive(true);
        this.skillInfo = new skillInfo(skillInfo);//讓資料和原本的相同
        skillimage.sprite = skillInfo.icon;
        skillName.text = skillInfo.skillName;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct SkillInfo
{
    public string skillName;
    public int skillID;
    public float skillCD;
    public Sprite skillIcon;
    [TextArea]
    public string description;
    //技能實體
    public GameObject skillObj;
}
[CreateAssetMenu(fileName = "SkillDB",menuName ="DB/SkillDB",order = 3)]
public class SkillDB : ScriptableObject {

    public List<SkillInfo> skillInfos;
    //資料庫檢索功能
    public SkillInfo SearchSkill(int id)
    {
        SkillInfo skillInfo = new SkillInfo();
        foreach(SkillInfo SI in skillInfos)
        {
            if(SI.skillID == id)
            {
                skillInfo = SI;
                break;
            }
        }
        return skillInfo;

    }
}

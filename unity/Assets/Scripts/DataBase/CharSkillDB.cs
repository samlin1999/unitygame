using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SkillGroupinfo
{
    public int level;
    public List<skillInfo> skillInfos;
}

[CreateAssetMenu(fileName = "CharSkillDB", menuName = "DB / CharSkillDB", order = 3)]
public class CharSkillDB : ScriptableObject
{
    public List<SkillGroupinfo> skillGroupinfos;
}

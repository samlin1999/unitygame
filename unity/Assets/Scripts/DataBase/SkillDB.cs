using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDB", menuName = "DB / SkillDB", order = 3)]
public class SkillDB : ScriptableObject {

    public List<skillInfo> skillInfos;
}

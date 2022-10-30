using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EffectDB", menuName = "DB / EffectDB", order = 2)]
public class EffectDB : ScriptableObject
{
    public List<EffectObj> fxList;
}

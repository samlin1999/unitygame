using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DropInfo
{
    public int ItemID;//物品編號
    public float chance;//機率
}
[CreateAssetMenu(fileName = "DropInfoDB", menuName = "DB/DropInfoDB", order = 1)]
public class DropChanceList : ScriptableObject {
    public List<DropInfo> dropInfos;
}

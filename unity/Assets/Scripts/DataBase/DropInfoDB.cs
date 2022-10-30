using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropInfoDB", menuName = "DB / DropInfoDB", order = 5)]
public class DropInfoDB : ScriptableObject {
    public List<ItemDropInfo> dropList;
}

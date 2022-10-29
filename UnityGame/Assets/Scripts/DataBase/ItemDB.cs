using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDB",menuName = "DB/ItemDB",order = 2)]
public class ItemDB : ScriptableObject {
    public List<ItemData> itemDatas;

    public ItemData SearchItem(int itemID)
    {
        ItemData itemData = null;
        for (int i = 0; i < itemDatas.Count; i++)
        {
            if (itemDatas[i].itemID == itemID)
            {
                itemData = itemDatas[i];
                break;
            }
        }
        return itemData;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagSystem : MonoBehaviour
{
    public static BagSystem ctrl;
    private void Awake()
    {
        ctrl = this;
    }
    public int slotCount = 20;//格子
    public GameObject slotTMP;
    public List<Slot> slots;
    RectTransform rectTransform
    {
        get{ return GetComponent<RectTransform>(); }
    }
    float CountHeight
    {
        get { return (slotCount / 4) * 150; }
    }
    void Start () {
        CreateSlot();
        createItems(6,5);
        createItems(5,5);
        StoreUpItem(1001);
        StoreUpItem(1002);
        StoreUpItem(1003);
        StoreUpItem(10001);
        StoreUpItem(10002);
        StoreUpItem(10003);
        StoreUpItem(10004);
        StoreUpItem(10005);
    }
    /// <summary>
    /// 創建背包物品圖片
    /// </summary>
	public void CreateSlot()
    {
        rectTransform.sizeDelta = Vector2.up * CountHeight;
        Slot slot = null;
        for(int i = 0;i < slotCount;i++)
        {
            slot = Instantiate(slotTMP, transform).GetComponent<Slot>();
            slot.gameObject.SetActive(true);
            slots.Add(slot);
        }
    }
	
	void Update () {
		
	}
    /// <summary>
    /// 預處理創建物品
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="count"></param>
    public void createItems(int itemID,int count = 1)
    {
        for(int i = 0;i < count;i++)
        {
            StoreUpItem(itemID);
        }
    }
    public void SortOutBag()
    {
        for(int i = 0;i < slots.Count - 1;i++)
        {
            if(slots[i].IsEmpty() && !slots[i + 1].IsEmpty())
            {
                slots[i].ChangeItemData(slots[i + 1]);
                slots[i + 1].DeleteItem();
            }
        }
    }
    /// <summary>
    /// 將掉落物的資料放入背包空格
    /// </summary>
    /// <param name="itemID"></param>
    public void StoreUpItem(int itemID)
    {
        Item itemTMP = GameManager.ctrl.gameData.itemDictionary[itemID];
        if(itemTMP.capacity > 1)
        {
            for(int i = 0;i < slots.Count;i++)
            {
                if(itemTMP.id == slots[i].itemID() && slots[i].CanStackable())
                {
                    slots[i].SetItem(itemTMP);
                    break;
                }
                else if(slots[i].IsEmpty())
                {
                    slots[i].SetItem(itemTMP);
                    break;
                }
            }
        }
        else
        {
            for(int i = 0;i < slots.Count;i++)
            {
                if(slots[i].IsEmpty())
                {
                    slots[i].SetItem(itemTMP);
                    break;
                }
            }
        }   
    }
    /// <summary>
    /// 搜索背包特定物品ID的格子
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public object[] SearchItemSlotByID(int itemID)
    {
        Slot getslot = null;
        int total = 0;
        foreach(Slot slot in slots)
        {
            if(slot.item.id == itemID)
            {
                getslot = slot;
                total += slot.item.amount;
            }
        }
        object[] itemAndCount = { getslot, total };
        return itemAndCount;
    }
    public object[] SearchItemSlotByType(ItemType itemType, int itemID)
    {
        Slot getslot = null;
        int total = 0;

        foreach(Slot slot in slots)
        {
            if(slot.item.itemType == itemType && slot.item.id == itemID)
            {
                getslot = slot;
                total += slot.item.amount;
            }
        }
        object[] itemAndCount = { getslot, total };
        return itemAndCount;
    }
}

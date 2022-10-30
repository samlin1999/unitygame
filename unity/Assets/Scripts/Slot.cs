using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
    IDragHandler,IDropHandler,IEndDragHandler,IBeginDragHandler
{
    public Image StatusImage;
    public Sprite defaultImage;
    public Sprite equipImage;
    public Image image
    {
        get { return GetComponent<Image>(); }
    }
    public Text text
    {
        get { return GetComponentInChildren<Text>(); }
    }
    public RectTransform rectTransform
    {
        get { return GetComponent<RectTransform>(); }
    }
    public Sprite emptySprite;
    public  static Slot slotfrom;
    public Item item;
    public static Action<Item> actionEnter;
    public static Action<Item> actionRefresh;
    public static Action actionExit;
    public static PotionBtn potionBtn;
    

    public bool isEquip
    {
        get { return GameManager.ctrl.equipSystem.CheckEquiped(item); }
    }
	// Use this for initialization
	void Start () {
        if (item.itemType == ItemType.None) DeleteItem();
    }
    //UNITY依照固定時間間隔更新的UPDATE
    void FixedUpdate () {
        if (!IsEmpty())
            ItemStatus();
	}
    void ItemStatus()
    {
        switch (item.itemType)
        {
            case ItemType.Consumable:
                StatusImage.fillAmount = Item.GCD.GetPercent();
                StatusImage.sprite = defaultImage;
                break;
            case ItemType.Stuff:
                StatusImage.fillAmount = 1;
                StatusImage.sprite = defaultImage;
                break;
            case ItemType.Weapon:
                StatusImage.fillAmount = isEquip ? 1 : 0;
                StatusImage.sprite = isEquip ? equipImage : defaultImage;
                break;
            case ItemType.Armor:
                StatusImage.fillAmount = isEquip ? 1 : 0;
                StatusImage.sprite = isEquip ? equipImage : defaultImage;
                break;
            default:
                StatusImage.fillAmount = 0;
                StatusImage.sprite = defaultImage;
                break;
        }
    }
    public int itemID()
    {
        return item.id;
    }
    public void SetItem(Item I)
    {
        //item = I;
        if (item.amount < 1)
        {
            item = I.CopyItem();//從字典複製資料(淺複製)，格式內資料不需重填
            image.sprite = item.icon;
        }     
        item.amount++;
        text.text = item.capacity > 1 ? item.amount.ToString() : string.Empty;
    }
    public bool CanStackable()
    {
        return item.capacity > 1 && item.amount < item.capacity;
    }
    public bool IsEmpty()
    {
        return item.itemType == ItemType.None;
    }
    /// <summary>
    /// 物品資料交換功能
    /// </summary>
    /// <param name="targetSlot"></param>
    public void ChangeItemData(Slot targetSlot)
    {
        Item itemTmp = item.CopyItem();//當前位置的資料複製(紀錄)
        item = targetSlot.item.CopyItem();
        targetSlot.item = itemTmp;
        reflashUI();
        targetSlot.reflashUI();
    }
    /// <summary>
    /// 及時刷新UI
    /// </summary>
    public void reflashUI()
    {
        image.sprite = item.icon;
        text.text = (item.capacity > 1) ? item.amount.ToString() : String.Empty;
    }
    public void DeleteItem()
    {
        image.sprite = emptySprite;
        text.text = String.Empty;
        item.itemType = ItemType.None;
        item.id = -1;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (actionEnter != null)
            actionEnter(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (actionExit != null)
            actionExit();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //雙點擊阻擋
        if (eventData.clickCount % 2 != 0)
            return;
        #region 消耗品
        UseConsumable();

        #endregion
        #region 裝備武器
        if (item.itemType == ItemType.Weapon)
            ((Weapon)item).Use();
        #endregion
        #region 裝備防具
        if (item.itemType == ItemType.Armor)
            ((Armor)item).Use();
        #endregion
        if (actionRefresh != null)
            actionRefresh(item);
    }
    public bool UseConsumable()
    {
        if (item.itemType == ItemType.Consumable && item.amount >= 1)
        {
            ((Consumable)item).Use();//物品使用
            if (item.amount == 0)
            {
                DeleteItem();
                GameManager.ctrl.bagSystem.SortOutBag();
                potionBtn.ClearItem();
            }
            else
                text.text = item.amount.ToString();
            return true;
        }
        return false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        slotfrom = this;
    }
    /// <summary>
    /// 拖曳記錄
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        slotfrom = this;
    }
    /// <summary>
    /// 拖曳放開
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        if (slotfrom.IsEmpty()) return;
        if(IsEmpty())
        {
            ChangeItemData(slotfrom);
            slotfrom.DeleteItem();//跟空格換要清除資料
            GameManager.ctrl.bagSystem.SortOutBag();//因為有空格，所以整理背包
        }
        else
            ChangeItemData(slotfrom);
        slotfrom = null;
        if (actionRefresh != null)
            actionRefresh(item);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!potionBtn) potionBtn = eventData.pointerEnter.GetComponent<PotionBtn>();
        if(eventData.pointerEnter.GetComponent<PotionBtn>() && item.itemType == ItemType.Consumable) potionBtn.SetSlot(this);
    }
}

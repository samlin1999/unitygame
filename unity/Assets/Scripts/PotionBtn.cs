using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionBtn : MonoBehaviour {

    /// <summary>
    /// 抓取按鈕零件
    /// </summary>
    public Button btn
    {
        get { return GetComponent<Button>(); }
    }
    public Image iconImg
    {
        get { return GetComponent<Image>(); }
    }
    public Sprite defaulticon, icon;
    public KeyCode keyCode;
    public Image cdImg;
    /*
    private Consumable item
    {
        get { return (Consumable)slot.item; }
    }*/
    #region 當前物品的資料
    private Slot slot;
    private int itemID;
    private int totalAmount;
    #endregion
    void Start()
    {
        cdImg.fillAmount = 0f;
    }

    void Update()
    {
        if (!icon) return;
        if (Input.GetKeyDown(keyCode) && cdImg.fillAmount == 0f && Item.GCD.isDone)
        {
            btn.onClick.Invoke();//自動觸發執行onClick中的事件
        }
        else if (slot != null)
        {
            //cd -= Time.deltaTime;
            cdImg.fillAmount = Item.GCD.GetPercent();
        }
    }
    public void ClearItem()
    {
        itemID = -1;
        icon = null;
        iconImg.sprite = icon ? icon : defaulticon;
        btn.onClick.RemoveListener(Potiontrigger);//註銷onClick
    }
    /// <summary>
    /// 設定來源物品資料(藥水)
    /// </summary>
    /// <param name="slot"></param>
    public void SetSlot(Slot slot)//將資料庫資料倒入
    {
        itemID = slot.itemID();
        icon = slot.item.icon;
        iconImg.sprite = icon ? icon : defaulticon;
        btn.onClick.AddListener(Potiontrigger);//自動放入onClick
    }
    public void Potiontrigger()
    {
        if(cdImg.fillAmount == 0f && icon)
        {
            object[] objs = GameManager.ctrl.bagSystem.SearchItemSlotByID(itemID);
            slot = (Slot)objs[0]; 
            totalAmount = (int)objs[1];
            if (slot.UseConsumable())
                totalAmount--;
            icon = totalAmount > 0 ? icon : null;
            iconImg.sprite = icon ? icon : defaulticon;
            slot = icon ? slot : null;
        }
    }

    /*
    public void Potiontrigger()
    {
        if (cdImg.fillAmount == 0f && slot != null)
        {
            slot = slot.UseConsumable() ? slot : null;//能否使用
            icon = slot ? icon : null;//是否用完
            iconImg.sprite = icon ? icon : defaulticon;
        }
    }*/
}

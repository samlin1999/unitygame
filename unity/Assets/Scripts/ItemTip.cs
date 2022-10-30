using System.Collections;
using System.Text;//文字排版函式庫
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTip : MonoBehaviour {
    public RectTransform bgrect
    {
        get { return GetComponent<RectTransform>(); }
    }
    public RectTransform textRect
    {
        get { return textTip.GetComponent<RectTransform>(); }
    }
    public Image imgBG
    {
        get { return GetComponent<Image>(); }
    }
    public Text textTip
    {
        get { return GetComponentInChildren<Text>(); }
    }
    public UISwitch uiSwitch
    {
        get { return GetComponent<UISwitch>(); }
    }

    private void Awake()
    {
        Slot.actionEnter += show;
        Slot.actionExit += hide;
        Slot.actionRefresh += TipRefresh;
    }
    private Vector2 baseSize = Vector2.one * 60;

    void Start () {
		
	}
    public StringBuilder SB = new StringBuilder();//文字排版
    private bool isShow;
	void Update () {
        if (!isShow) return;
		bgrect.sizeDelta = textRect.sizeDelta + baseSize;
	}
    //開啟物品說明
    void show(Item item)
    {
        if (isShow || item.itemType == ItemType.None) return;
        uiSwitch.Switch(true);
        StringTpye(item);//依照物品類型產生說明格式
        isShow = true;
    }
    void TipRefresh(Item item)
    {
        StringTpye(item);//依照物品類型產生說明格式
    }
    //關閉物品說明
    void hide()
    {
        isShow = false;              
        uiSwitch.Switch(false);
        bgrect.sizeDelta = baseSize;
    }

    void StringTpye(Item item)
    {
        switch (item.itemType)
        {
            case ItemType.Consumable:
                SB = new StringBuilder();
                SB.AppendFormat("<color=red>{0}</color> ({1})", item.itemName,"消耗品");
                SB.AppendLine();//換行
                SB.AppendLine();
                if(item.capacity > 1)
                {
                    SB.AppendFormat("持有數量: {0}", item.amount);
                    SB.AppendLine();
                    SB.AppendFormat("堆疊上限: {0}", item.capacity);
                    SB.AppendLine();
                }
                if(((Consumable)item).recoveryHP > 0)
                {
                    SB.AppendFormat("血量回復量: {0}", ((Consumable)item).recoveryHP);
                    SB.AppendLine();
                }
                if (((Consumable)item).recoveryMP > 0)
                {
                    SB.AppendFormat("魔力回復量: {0}", ((Consumable)item).recoveryMP);
                    SB.AppendLine();
                }
                SB.Append(item.description);
                textTip.text = SB.ToString();
                break;
            case ItemType.Stuff:
                SB = new StringBuilder();
                SB.AppendFormat("<color=red>{0}</color> ({1})", item.itemName, "素材");
                SB.AppendLine();//換行
                if(item.capacity > 1)
                {
                    SB.AppendFormat("持有數量: {0}", item.amount);
                    SB.AppendLine();
                    SB.AppendFormat("堆疊上限: {0}", item.capacity);
                    SB.AppendLine();
                }
                SB.Append(item.description);
                textTip.text = SB.ToString();
                break;
            case ItemType.Weapon:
                SB = new StringBuilder();
                SB.AppendFormat("<color=red>{0}</color> ({1})", item.itemName, "武器");
                SB.AppendLine();//換行
                SB.AppendFormat("傷害值: {0}", ((Weapon)item).damage);
                SB.AppendLine();
                SB.Append(item.description);
                textTip.text = SB.ToString();
                break;
            case ItemType.Armor:
                SB = new StringBuilder();
                SB.AppendFormat("<color=red>{0}</color> ({1})", item.itemName, "防具");
                SB.AppendLine();//換行
                SB.AppendFormat("防禦值: {0}", ((Armor)item).defense);
                SB.AppendLine();
                SB.Append(item.description);
                textTip.text = SB.ToString();
                break;
            default:
                textTip.text = "";
                break;
        }

    }
}

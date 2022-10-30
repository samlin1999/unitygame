using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;

/// <summary>
/// 職業類型
/// </summary>
public enum ClassType {Warrior, Archer, Mage }
/// <summary>
/// 職業數據組合
/// </summary>
[System.Serializable]
public struct CharAttr
{
    public string className;
    public ClassType classType;
    public int CON;
    public int STR;
    public int INT;
    public int DEX;
    public int GetAttrByIndex(int index)
    {
        int attr = 0;
        switch(index)
        {
            case 0: attr = CON; break;
            case 1: attr = STR; break;
            case 2: attr = INT; break;
            case 3: attr = DEX; break;
            default: attr = 4; break;
        }
        return attr;
    }
}

/// <summary>
/// 點數分配
/// </summary>
[System.Serializable]
public struct Attributes
{
    public static bool LockPoint;
    public int min;
    public const int max = 50;
    public int point;//已加入的點數
    public int tmpPoint;//未加入前的點數
    public Text pointTxt;
    public Button plusBtn, minusBtn;
    public int end;//最終點數
    public void Set(int startval)
    {
        min = startval;
    }
    public void Add()
    {
        tmpPoint++;
        LockPoint = false;
    }
    public void release()
    {
        tmpPoint--;
    }
    public void Apply()
    {
        LockPoint = true;
        point += tmpPoint;//加上暫存點數
        tmpPoint = 0;//暫存清空
    }
    public void Reload(int freePoint)//加減按鈕和可用點數的顯示
    {
        end = min + point + tmpPoint;
        if (end > max)
            end = max;
        pointTxt.text = end.ToString();
        plusBtn.interactable = (freePoint > 0 && end < max);
        minusBtn.interactable = (tmpPoint > 0 && !LockPoint);
    }
    public bool GetTmpPoint()//判斷是否拿到點數
    {
        return tmpPoint > 0;
    }
}
/// <summary>
/// 特效呈現
/// </summary>
[System.Serializable]
public struct EffectObj
{
    public GameObject fx;
}
/// <summary>
/// 技能資訊
/// </summary>
[System.Serializable]
public class skillInfo
{
    public string skillName;
    public Sprite icon;
    public float cd,  costHp, costMp;
    public int skillNum;
    public GameObject skillObj;
    public Timer timer;//共用計時
    /// <summary>
    /// 建構式，將資料庫資料放入，技能本體
    /// </summary>
    /// <param name="skillInfo"></param>
    public skillInfo(skillInfo skillInfo)
    {
        this.icon = skillInfo.icon;
        this.skillName = skillInfo.skillName;
        this.cd = skillInfo.cd;
        this.costHp = skillInfo.costHp;
        this.costMp = skillInfo.costMp;
        this.skillNum = skillInfo.skillNum;
        this.skillObj = skillInfo.skillObj;
        timer = new Timer(cd);
    }

}
/// <summary>
/// 場景資訊
/// </summary>
[System.Serializable]
public struct stageInfo
{
    public string keyVal;//場景名稱
    public string stageName;//地圖名稱
    public List<Task> taskList;
}
/// <summary>
/// 任務標題
/// </summary>
[System.Serializable]
public class Task
{
    public string taskName, eventName;//任務標題
    public List<Condition> conditionList;//任務完成條件清單
    public taskcontent taskcontents;//紀錄樣板的實體對象,連結的UI面板
    public bool complete;
    public Task(Task task)
    {
        taskName = task.taskName;
        conditionList = new List<Condition>();
        for(int i = 0;i < task.conditionList.Count;i++)
        {
            conditionList.Add(new Condition(task.conditionList[i]));
        }
    }
    public taskcontent GetTmpUI()
    {
        return taskcontents;
    }
    public void SetTmpUI(taskcontent TC)
    {
        taskcontents = TC;
        taskcontents.SetTask(this);
    }
    public void Update()
    {
        for(int i = 0;i < conditionList.Count;i++)
        {
            if(conditionList[i].Complete())
            {
                complete = true;
            }
            else
            {
                complete = false;
                break;
            }
        }
        taskcontents.TaskUpdate(complete);
    }
    public bool Complete()
    {
        return complete;
    }
    
}
/// <summary>
/// 任務條件
/// </summary>
[System.Serializable]
public class Condition
{
    public string conditionDes;//任務描述
    public int amount, completeamount;//未完成數和完成數
    conditionContent conditionContent;//連結的UI面板
    public string name;
    bool complete;
    public Condition(Condition C)
    {
        conditionDes = C.conditionDes;
        completeamount = C.completeamount;
        name = C.name;
    }
    public conditionContent GetTmpUI()
    {
        return conditionContent;
    }
    public void SetTmpUI(conditionContent CC)
    {
        conditionContent = CC;
        conditionContent.Setcondition(this);
    }
    public string CompleteCount()
    {
        return " (" + amount.ToString() + "/" + completeamount.ToString() + ")";
    }
    
    public bool Complete()
    {
        return complete; 
    }
    /// <summary>
    /// 怪物死亡計數器增加
    /// </summary>
    public void AddToAmount()
    {
        amount++;
        Update();
    }
    public void Update()
    {
        if(amount >= completeamount)
        {
            amount = completeamount;
            complete = true;
        }
        conditionContent.ConditionUpdate();
    }
}
/// <summary>
/// 掉落率
/// </summary>
[System.Serializable]
public struct DropChance
{
    public float max, min;
    public void Set(float a,float b)
    {
        min = a;
        max = b;
    }
    public float report()
    {
        return UnityEngine.Random.Range(min, max);
    }
    public int reportInt()
    {
        return (int)UnityEngine.Random.Range(min, max);
    }
}
/// <summary>
/// 掉落物品資訊
/// </summary>
[System.Serializable]
public struct ItemDropInfo
{
    public int itemID;//掉落物ID
    public float chance;//掉落率
}
public enum ItemType { None, Stuff, Consumable, Weapon, Armor }
[System.Serializable]
public class Item
{
    public static Timer GCD = new Timer(0.5f);//公共冷卻CD
    public ItemType itemType;//種類
    public int id;//編號
    public string itemName;//名稱
    public string description;//描述
    public int capacity;//容量上限
    public int amount;//持有數量
    public List<IBuff> buffs = new List<IBuff>(); //紀錄所持有的buff
    public Sprite icon
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Items/" + id.ToString() + ".png");//Resources.Load<Sprite>("Items/" + id.ToString() + ".png"); 
        }
    }
    /// <summary>
    /// 基礎格式
    /// </summary>
    /// <param name="id">編號</param>
    /// <param name="itemName">名稱</param>
    /// <param name="description">描述</param>
    /// <param name="capacity">容量上限</param>
    public Item(int id,string itemName,string description,int capacity)
    {
        this.id = id;
        this.itemName = itemName;
        this.description = description;
        this.capacity = capacity;
    }
    /// <summary>
    /// 淺複製物件，格式內資料不需重填
    /// </summary>
    /// <returns></returns>
    public Item CopyItem()
    {
        return (Item)this.MemberwiseClone();//淺複製
    }
}
/// <summary>
/// 素材
/// </summary>
public class Stuff : Item
{
    public Stuff(int id, string itemName, string description, int capacity)
        : base(id, itemName, description, capacity)
    {
        this.itemType = ItemType.Stuff;
        this.id = id;
        this.itemName = itemName;
        this.description = description;
        this.capacity = capacity;
    }
}

/// <summary>
/// 消耗品
/// </summary>
public class Consumable : Item
{
    public int recoveryHP, recoveryMP;
    public Consumable(int recoveryHP,int recoveryMP,int id, string itemName, string description,int capacity) 
        : base( id, itemName, description, capacity)
    {
        this.itemType = ItemType.Consumable;
        this.recoveryHP = recoveryHP;
        this.recoveryMP = recoveryMP;
        this.id = id;
        this.itemName = itemName;
        this.description = description;
        this.capacity = capacity;
    }
    public void Use()
    {
        if(GCD.isDone)
        {
            if(GameManager.ctrl.characterInfo.recovery(recoveryHP,recoveryMP))
            {
                amount--;
                GCD.StartTimer();//啟動冷卻
            }
            else
            {
                GameManager.ctrl.characterInfo.PopUp("已經滿了");
            }
        }
        else
        {
            GameManager.ctrl.characterInfo.PopUp("正在準備");
        }
    }
}
/// <summary>
/// 裝備介面
/// </summary>
[System.Serializable]
public struct EquipInfo
{
    public Image icon;
    public Sprite defaultImage;
    public Item itemData;
    /// <summary>
    /// 設置裝備圖標和資料
    /// </summary>
    /// <param name="item"></param>
    public void SetData(Item item)
    {
        if (itemData != item)
        {
            itemData = item;
            icon.sprite = itemData.icon;
            GameManager.ctrl.buffManager.AddBuff(item.buffs);
        }
        else
            UnEquip();
        //Debug.Log(item.itemName);
    }
    /// <summary>
    /// 雙手武器的設置資料
    /// </summary>
    /// <param name="item"></param>
    /// <param name="OffHand"></param>
    /*public void SetData(Weapon item,EquipInfo otherHand)
    {
        if (itemData != item)
        {
            itemData = item;
            icon.sprite = itemData.icon;
            if(item.handType == HandType.Bothhand)
                otherHand.UnEquip();//當另一隻手有武器且接下來要裝雙手武器
            if (item.handType == HandType.Offhand && ((Weapon)otherHand.itemData).handType == HandType.Bothhand)
                otherHand.UnEquip();//當目前裝了雙手武器且接下來要裝單手武器
        }
        else
            UnEquip();
    }*/
    public Weapon GetWeaponData()
    {
        return (Weapon)itemData;
    }
    public void UnEquip()
    {
        if (itemData == null) return;
        GameManager.ctrl.buffManager.RemoveBuff(itemData.buffs);
        itemData = null;
        icon.sprite = defaultImage;
       
    }
    /// <summary>
    /// 比對裝備欄裝備是否和實際要的裝備相同(背包和裝備欄比對)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Checker(Item item)
    {
        return itemData == item;
    }

}
public enum HandType { Mainhand,Offhand,Bothhand}
/// <summary>
/// 武器
/// </summary>
public class Weapon : Item
{
    public int damage;
    public AttackBuff attackBuff;//個別定義的buff
    public HandType handType;
    public Weapon(HandType handType, int damage, int id, string itemName, string description, int capacity = 1)
        : base(id, itemName, description, capacity)
    {
        this.handType = handType;
        this.itemType = ItemType.Weapon;
        this.damage = damage;
        attackBuff = new AttackBuff(damage,BuffType.None,-1);
        buffs.Add(attackBuff);
        this.id = id;
        this.itemName = itemName;
        this.description = description;
        this.capacity = capacity;
    }
    public void Use()
    {
        switch (handType)
        {
            case HandType.Mainhand:
                GameManager.ctrl.equipSystem.mainweapon.SetData(this);
                break;
            case HandType.Offhand:
                GameManager.ctrl.equipSystem.secweapon.SetData(this);
                Weapon weapon = GameManager.ctrl.equipSystem.mainweapon.GetWeaponData();
                if(weapon != null && weapon.handType == HandType.Bothhand)
                {
                    GameManager.ctrl.equipSystem.mainweapon.UnEquip();
                }
                
                break;
            case HandType.Bothhand:
                GameManager.ctrl.equipSystem.secweapon.UnEquip();
                GameManager.ctrl.equipSystem.mainweapon.SetData(this);
                break;
        }

    }
}
public enum PartType { Helmet, Body, Gloves, Leg, Boots}
/// <summary>
/// 護甲
/// </summary>
public class Armor : Item
{
    public int defense;
    public PartType partType;
    public DefenseBuff defenseBuff;
    public Armor(PartType partType,int defense, int id, string itemName, string description, int capacity = 1)
        : base(id, itemName, description, capacity)
    {
        this.partType = partType;
        this.itemType = ItemType.Armor;
        defenseBuff = new DefenseBuff(defense, BuffType.None, -1);
        buffs.Add(defenseBuff);
        this.defense = defense;
        this.id = id;
        this.itemName = itemName;
        this.description = description;
        this.capacity = capacity;
    }
    public void Use()
    {
        switch (partType)
        {
            case PartType.Helmet:
                GameManager.ctrl.equipSystem.helmet.SetData(this);
                break;
            case PartType.Body:
                GameManager.ctrl.equipSystem.body.SetData(this);
                break;
            case PartType.Gloves:
                GameManager.ctrl.equipSystem.gloves.SetData(this);
                break;
            case PartType.Leg:
                GameManager.ctrl.equipSystem.leg.SetData(this);
                break;
            case PartType.Boots:
                GameManager.ctrl.equipSystem.boots.SetData(this);
                break;
        }
    }
}
public sealed class Timer
{
    public bool isDone { get { return duration >= 0 && timeNow <= 0; } }//計時器跑完
    public float timeNow { get { return UpdateTimer(); } }
    private float duration;//跑多久
    private float startTime, timeLeft;//開始時間，剩餘要跑完的時間

    public Timer(float duration)
    {
        this.duration = duration;
        startTime = -duration;
    }
    /// <summary>
    /// 啟動計時器
    /// </summary>
    public void StartTimer()
    {
        startTime = Time.time;//紀錄計時器開始跑的時間
    }
    /// <summary>
    /// 回傳剩餘要跑完的時間
    /// </summary>
    /// <returns></returns>
    float UpdateTimer()
    {
        timeLeft = duration - (Time.time - startTime);
        return timeLeft;// < 0 ? 0: timeLeft;
    }
    /// <summary>
    /// 比例
    /// </summary>
    /// <returns></returns>
    public float GetPercent()
    {
        return timeNow / duration;
    }

}

public class GameData  {
    public Dictionary<int, Item> itemDictionary;

    public GameData()
    {
        itemDictionary = new Dictionary<int, Item>();
        itemDictionary.Add(0, new Consumable(100, 0, 0, "恢復藥劑", "讓血量小幅回復", 99));
        itemDictionary.Add(1, new Consumable(0, 100, 1, "魔力藥劑", "讓魔力小幅回復", 99));
        itemDictionary.Add(2, new Consumable(500, 0, 2, "中型恢復藥劑", "讓血量中幅回復", 99));
        itemDictionary.Add(3, new Consumable(0, 200, 3, "中型魔力藥劑", "讓魔力中幅回復", 99));
        itemDictionary.Add(4, new Consumable(1000, 0, 4, "大型恢復藥劑", "讓血量大幅回復", 99));
        itemDictionary.Add(5, new Consumable(0, 500, 5, "大型魔力藥劑", "讓魔力大幅回復", 99));
        itemDictionary.Add(6, new Consumable(1000, 500, 6, "混和恢復藥劑", "使體力和魔力恢復", 99));
        itemDictionary.Add(11, new Stuff(11, "骨頭碎片", "骨頭碎片", 50));
        itemDictionary.Add(12, new Stuff(12, "破碎的吊飾", "破碎的吊飾", 50));
        itemDictionary.Add(1001, new Weapon(HandType.Mainhand, 100, 1001, "長劍", "品質一般的武器"));
        itemDictionary.Add(1002, new Weapon(HandType.Offhand, 100, 1002, "盾", "品質一般的盾牌"));
        itemDictionary.Add(1003, new Weapon(HandType.Bothhand, 200, 1003, "十字弓", "品質一般的雙手弓"));
        itemDictionary.Add(10001, new Armor(PartType.Body,50, 10001, "鐵甲", "防禦不高的盔甲" ));
        itemDictionary.Add(10002, new Armor(PartType.Helmet, 50, 10002, "鐵盔", "防禦不高的頭盔"));
        itemDictionary.Add(10003, new Armor(PartType.Gloves, 50, 10003, "護手", "防禦不高的護手"));
        itemDictionary.Add(10004, new Armor(PartType.Boots, 50, 10004, "鞋子", "防禦不高的鞋子"));
        itemDictionary.Add(10005, new Armor(PartType.Leg, 50, 10005, "褲子", "防禦不高的褲子"));
    }

}

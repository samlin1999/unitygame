using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SaveType { LEVEL, EXP, MONEY, GEM, ENERGY}

/// <summary>
/// 場景資料結構設計(系統序列化)，包含多個Stage
/// </summary>
[System.Serializable]
public struct SceneInfo
{
    public List<StageInfo> stageInfos;
}
/// <summary>
/// 關卡資料結構設計(系統序列化)
/// </summary>
[System.Serializable]
public struct StageInfo
{
    public string stageName;
    public int cost;
}

/// <summary>
/// 用於儲存資料
/// </summary>
public class GameData {
    public static GameData ctrl;
    public int energy;
    public int money;
    public int gem;
    public float exp;
    public int level;
    public List<SlotData> slotDatas;
    public List<SkillToggleData> skillToggles;

    public GameData()
    {
        //Debug.Log("Load game data");
        ctrl = this;
        slotDatas = new List<SlotData>();
        skillToggles = new List<SkillToggleData>();
        energy = PlayerPrefs.GetInt("energy");//讀取上次存檔的資料
        money = PlayerPrefs.GetInt("money");
        gem = PlayerPrefs.GetInt("gem");
        level = PlayerPrefs.GetInt("level");
        exp = PlayerPrefs.GetFloat("exp");
    }

    public int LoadEnergy(int energyMax)
    {
        return PlayerPrefs.GetInt("energy", energyMax);
    }

    public int LoadMoney()
    {
        return PlayerPrefs.GetInt("money", 0);
    }

    public int Loadgem()
    {
        return PlayerPrefs.GetInt("gem", 100);
    }

    public int LoadLevel()
    {
        return PlayerPrefs.GetInt("level", level);
    }

    public float LoadExp()
    {
        return PlayerPrefs.GetFloat("exp",0);
    }

    public void Save()
    {
        PlayerPrefs.SetString("GameData", JsonUtility.ToJson(this));//以Json字串存取資料
        /*PlayerPrefs.SetInt("energy", energy);//存取資料
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetInt("gem", gem);
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetFloat("exp", exp);*/
    }

    public void BagSave(SlotData[] slotDatas)
    {
        this.slotDatas = new List<SlotData>(slotDatas);
        PlayerPrefs.SetString("GameData", JsonUtility.ToJson(this));
        Debug.Log(JsonUtility.ToJson(this));
    }

    public void SkillSave(List<SkillToggleData> skillToggles)
    {
        this.skillToggles = new List<SkillToggleData>(skillToggles);
        PlayerPrefs.SetString("GameData", JsonUtility.ToJson(this));
        //Debug.Log(JsonUtility.ToJson(this));
    }

    public void Save(SaveType saveType)
    {
        switch(saveType)
        {
            case SaveType.ENERGY:
                PlayerPrefs.SetInt("energy", energy);//存取資料
                break;
            case SaveType.MONEY:
                PlayerPrefs.SetInt("money", money);
                break;
            case SaveType.GEM:
                PlayerPrefs.SetInt("gem", gem);
                break;
            case SaveType.LEVEL:
                PlayerPrefs.SetInt("level", level);
                break;
            case SaveType.EXP:
                PlayerPrefs.SetFloat("exp", exp);
                break;

        }
    }
}

/// <summary>
/// 計時器
/// </summary>
public sealed class Timer
{
    private float duration;//持續時間
    public float timeLeft, startTime;//剩餘時間，開始時間
    public bool isDone
    {
        get { return timeNow <= 0; }
    }

    public float timeNow
    {
        get { return Update(); }
    }
    public Timer(float duration)
    {
        this.duration = duration;
    }
    //啟動計時器
    public void Start()
    {
        startTime = Time.time;
    }
    //更新計時器時間(剩餘時間)
    float Update()
    {
        return timeLeft = duration - (Time.time - startTime);
    }
}
/// <summary>
/// 骰子的資料結構
/// </summary>
[System.Serializable]
public struct Dice
{
    public float min, max;
    /// <summary>
    /// 設定(製作)骰子
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetDice(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
    /// <summary>
    /// 回傳骰子執行結果
    /// </summary>
    /// <returns></returns>
    public float Report()
    {
        return Random.Range(min, max);
    }
}
/// <summary>
/// 物品資訊
/// </summary>
[System.Serializable]
public class ItemData
{
    public string itemName;
    public int itemID;
    public int maxCount;
    public Sprite icon;
}


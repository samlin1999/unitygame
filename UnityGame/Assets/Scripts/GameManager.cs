using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 中央主控
/// </summary>
public class GameManager : MonoBehaviour {
    #region 選單介面攝影機抓取功能
    public Canvas canvas;//選取攝影機
    #endregion
    public static GameManager ctrl;
    //--InfoSystem----------------------------------
    public InfoSystem infoSystem//系統對外的公開接口
    {
        get
        {
            if (m_infoSystem == null)
                m_infoSystem = InfoSystem.ctrl;
            return m_infoSystem;
        }
    }
    private InfoSystem m_infoSystem;//系統實體存放的位置
    //--StageSystem----------------------------------
    public StageSystem stageSystem
    {
        get
        {
            if (m_stageSystem == null)
                m_stageSystem = StageSystem.ctrl;
            return m_stageSystem;
        }
    }
    private StageSystem m_stageSystem;
    //--HUDSystem----------------------------------
    public HUDSystem hudSystem
    {
        get
        {
            if (m_hudSystem == null)
                m_hudSystem = HUDSystem.ctrl;
            return m_hudSystem;
        }
    }
    private HUDSystem m_hudSystem;
    //--BagSystem----------------------------------
    public BagSystem bagSystem
    {
        get
        {
            if (m_bagSystem == null)
                m_bagSystem = new BagSystem();//給予實體空間
            return m_bagSystem;
        }
    }
    private BagSystem m_bagSystem;
    //--SkillSystem----------------------------------
    public SkillSystem skillSystem
    {
        get
        {
            if (m_skillSystem == null)
                m_skillSystem = SkillSystem.ctrl;
            return m_skillSystem;
        }
    }
    private SkillSystem m_skillSystem;
    int tmpMoney, tmpExp;
    private void Awake()
    {
        ctrl = this;
        DontDestroyOnLoad(gameObject);//無論如何不要卸載
    }
    //--TargetSystem----------------------------------
    public TargetSystem targetSystem
    {
        get
        {
            if (m_targetSystem == null)
                m_targetSystem = new TargetSystem();
            return m_targetSystem;
        }
    }
    private TargetSystem m_targetSystem;
    //--BuffSystem----------------------------------
    public BuffSystem buffSystem
    {
        get
        {
            if (m_buffSystem == null)
                m_buffSystem = new BuffSystem();
            return m_buffSystem;
        }
    }
    private BuffSystem m_buffSystem;
    //--AudioSystem----------------------------------
    public AudioSystem audioSystem
    {
        get
        {
            if (m_audioSystem == null)
                m_audioSystem = new AudioSystem();
            return m_audioSystem;
        }
    }
    private AudioSystem m_audioSystem;
    //--GameData----------------------------------
    public bool newGame;
    public GameData gameData
    {
        get
        {
            if (newGame) PlayerPrefs.DeleteAll();//存檔刪除
            if (m_gameData == null)
            {
                m_gameData = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString("GameData"));//讀取舊的遊戲資料
                if(m_gameData == null)
                   m_gameData = new GameData();//讀取舊的遊戲資料失敗，建立新的遊戲資料
            }
            else
            {
                m_gameData = GameData.ctrl;
            }
            return m_gameData;
        }
    }
    private GameData m_gameData;

    public ItemDB itemDB;
    public Dictionary<int, int> dropItemsTMP = new Dictionary<int, int>();//將掉落物創建字典
    void Start () {
        //audioSystem.PlayBGM(music, 0.7f);
        canvas.worldCamera = Camera.main;//將主攝影機設為關注對象(canvas中)
        infoSystem.Initial();//該系統初始化
        stageSystem.Initial();//關卡資訊初始化
        bagSystem.UpdateUI();//背包介面刷新(初始化)
        skillSystem.Initial();//技能初始化
    }
    //public AudioClip music;
    public SoundDB bgmDB, sfxDB;
	void Update () {
        buffSystem.UpdateBuff(Time.deltaTime);
	}
    /// <summary>
    /// 離開關卡
    /// </summary>
    public void BackToMenu()
    {
        infoSystem.moneyCtrl(tmpMoney);//存錢
        tmpMoney = 0;//暫存的錢歸零
        infoSystem.expCtrl(tmpExp);//加經驗
        BagUpdate();//背包物品資料刷新
        tmpExp = 0;//暫存的經驗值歸零
        targetSystem.Clear();//目標清單重置(離開關卡)
        buffSystem.ClearBuff();//buff狀態移除重置
        infoSystem.Switchpanel(true);
        SceneManager.LoadScene("Menu");
    }
    /// <summary>
    /// 播放背景音樂
    /// </summary>
    /// <param name="bgmName"></param>
    public void PlayBGM(string bgmName)
    {
        audioSystem.PlayBGM(bgmDB.SearchBGM(bgmName));
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="sfxName"></param>
    public void PlaySFX(string sfxName)
    {
        audioSystem.PlaySFX(sfxDB.SearchBGM(sfxName));
    }

    public void HpBarCtrl(float hp, float hpMax)
    {
        HUDSystem.ctrl.UpdateHp(hp, hpMax);
    }

    public void MoneyCtrl(int money)
    {
        tmpMoney += money;
        HUDSystem.ctrl.UpdateMoney(money);
    }

    public void GiveExp(int exp)
    {
        tmpExp += exp; 
    }
    public void BagUpdate()
    {
        foreach(KeyValuePair<int,int> keyValuePairs in dropItemsTMP)
        {
            ItemData itemData = itemDB.SearchItem(keyValuePairs.Key);
            bagSystem.PushItem(itemData, keyValuePairs.Value);
            print("Get: " + itemData.itemName + " / 數量: " + keyValuePairs.Value);
        }
        bagSystem.UpdateItem();
    }

    public void DropItem(int itemID)
    {
        int count = 0;
        if(dropItemsTMP.TryGetValue(itemID,out count))
        {
            dropItemsTMP[itemID] = count + 1;//找到既有的物件編號，修改數量
        }
        else
        {
            dropItemsTMP.Add(itemID, 1);//增加新物件
        }
        /*輸出測試
        for(int i = 0;i < itemDB.itemDatas.Count;i++)
        {
            if(itemDB.itemDatas[i].itemID == itemID)
            {
                print("Get: " + itemDB.itemDatas[i].itemName + " / 數量: " + dropItemsTMP[itemID]);
                break;
            }
        }*/
    }
}
/// <summary>
/// 背包空格資訊
/// </summary>
[System.Serializable]
public class SlotData
{
    public string itemName;
    public int itemID;
    public int maxCount = 10, count;
    public Sprite icon;
    public bool canPush
    {
        get { return count < maxCount; }
    }

    public int emptyCount
    {
        get { return maxCount - count; }
    }


    public void PushItem(ItemData itemData)
    {
        itemName = itemData.itemName;
        itemID = itemData.itemID;
        maxCount = itemData.maxCount;
        icon = itemData.icon;
        count++;
    }

    public void PushItem(ItemData itemData, int count)
    {
        itemName = itemData.itemName;
        itemID = itemData.itemID;
        maxCount = itemData.maxCount;
        icon = itemData.icon;
        this.count += count;
    }

}
/// <summary>
/// 背包系統
/// </summary>
public class BagSystem
{
    int bagCount;//背包格數
    public SlotData[] bagSlots;
    public BagSystem(int bagCount = 40)
    {
        this.bagCount = bagCount;
        bagSlots = new SlotData[this.bagCount];//空間規劃，創造空格
        for(int i = 0;i < this.bagCount;i++)
        {
            bagSlots[i] = new SlotData();//資料實體化(實體空格)
        }
        //存檔資料倒回
        for (int i = 0;i < GameManager.ctrl.gameData.slotDatas.Count;i++)
        {
            if (GameManager.ctrl.gameData.slotDatas[i].count == 0)
                break;
            bagSlots[i].itemName = GameManager.ctrl.gameData.slotDatas[i].itemName;
            bagSlots[i].itemID = GameManager.ctrl.gameData.slotDatas[i].itemID;
            bagSlots[i].maxCount = GameManager.ctrl.gameData.slotDatas[i].maxCount;
            bagSlots[i].count = GameManager.ctrl.gameData.slotDatas[i].count;
            bagSlots[i].icon = GameManager.ctrl.gameData.slotDatas[i].icon;
        }
    }
    /// <summary>
    /// 背包初始化
    /// </summary>
    public void UpdateUI()
    {
        UIBagPanelCtrl.ctrl.Initial(bagCount);//背包介面初始化，計算col數量
    }
    public void UpdateItem()
    {
        UIBagPanelCtrl.ctrl.UpdateUI(bagSlots);//背包介面初始化，計算col數量
    }

    public void PushItem(ItemData itemData,int count)
    {
        int itemCount = 0;
        //找空格的邏輯
        for (int i = 0;i < bagSlots.Length;i++)
        {
            //Debug.Log(bagSlots[i].canPush);
            if(!bagSlots[i].canPush) continue;//略過不可放的格子
            bagSlots[i].PushItem(itemData);
            if (itemData.maxCount == 1 && bagSlots[i].maxCount == 1)//只能放1個且是空格
            {//不可堆疊的物件檢查邏輯
                while (itemCount < count)
                {
                    bagSlots[i + itemCount].PushItem(itemData); //Debug.Log(itemData.itemName);
                    itemCount++;
                }
                break;
            }
            else if (itemData.itemID == bagSlots[i].itemID || bagSlots[i].maxCount == 1)//找到相同物品或空格
            {//可堆疊的物件檢查邏輯
                if (count <= bagSlots[i].emptyCount)
                {
                    bagSlots[i].PushItem(itemData,count);
                }
                else if(count > bagSlots[i].emptyCount)
                {
                    int lastCount = count - bagSlots[i].emptyCount;
                    bagSlots[i].PushItem(itemData, bagSlots[i].emptyCount);
                    PushItem(itemData, lastCount);//遞回將剩餘數量再次PUSH倒背包
                }
                 //Debug.Log(itemData.itemName);
                break;
            }
        }
        //背包儲存
        GameManager.ctrl.gameData.BagSave(bagSlots);
    }
}
/// <summary>
/// 目標系統
/// </summary>
public class TargetSystem
{
    private List<MonsterCtrl> monsterList;

    public TargetSystem()
    {
        monsterList = new List<MonsterCtrl>();
    }

    public void Add(MonsterCtrl monster)
    {
        monsterList.Add(monster);
    }

    public void Remove(MonsterCtrl monster)
    {
        monsterList.Remove(monster);
    }

    public void Clear()
    {
        monsterList.Clear();
    }

    public MonsterCtrl Keep(int index)
    {
        return monsterList[index];
    }

    public int Count()
    {
        return monsterList.Count;
    }
}
/// <summary>
/// buff系統
/// </summary>
public class BuffSystem
{
    UIBuffIconCtrl iconCtrl
    {
        get { return UIBuffIconCtrl.ctrl; }
    }
    float base_moveSpeed = 1;
    float addition_moveSpeed = 1;
    public float moveSpeed
    {
        get { return base_moveSpeed * addition_moveSpeed; }
    }
    List<Buff> buffs;//buff清單
    //建構式
    public BuffSystem()
    {
        buffs = new List<Buff>();
    }
    /// <summary>
    /// 更新buff全體時間
    /// </summary>
    /// <param name="time"></param>
    public void UpdateBuff(float time)
    {
        if (buffs.Count == 0) return;
        for(int i = 0;i < buffs.Count;i++)
        {
            buffs[i].Tick(time);
            if (buffs[i].timeUp) RemoveBuff(buffs[i]);
        }
    }
    /// <summary>
    /// 增加buff
    /// </summary>
    /// <param name="buff"></param>
    public void AddBuff(Buff buff)
    {
        buffs.Add(buff);//Debug.Log("加入buff:" + buff.GetType().Name);
        buff.buffIcon = iconCtrl.CreateBuffIcon(buff);
        switch (buff.GetType().Name)
        {
            case "MoveSpeedUp":
                base_moveSpeed = PlayerCtrl.ctrl.speed;
                addition_moveSpeed = ((MoveSpeedUp)buff).addition_moveSpeed;
                PlayerCtrl.ctrl.speed = moveSpeed;
                Debug.Log("移動速率" + moveSpeed.ToString());
                break;
        }

    }
    /// <summary>
    /// 移除buff
    /// </summary>
    /// <param name="buff"></param>
    public void RemoveBuff(Buff buff)
    {
        buffs.Remove(buff); //Debug.Log("移除buff:" + buff.GetType().Name);
        buff.buffIcon.gameObject.SetActive(false);
        switch (buff.GetType().Name)
        {
            case "MoveSpeedUp":
                addition_moveSpeed = 1f;
                PlayerCtrl.ctrl.speed = base_moveSpeed;
                Debug.Log("移動速率" + moveSpeed.ToString());
                break;
        }
    }
    /// <summary>
    /// 清除buff
    /// </summary>
    public void ClearBuff()
    {
        buffs.Clear();
    }
}

public class AudioSystem
{
    public AudioSource BGM
    {
        get
        {
            if (m_BGM == null) m_BGM = new GameObject("BGM_Player").AddComponent<AudioSource>();
            m_BGM.transform.SetParent(GameManager.ctrl.transform);
            return m_BGM;
        }
    }
    private AudioSource m_BGM;

    public AudioSource SFX
    {
        get
        {
            if (m_SFX == null) m_SFX = new GameObject("SFX_Player").AddComponent<AudioSource>();
            m_SFX.transform.SetParent(GameManager.ctrl.transform);
            return m_SFX;
        }
    }
    private AudioSource m_SFX;

    public void PlayBGM(AudioClip audioClip, float volume = 1f, bool loop = true, float pitch = 1f)
    {
        BGM.clip = audioClip;
        BGM.volume = volume;
        BGM.loop = loop;
        BGM.pitch = pitch;
        BGM.Play();
    }

    public void PlaySFX(AudioClip audioClip, float volume = 1f, bool loop = false, float pitch = 1f)
    {
        SFX.volume = volume;
        SFX.loop = loop;
        SFX.pitch = pitch;
        SFX.PlayOneShot(audioClip);//只播放一次
    }
}
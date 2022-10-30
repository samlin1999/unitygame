using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;//場景管理

public class GameManager : MonoBehaviour {
    public static GameManager ctrl;
    /// <summary>
    /// 給予路徑=>自動從資料庫拿取資料
    /// </summary>
	public GameDB DB
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<GameDB>("Assets/Scripts/DataBase/GameDB.asset");//Resources.Load<GameDB>("GameDB.asset");//
        }
    }
    private GameData m_gameData;
    /// <summary>
    /// 建構資料
    /// </summary>
    public GameData gameData
    {
        get
        {
            if (m_gameData == null)
                m_gameData = new GameData();
            return m_gameData;
        }
    }
    public GameObject gpsicon;
    /// <summary>
    /// player自動生成
    /// </summary>
	public playercontrol player
	{
		get
		{
            if (m_player == null)
            {
                m_player = new GameObject("Player").AddComponent<playercontrol>();
                m_player.SFX = DB.audioClip;
                Instantiate(DB.models[(int)DB.classType], m_player.transform);//實例化角色
                Instantiate(gpsicon, m_player.transform);
            }
			return m_player;
		}
	}
	private playercontrol m_player;
    /// <summary>
    /// 背包控制器
    /// </summary>
    public BagSystem bagSystem
    {
        get
        {
            if (m_BagSystem == null)
                m_BagSystem = BagSystem.ctrl;
            return m_BagSystem;
        }
    }
    private BagSystem m_BagSystem;
    /// <summary>
    /// 攝影機控制器
    /// </summary>
    public CameraSystem cameraSystem
    {
        get
        {
            if(m_cameraSystem == null)
                m_cameraSystem = CameraSystem.ctrl;
            return m_cameraSystem;
        }
    }
    private CameraSystem m_cameraSystem;
    /// <summary>
    /// 裝備控制器
    /// </summary>
    public EquipSystem equipSystem
    {
        get
        {
            if (m_equipSystem == null)
                m_equipSystem = EquipSystem.ctrl;
            return m_equipSystem;
        }
    }
    private EquipSystem m_equipSystem;
    /// <summary>
    /// 角色屬性控制器
    /// </summary>
    public characterInfo characterInfo
    {
        get
        {
            if (m_characterInfo == null)
                m_characterInfo = characterInfo.ctrl;
            return m_characterInfo;
        }
    }
    private characterInfo m_characterInfo;
    public SkillTreeSystem skillTreeSystem
    {
        get
        {
            if (m_skillTreeSystem == null)
                m_skillTreeSystem = SkillTreeSystem.ctrl;
            return m_skillTreeSystem;
        }
    }
    private SkillTreeSystem m_skillTreeSystem;
    private void Awake()
    {
        ctrl = this;
    }
    /// <summary>
    /// 關卡控制器
    /// </summary>
    public StageManager stageManager
    {
        get
        {
            if (m_stageManager == null)
                m_stageManager = new StageManager();//執行建構式
            return m_stageManager;
        }
    }
    private StageManager m_stageManager;
    public BuffManager buffManager
    {
        get
        {
            if (m_buffManager == null)
                m_buffManager = new BuffManager();//執行建構式
            return m_buffManager;
        }
    }
    private BuffManager m_buffManager;
    public MissionSystem missionSystem
    {
        get
        {
            if (m_missionSystem == null)
                m_missionSystem = new MissionSystem();
            return m_missionSystem;
        }
    }
    private MissionSystem m_missionSystem;
    public List<ISystem> systems = new List<ISystem>();
    public AudioClip bgm;
    void Start () {
        AudioManager.ctrl.PlayMusic(bgm);
		//print (DB.classType);
        player.Inital();
        cameraSystem.Inital();//呼叫攝影機初始化它的目標
        characterInfo.Inital();
        missionSystem.Inital();
        skillTreeSystem.Inital();
        InvokeRepeating("BuffUpdate", 0, 1);
        //print(stageManager.stageName);
	}
    public void BuffUpdate()
    {
        buffManager.Update();
    }
	void Update () {
        //print (DB.classType);
        //characterInfo.ctrl.SetPlayerAttr(DB.charAttrs[(int)DB.classType]);
    }
}

public class StageManager
{
    public string stageName = "Stage1";

    public StageDB stageDB
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<StageDB>("Assets/Scripts/DataBase/StageDB.asset");//Resources.Load<StageDB>("StageDB.asset");// 
        }
    }
    private string keyVal
    {
        get { return SceneManager.GetActiveScene().name;}
    }
    public List<Task> taskList;
    /// <summary>
    /// 在 new 時執行的建構式(任務資訊)
    /// </summary>
    public StageManager()//<=建構式，不寫回傳型別
    {
        foreach(stageInfo SI in stageDB.stageInfos)
        {
            taskList = new List<Task>();
            if (SI.keyVal == keyVal)
            {
                stageName = SI.stageName;
                foreach(Task task in SI.taskList)
                {
                    taskList.Add(new Task(task));
                    GameManager.ctrl.missionSystem.FindMissionEvent(task);
                }
            }           
        }
    }
    /// <summary>
    /// 檢查擊殺怪物數量
    /// </summary>
    /// <param name="code"></param>
    public void Check(string code)
    {
        
        for(int i = 0;i < taskList.Count;i++)
        {
            //用腳本經過實體物件儲存=>taskList[i](虛擬記憶體)--->taskcontents.conditionList(實體物件)
            List<Condition> list = taskList[i].conditionList;
            for(int j = 0;j < list.Count;j++)
            {
                if(code == list[j].name)
                {
                    list[j].AddToAmount();//任務進度更新
                }
            }
            taskList[i].Update();//任務更新
        }
    }
}
/// <summary>
/// 管理全部的BUFF
/// </summary>
public class BuffManager
{
    public List<IBuff> buffs;
    public float damage, defense;

    public BuffManager()
    {
        buffs = new List<IBuff>();
    }
    public void AddBuff(IBuff buff)
    {
        buffs.Add(buff);
        Refresh(buff, true);
    }
    public void AddBuff(List<IBuff> buffs)
    {
        foreach(IBuff buff in buffs)
        {
            this.buffs.Add(buff);
            Refresh(buff, true);
        }
        
    }

    public void RemoveBuff(IBuff buff)
    {
        this.buffs.Remove(buff);
        Refresh(buff, false);
    }
    public void RemoveBuff(List<IBuff> buffs)
    {
        foreach (IBuff buff in buffs)
        {
            this.buffs.Remove(buff);
            Refresh(buff, false);
        }
        
    }
    public void Update()
    {
        for(int i = 0;i < buffs.Count;i++)
        {
            buffs[i].Update();
            if(buffs[i].timer.isDone)
            {
                RemoveBuff(buffs[i]);
            }
            else
                Debug.Log(buffs[i].buffName);
        }
    }
    public void Refresh(IBuff buff,bool active)
    {
        switch (buff.buffSort)
        {
            case BuffSort.Damage:
                damage += ((AttackBuff)buff).addDamage * (active ? 1 : -1);
                break;
            case BuffSort.Defense:
                defense += ((DefenseBuff)buff).addDefense * (active ? 1 : -1);
                break;
        }

    }
}
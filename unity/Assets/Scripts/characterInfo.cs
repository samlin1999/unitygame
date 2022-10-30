using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// 人物屬性設定
/// </summary>
public class characterInfo : ISystem {
    public static characterInfo ctrl;//使自身腳本可控
    public Image hpbar, mpbar, expbar, headicon;
    public Text level, mdTxt, rgTxt, maxhpTxt, maxmpTxt, popUpTxt,stageTxt,moneyTxt;
    public Text freepoint;
    public Button applyBtn;
    [Header("血量設定")]
    public float hp;
    private float maxHp = 100f;
    [Header("魔力設定")]
    public float mp;
    private float maxMp = 100f;
    [Header("經驗條設定")]
    public float exp;
    private float maxExp = 200f, totalexp;
    [Header("金幣掉落")]
    public int money = 0;
    public int meleeDamage, rangeDamage;//近戰和遠程武器傷害
    public bool cheat;
    private int Lv = 1, maxLv = 99;
    public int freePoint = 0;
    public int TheLastLevel = 1;
    public Attributes[] attributes = new Attributes[4];
    public ClassType classType;
    /// <summary>
    /// 給予路徑=>自動從資料庫拿取資料
    /// </summary>
    public SkillDB skillDB
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<SkillDB>("Assets/Scripts/DataBase/SkillDB.asset");//Resources.Load<SkillDB>("SkillDB.asset");
        }
    }
    public List<skillBtn> skillBtns = new List<skillBtn>(4);//技能列表
    public GameObject TaskTmp;
    public Transform taskcontent
    {
        get { return TaskTmp.GetComponentInParent<Transform>(); }
    }
    private Queue<string> popUpInfo = new Queue<string>();

	void Start () {
        
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public override void Inital()
    {
        SetPlayerAttr(GM.DB.charAttrs[(int)GM.DB.classType]);
        /*skillBtns[0].setSkill(skillDB.skillInfos[0]);
        skillBtns[1].setSkill(skillDB.skillInfos[1]);
        skillBtns[2].setSkill(skillDB.skillInfos[2]);
        skillBtns[3].setSkill(skillDB.skillInfos[3]);*/
        stageTxt.text = GM.stageManager.stageName;
        moneyTxt.text = money.ToString();
        taskcontent obj = null;
        for(int i = 0;i < GM.stageManager.taskList.Count;i++)
        {
            obj = Instantiate(TaskTmp, taskcontent).GetComponent<taskcontent>();
            obj.gameObject.SetActive(true);
            GM.stageManager.taskList[i].SetTmpUI(obj);
            //GM.stageManager.taskList[i].taskcontents.SetTask(GM.stageManager.taskList[i]);
        }
    }
    /// <summary>
    /// 控制自身腳本
    /// </summary>
    public void Awake()
    {
        ctrl = this;
    }
    public void GotMoney(float M)
    {
        money += (int)M;
        moneyTxt.text = money.ToString();
    }
    /// <summary>
    /// 回傳職業對應傷害類型
    /// </summary>
    /// <returns></returns>
    public int returnDamage()
    {
        if (classType == ClassType.Archer)
            return rangeDamage;
        else if (classType == ClassType.Mage)
            return rangeDamage;
        else
            return meleeDamage;
        //return classType == ClassType.Archer ? rangeDamage : meleeDamage;
    }
    /// <summary>
    /// 回傳職業對應傷害距離
    /// </summary>
    /// <returns></returns>
    public float returnAttackRange()
    {
        if (classType == ClassType.Archer)
            return 10f;
        else if (classType == ClassType.Mage)
            return 8f;
        else
            return 3f;
    }
    /// <summary>
    /// 回傳職業對應傷害範圍(單體or複數)
    /// </summary>
    /// <returns></returns>
    public bool returnAttackType()
    {
        return classType == ClassType.Archer;
    }
    /// <summary>
    /// 初始化角色屬性
    /// </summary>
    /// <param name="charAttr"></param>
    public void SetPlayerAttr(CharAttr charAttr)//設定角色屬性
    {
        classType = charAttr.classType;
        for(int i = 0;i < attributes.Length;i++)
        {
            attributes[i].Set(charAttr.GetAttrByIndex(i));
        }
        maxHp = 200 + attributes[0].min * 10;
        maxMp = 150 + attributes[2].min * 7;
        meleeDamage = 15 + attributes[1].min * 2;
        rangeDamage = 20 + attributes[3].min * 2;
        setHp(maxHp);
        setMp(maxMp);
        setEXp(0);
        level.text = Lv.ToString();//顯示等級UI
        maxhpTxt.text = maxHp.ToString();//顯示總血量UI
        maxmpTxt.text = maxMp.ToString();//顯示總魔量UI
        mdTxt.text = meleeDamage.ToString();//
        rgTxt.text = rangeDamage.ToString();
        LoadPoint();
        applyBtn.interactable = false;
    }
    /// <summary>
    /// 刷新點數的分配情形
    /// </summary>
    public void LoadPoint()
    {
        foreach(Attributes attr in attributes)//只取值，不可修改(遍歷:每個屬性都輪過)
        {
            attr.Reload(freePoint);
        }
    }
	
	void Update () {
        if(cheat)
        {
            if (Input.GetKey(KeyCode.Keypad1)) setHp(10);
            if (Input.GetKey(KeyCode.Keypad2)) setHp(-10);
            if (Input.GetKey(KeyCode.Keypad3)) setHp(-maxHp);
            if (Input.GetKey(KeyCode.Keypad4)) setMp(10);
            if (Input.GetKey(KeyCode.Keypad5)) setMp(-10);
            if (Input.GetKey(KeyCode.Keypad6)) setMp(-maxMp);
            if (Input.GetKeyDown(KeyCode.Keypad7)) setEXp(1000);
            if (Input.GetKey(KeyCode.Keypad8)) setEXp(-10);
            if (Input.GetKey(KeyCode.Keypad9)) setEXp(-maxExp);
        }
        
	}
    /// <summary>
    /// 顯現文字
    /// </summary>
    /// <param name="info"></param>
    public void PopUp(string info)
    {
        if(popUpTxt.text == string.Empty)
            InvokeRepeating("autoClearPopUp", 3f, 3f);
        popUpInfo.Enqueue("\n" + info);
        popUpTxt.text = reflash();
    }
    /// <summary>
    /// 刷新文字
    /// </summary>
    /// <returns></returns>
    string reflash()
    {
        string line = string.Empty;
        if (popUpInfo.Count > 3)
        {
            popUpInfo.Dequeue();
        }
        foreach(string S in popUpInfo)
        {
            line += S;
        }
        return line;
    }
    /// <summary>
    /// 自動清除多餘文字
    /// </summary>
    public void autoClearPopUp()
    {
        popUpInfo.Dequeue();
        popUpTxt.text = reflash();
        if (popUpTxt.text == string.Empty)
            CancelInvoke();
    }
    /// <summary>
    /// 判斷剩餘消耗是否可使用技能
    /// </summary>
    /// <param name="costHp">技能的血量消耗</param>
    /// <param name="costMp">技能的魔量消耗</param>
    /// <returns></returns>
    public bool costCheck(float costHp = 0,float costMp = 0)
    {
        bool useHp = hp > costHp;
        bool useMp = mp >= costMp;
        if(!useHp)
        {
            PopUp("<color=black>血量不足</color>" + costHp);
        }
        if(!useMp)
        {
            PopUp("<color=black>魔量不足</color>" + costMp);
        }
        return useHp && useMp;
    }
    public bool recovery(float reHP,float reMP)
    {
        bool H = setHp(reHP);
        bool M = setMp(reMP);
        return (H && reHP > 0) || (M && reMP > 0);
    }
    /// <summary>
    /// 顯示血量
    /// </summary>
    /// <param name="F"></param>
    public bool setHp(float F)
    {
        if (F > 0 && hp == maxHp) return false;
        hp += F;
        if (hp >= maxHp)
            hp = maxHp;
        else if (hp <= 0)
            hp = 0;
        hpbar.fillAmount = hp / maxHp;
        return true;
    }
    /// <summary>
    /// 顯示魔量
    /// </summary>
    /// <param name="F"></param>
    public bool setMp(float F)
    {
        if (F > 0 && mp == maxMp) return false;
        mp += F;
        if (mp >= maxMp)
            mp = maxMp;
        else if (mp <= 0)
            mp = 0;
        mpbar.fillAmount = mp / maxMp;
        return true;
    }
    /// <summary>
    /// 顯示經驗條
    /// </summary>
    /// <param name="F"></param>
    public void setEXp(float F)
    {
        exp += F;
        float NowExp = exp - totalexp;
        while(NowExp >= maxExp)
        {
            LevelUp();//升等
            NowExp = exp - totalExp();//更新分子
            maxExp = MaxEXP(Lv);//更新分母
        }
        if(NowExp < 0)
        {
            //exp = totalexp;//不降等
            if(Lv > 1)
                LevelDown();
            if (exp <= 0)
                exp = 0;
            //level.text = Lv.ToString();//顯示等級UI
            NowExp = exp - totalExp();//更新分子
            maxExp = MaxEXP(Lv);//更新分母
        }
        if (Lv >= maxLv)
        {
            Lv = maxLv;
            expbar.fillAmount = 1;
        }
        expbar.fillAmount = NowExp / maxExp;
    }
    /// <summary>
    /// 設置每一級的升等所需經驗
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    float MaxEXP(int num)
    {
        return 200f * Mathf.Pow(2,num - 1);
    }
    /// <summary>
    /// 前一級的累積經驗
    /// </summary>
    /// <returns></returns>
    float totalExp()
    {
        totalexp = 0f;
        for(int i = 1;i < Lv;i++)
        {
            totalexp += MaxEXP(i);
        }
        return totalexp;
    }
    /// <summary>
    /// 升級會做的事
    /// </summary>
    void LevelUp()
    {
        Lv++;
        PopUp("<color=black>恭喜升到</color>" + Lv + "<color=black>等!!</color>");
        if(TheLastLevel < Lv)
        {
            TheLastLevel++;
            freePoint += 2;
            freepoint.text = freePoint.ToString();
        }
        level.text = Lv.ToString();//顯示等級UI
        LoadPoint();
        setHp(maxHp);
        setMp(maxMp);
    }
    /// <summary>
    /// 降級會做的事
    /// </summary>
    void LevelDown()
    {
        Lv--;
        level.text = Lv.ToString();//顯示等級UI
    }
    /// <summary>
    /// 添加技能點
    /// </summary>
    /// <param name="index"></param>
    public void addPoint(int index)
    {
        if (!Attributes.LockPoint)
            applyBtn.interactable = true;
        attributes[index].Add();
        freePoint--;
        freepoint.text = freePoint.ToString();
        LoadPoint();
        
    }
    /// <summary>
    /// 減少技能點
    /// </summary>
    /// <param name="index"></param>
    public void releasePoint(int index)
    {
        
        attributes[index].release();
        freePoint++;
        freepoint.text = freePoint.ToString();
        if (NoPointUse())
            applyBtn.interactable = false;
        LoadPoint();
    }
    /// <summary>
    /// 確定添加技能點
    /// </summary>
    public void ApplyPoint()
    {
        for(int i = 0;i < attributes.Length;i++)
        {
            attributes[i].Apply();
        }
        LoadPoint();
        applyBtn.interactable = false;

    }
    /// <summary>
    /// 判斷是否使用點數
    /// </summary>
    /// <returns></returns>
    bool NoPointUse()
    {
        bool check = true;
        foreach(Attributes attr in attributes)
        {
            if(attr.GetTmpPoint())
            {
                check = false;
                break;
            }
        }
        return check;
    }
}

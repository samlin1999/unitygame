using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoSystem : MonoBehaviour {
    public static InfoSystem ctrl;
    public UIPanelSwitch panelSwitch;
    public Canvas canvas
    {
        get { return panelSwitch.GetComponent<Canvas>(); }
    }
    public UIToggleCtrl toggleCtrl;
    public Image headIcon, expbar;
    public Text lv_text, energyText, moneyText, gemText;
    private int level = 1, levelMax = 30;
    private int energy, money, gem, maxEnergy = 200;
    private float exp = 0f, expMax = 100f, expNow;
    public bool reset;
    private GameData gameData;
    public Timer timer = new Timer(1f);

    private void Awake()
    {
        ctrl = this;
    }

    public void Initial () {
        gameData = new GameData();//在記憶體空間中創建儲存資料的位置
        lv_text.text = level.ToString();
        expbar.fillAmount = exp / expMax;
        if (reset) PlayerPrefs.DeleteAll();
        expCtrl(gameData.LoadExp());
        moneyCtrl(gameData.LoadMoney());
        energyCtrl(gameData.LoadEnergy(maxEnergy));
        gemCtrl(gameData.Loadgem());
	}
	
	void Update () {
        RecoveryEnergy();
        if (Input.GetKey(KeyCode.Keypad1))
            expCtrl(10);
        if (Input.GetKey(KeyCode.Keypad2))
            expCtrl(-1);
        if (Input.GetKey(KeyCode.Keypad3))
            moneyCtrl(100);
        if (Input.GetKey(KeyCode.Keypad4))
            moneyCtrl(-1000);
        if (Input.GetKey(KeyCode.Keypad5))
            gemCtrl(100);
        if (Input.GetKey(KeyCode.Keypad6))
            gemCtrl(-1000);
        if (Input.GetKey(KeyCode.Keypad7))
            energyCtrl(10);
        if (Input.GetKey(KeyCode.Keypad8))
           energyCtrl(-100);
    }
    public void Switchpanel(bool B)
    {
        panelSwitch.Switch(B);
        if (B) Invoke("SetCamera", 1f);
    }

    public void SetCamera()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        toggleCtrl.CameraOffset();
    }
    public void RecoveryEnergy()
    {
        if (energy >= maxEnergy) return;
        if(timer.isDone)
        {
            timer.Start();
            energyCtrl(1);
        }
    }
    public bool moneyCtrl(int money)
    {
        if ((this.money + money) < 0) return false;//阻止執行，無法扣除未擁有之數
        this.money += money;
        this.money = Mathf.Clamp(this.money,0,999999999);//數值上下限制，等同於下方註解程式碼
        /*if (this.money < 0)
            this.money = 0;*/
        moneyText.text = this.money.ToString();//this為本腳本下的變數
        gameData.money = this.money;//暫存資料
        gameData.Save(SaveType.MONEY);//儲存資料
        return true;//完成邏輯
    }

    public bool gemCtrl(int gem)
    {
        if ((this.gem + gem) < 0) return false;//阻止執行
        this.gem += gem;
        this.gem = Mathf.Clamp(this.gem, 0, 999999999);//數值上下限制，等同於下方註解程式碼
        /*if (this.gem < 0)
            this.gem = 0;*/
        gemText.text = this.gem.ToString();//this為本腳本下的變數
        gameData.gem = this.gem;//暫存資料
        gameData.Save(SaveType.GEM);//儲存資料
        return true;//完成邏輯
    }

    public bool energyCtrl(int energy)
    {
        if ((this.energy + energy) < 0) return false;//阻止執行
        this.energy += energy;
        this.energy = Mathf.Clamp(this.energy, 0, maxEnergy);//數值上下限制，等同於下方註解程式碼
        /*if (this.energy < 0)
            this.energy = 0;
        if (this.energy > maxEnergy)
            this.energy = maxEnergy;*/
        energyText.text = this.energy.ToString() + "/" + maxEnergy.ToString();//this為本腳本下的變數
        gameData.energy = this.energy;//暫存資料
        gameData.Save(SaveType.ENERGY);//儲存資料
        return true;//完成邏輯
    }

    public void expCtrl(float F)
    {
        exp += F;
        if (level >= levelMax)
            return;
        expNow = exp - TotalExp();

        while(expNow >= expMax)
        {
            levelUp();
            expNow = exp - TotalExp();
        }
        expbar.fillAmount = expNow / expMax;
        gameData.exp = exp;//暫存資料
        gameData.Save(SaveType.EXP);//儲存資料
    }

    public void levelUp()
    {
        level++;
        lv_text.text = level.ToString();
        expMax = 100 * level;
        if(level > gameData.LoadLevel())
        {
            Debug.Log("取得升級獎勵");
            gameData.level = level;//暫存資料
            gameData.Save(SaveType.LEVEL);//儲存資料
        }
        
    }
    /// <summary>
    /// 當級之前的經驗總合
    /// </summary>
    /// <returns></returns>
    public float TotalExp()
    {
        float totalExp = 0;
        for(int i = 0;i < level;i++)
        {
            totalExp += 100 * i;
        }
        return totalExp;
    }
}

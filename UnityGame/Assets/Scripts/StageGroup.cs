using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageGroup : MonoBehaviour {

    public StageDB DB;
    List<StageInfo> stageInfos = new List<StageInfo>();
    /// <summary>
    /// 關卡按鈕資料
    /// </summary>
    [System.Serializable]
    public struct StageBtn
    {
        public Button btn;
        public Text lab;
        public int cost;
    }
    /// <summary>
    /// 關卡按鈕資訊宣告
    /// </summary>
    public StageBtn[] stageBtns = new StageBtn[5];
    void Start() {

    }
    /// <summary>
    /// 設置關卡資訊及按鈕資訊
    /// </summary>
    /// <param name="stageInfos"></param>
    public void SetStageInfo(List<StageInfo> stageInfos)
    {
        gameObject.SetActive(true);
        this.stageInfos = stageInfos;
        for (int i = 0; i < stageBtns.Length; i++)
        {
            string stageName = this.stageInfos[i].stageName;
            int cost = this.stageInfos[i].cost;
            stageBtns[i].lab.text = stageName;
            stageBtns[i].cost = cost;
            stageBtns[i].btn.onClick.AddListener(delegate { GoToStage(stageName, cost); });//啟動按鈕事件
        }
    }
    /// <summary>
    /// 進關卡
    /// </summary>
    void GoToStage(string stageName, int cost)
    {
        print("in stage: " + stageName + " cost: " + cost);
        if(GameManager.ctrl.infoSystem.energyCtrl(-cost))
        {
            SceneManager.LoadScene(stageName);
            GameManager.ctrl.infoSystem.Switchpanel(false);
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageSystem : MonoBehaviour {
    public static StageSystem ctrl;
    public StageDB stageDB;
    public StageGroup StageGroupTMP;

    private void Awake()
    {
        ctrl = this;
    }

    public void Initial()
    {
		for(int i = 0;i < stageDB.sceneInfos.Count;i++)
        {
            Instantiate(StageGroupTMP, transform).SetStageInfo(stageDB.sceneInfos[i].stageInfos);//具現化
            GetComponentInParent<UIStageSwitch>().Initial();
        }
	}
	
}

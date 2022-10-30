using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FXSystem : MonoBehaviour {
    /// <summary>
    /// 自動生成物件且不會重覆建置，其他腳本也可呼叫此腳本
    /// </summary>
    public static FXSystem ctrl
    {
        get
        {
            if (fxSystem == null) fxSystem = new GameObject("FXSystem").AddComponent<FXSystem>();
            return fxSystem;
        }
    }
    private static FXSystem fxSystem;
    /// <summary>
    /// 給予路徑=>自動從資料庫拿取資料
    /// </summary>
    public EffectDB effectDB
    {
        get
        {
            return  AssetDatabase.LoadAssetAtPath<EffectDB>("Assets/Scripts/DataBase/EffectDB.asset");//Resources.Load<EffectDB>("EffectDB.asset");//
        }
    }

    /*
    private void Awake()
    {
        ctrl = this;
    }*/
    void Start () {
		
	}
	
	void Update () {
		
	}
    /// <summary>
    /// 製造特效
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="num"></param>
    public void callFX(Transform spawnPoint,int num = 0)
    {
        Instantiate(effectDB.fxList[num].fx, spawnPoint.position ,Quaternion.identity );
    }
}

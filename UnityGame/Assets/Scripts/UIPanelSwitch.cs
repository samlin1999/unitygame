using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 自動附加相關腳本(屬性修飾詞)
/// </summary>
/// <param name=""></param>
[RequireComponent(typeof(CanvasGroup))]
public class UIPanelSwitch : MonoBehaviour {
    /// <summary>
    /// 控制面板顯示
    /// </summary>
    public CanvasGroup CG
    {
        get { return GetComponent<CanvasGroup>(); }
    }

	
	void Start () {
        //CG.alpha = 0;//視覺上的透明
        //CG.blocksRaycasts = false;//觸控取消
    }
	
	void Update () {
		
	}

    public void Switch(bool sw)
    {
        CG.alpha = sw ? 1 : 0;
        CG.blocksRaycasts = sw;
        /*if(sw)
        {
            CG.alpha = 1;
            CG.blocksRaycasts = true;
        }
        else
        {
            CG.alpha = 0;
            CG.blocksRaycasts = false;
        }*/
    }
    /// <summary>
    /// 自動開啟/關閉面板
    /// </summary>
    public void Switch()
    {
        CG.alpha = CG.blocksRaycasts ? 0 : 1;//可觸控時，接下來要關
        CG.blocksRaycasts = !CG.blocksRaycasts;
    }
}

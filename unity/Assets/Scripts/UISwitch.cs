using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 切換面板開關
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class UISwitch : MonoBehaviour {
    public CanvasGroup CG
    {
        get { return GetComponent<CanvasGroup>(); }
    }
	void Start () {
        CG.alpha = 0;
        CG.blocksRaycasts = false;
	}
    public void Switch()
    {
        CG.alpha = CG.blocksRaycasts ? 0 : 1;
        CG.blocksRaycasts = !CG.blocksRaycasts;           
    }
    public void Switch(bool B)
    {
        CG.alpha = B ? 1 : 0;
        CG.blocksRaycasts = B;
    }
}

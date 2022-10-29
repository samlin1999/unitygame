using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStageSwitch : MonoBehaviour {
    public bool loop;
    /// <summary>
    /// 遮罩面板取得
    /// </summary>
    public RectTransform RT
    {
        get { return GetComponent<RectTransform>(); }
    }
    /// <summary>
    /// 內容面板
    /// </summary>
    public RectTransform contentRT;
    /// <summary>
    /// 選擇stage
    /// </summary>
    int index = 0;
    /// <summary>
    /// 場景數
    /// </summary>
    int count
    {
        get { return contentRT.childCount - 1; }//子物件總數減去模板TMP(不顯示的)
    }
    Vector2 pos;
    /// <summary>
    /// 面板寬度
    /// </summary>
    public float panelWidth
    {
        get { return RT.sizeDelta.x; }
    }

	public void Initial () {
        contentRT.sizeDelta = new Vector2(panelWidth * count, contentRT.sizeDelta.y);
	}
	
    public void Right()
    {
        index++;
        index = (index >= count) ? (loop ? 0 : count - 1) : index;
        /*if (index >= count)
            index = 0;*/
        pos.x = -panelWidth * index;
        contentRT.anchoredPosition = pos;//面板座標
    }

    public void Left()
    {
        index--;
        index = (index < 0) ? (loop ? count - 1 : 0) : index;
        /*if (index < 0)
            index = count - 1;*/
        pos.x = -panelWidth * index;
        contentRT.anchoredPosition = pos;
    }
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuffIconCtrl : MonoBehaviour {
    public static UIBuffIconCtrl ctrl;
    public UIBuffIcon buffIconTMP;
    List<UIBuffIcon> buffIcons = new List<UIBuffIcon>();
    private void Awake()
    {
        ctrl = this;
    }
    /// 建立BUFF的UI圖示
    /// </summary>
    /// <param name="buff">BUFF資料</param>
    /// <returns>回傳對應的UI控件</returns>
    public UIBuffIcon CreateBuffIcon(Buff buff)
    {
        UIBuffIcon buffIcon = null;
        for(int i = 0;i < buffIcons.Count;i++)
        {
            if(!buffIcons[i].gameObject.activeSelf)
            {
                buffIcon = buffIcons[i];
                buffIcon.SetBuffData(buff);
                break;
            }
        }
        if(buffIcon == null || buffIcons.Count == 0)
        {
            buffIcon = Instantiate(buffIconTMP, transform);
            buffIcon.SetBuffData(buff);
            buffIcons.Add(buffIcon);//加入回收清單
        }
        return buffIcon;
    }
    
}

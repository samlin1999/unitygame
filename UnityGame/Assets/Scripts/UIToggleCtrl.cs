using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleCtrl : MonoBehaviour {
    
    public UIPanelSwitch obj1, obj2, obj3;
    public int offset = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// 切換選取框
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleSwitch(Toggle toggle)
    {
        switch (toggle.name)
        {
            case "Toggle (關卡)":
                obj1.Switch(toggle.isOn);
                offset = 10;
                CameraOffset();
                break;
            case "Toggle (角色)":
                obj2.Switch(toggle.isOn);
                offset = 0;
                CameraOffset();
                break;
            case "Toggle (技能)":
                obj3.Switch(toggle.isOn);
                offset = 10;
                CameraOffset();
                break;
            default:
                break;
        }
    }

    public void CameraOffset(int offset)
    {
        Camera.main.transform.position = Vector3.up * offset;//位移主攝影機(不顯示模型)
    }

    public void CameraOffset()
    {
        Camera.main.transform.position = Vector3.up * offset;//位移主攝影機(不顯示模型)
    }
}

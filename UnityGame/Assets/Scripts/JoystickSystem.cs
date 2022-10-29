using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface ITest
{
    void Test();
}
public class JoystickSystem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler {
    public static JoystickSystem ctrl;
    public RectTransform padBG, handle;
    public bool keyboardTest;
    private void Awake()
    {
        ctrl = this;
    }
    /// <summary>
    /// 判斷是否使用搖桿
    /// </summary>
    public bool PadInUse
    {
        get { return handlePos != Vector2.zero; }
    }
    /// <summary>
    /// X軸的外部接口
    /// </summary>
    public float axisX
    {
        get
        {
            return handlePos.x;
        }
    }
    /// <summary>
    /// Y軸的外部接口
    /// </summary>
    public float axisY
    {
        get
        {
            return handlePos.y;
        }
    }
    /// <summary>
    /// PAD方向角的接口(以正12點方位為0)
    /// </summary>
    public float padAng
    {
        get
        {
            return Vector2.Angle(Vector2.up, handlePos) * Mathf.Sign(axisX);
        }
    }

    Vector2 handlePos;
    /// <summary>
    /// 開始拖曳PAD
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
    /// <summary>
    /// PAD拖曳中
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle
            (padBG,eventData.position,eventData.pressEventCamera,out handlePos))
        {   
            //Handle視覺修正(先修正，後整理數據)
            if (handlePos.magnitude > (padBG.sizeDelta.x / 2) * 0.7f)
                handlePos = handlePos.normalized * (padBG.sizeDelta.x / 2) * 0.7f;
            handle.anchoredPosition = handlePos;
            //從中心點計算
            handlePos.x = handlePos.x / padBG.sizeDelta.x / 2;
            handlePos.y = handlePos.y / padBG.sizeDelta.y / 2;
            //限制方向向量百分比為 +1 ~ -1之間
            if (handlePos.magnitude > 1f)
                handlePos.Normalize();
            //print(handlePos);
            
        }
    }
    /// <summary>
    /// 放開PAD
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        handlePos = Vector2.zero;
        handle.anchoredPosition = handlePos;//錨點座標
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if(keyboardTest)
        {
            Vector2 keyPos = Vector2.zero;
            keyPos.x = Input.GetAxis("Horizontal");
            keyPos.y = Input.GetAxis("Vertical");
            handlePos = keyPos;
        }
    }
}

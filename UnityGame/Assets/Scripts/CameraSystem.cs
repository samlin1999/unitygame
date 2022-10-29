using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public struct CameraPosREC
{
    public Vector3 offset;
    public float distance;
    public float angX;
    public float angY;

    public void Set(Vector3 offset, float distance, float angX, float angY)
    {
        this.offset = offset;
        this.distance = distance;
        this.angX = angX;
        this.angY = angY;
    }
}

public class CameraSystem : MonoBehaviour {
    public static CameraSystem ctrl;
    public Transform target;
    Transform focusPos;
    [Header("跟隨距離")]
    [Range(minDis,maxDis)]
    public float distance = 7;
    //單一常數統一控制距離範圍限制
    const float minDis = 2f, maxDis = 15f;
    [Header("跟隨視角")]
    public Vector3 posOffset;
    [Range(minAngX, maxAngX)]
    public float angX = 15f;
    const float minAngX = 15f, maxAngX = 89f;
    [Range(minAngY, maxAngY)]
    public float angY = 5f;
    const float minAngY = 0f, maxAngY = 360f;
    [Range(1f, 10f)]
    public float rotspeed = 5f;
    
    Vector3 m_angle;
    Vector2 ScreenPos;
    private void Awake()
    {
        ctrl = this;
    }
    public Vector3 angle
    {
        get
        {
            m_angle.x = angX;
            m_angle.y = angY;
            return m_angle;
        }
    }
    CameraPosREC posREC;
    // Use this for initialization
    void Start () {
        focusPos = new GameObject("CameraFocusPos").transform;
        transform.SetParent(focusPos);
	}
	
	// Update is called once per frame
	void Update () {
        CameraFollow();
        if (Input.GetKeyDown(KeyCode.KeypadDivide)) CameraMovement(new Vector3(1.5f, -0.5f, 0), 2, 5, 250);
        if (Input.GetKeyDown(KeyCode.KeypadMultiply)) CameraBack();
    }
    /// <summary>
    /// 鏡頭跟隨
    /// </summary>
    void CameraFollow()
    {
        Vector3 newPos = focusPos.position + Angle() * Distance();
        //Lerp線性差值，做漸進式跟隨(增加流暢感)
        transform.position = Vector3.Lerp(transform.position,newPos, Time.deltaTime * 7);
        focusPos.position = target.position + posOffset;
        transform.LookAt(focusPos);//座標位置計算完畢後，再專注目標(減少晃動)
        
    }
    /// <summary>
    /// 運鏡功能
    /// </summary>
    /// <param name="pos">焦點偏移量</param>
    /// <param name="distance">鏡頭距離</param>
    /// <param name="angX">仰角</param>
    /// <param name="angY">旋轉角</param>
    public void CameraMovement(Vector3 pos, float distance, float angX, float angY)
    {
        posREC.Set(posOffset, this.distance, this.angX, this.angY);//紀錄原來位置
        posOffset = pos;
        this.distance = distance;
        this.angX = angX;
        this.angY = angY;
    }
    /// <summary>
    /// 復原原本鏡頭的位置
    /// </summary>
    public void CameraBack()
    {
        posOffset = posREC.offset;
        distance = posREC.distance;
        angX = posREC.angX;
        angY = posREC.angY;
    }
    /// <summary>
    /// 鏡頭和物體的距離
    /// </summary>
    /// <returns></returns>
    Vector3 Distance()
    {
        distance -= Input.mouseScrollDelta.y;//滑鼠滾輪
        distance = (distance <= minDis) ? minDis : ((distance >= maxDis) ? maxDis : distance);
        /*if (distance <= minDis) distance = minDis;
        if (distance >= maxDis) distance = maxDis;*/
        
        return Vector3.back * distance;
    }
    /// <summary>
    /// 攝影機投射和地面的夾角
    /// </summary>
    /// <returns></returns>
    Quaternion Angle()
    {
        if(!JoystickSystem.ctrl.PadInUse)//操作PAD時不旋轉鏡頭
        {
            if(Input.GetMouseButtonDown(0))
            {
                ScreenPos = Input.mousePosition;
            }
            if(Input.GetMouseButton(0))
            {
                angY += (Input.mousePosition.x > ScreenPos.x + 10) ? rotspeed :
                    ((Input.mousePosition.x < ScreenPos.x - 10) ? -rotspeed : 0);
                angY = (angY <= minAngY) ? maxAngY : ((angY >= maxAngY) ? minAngY : angY);
                angX += (Input.mousePosition.y > ScreenPos.y + 10) ? rotspeed :
                    ((Input.mousePosition.y < ScreenPos.y - 10) ? -rotspeed : 0);
                angX = (angX <= minAngX) ? minAngX : ((angX >= maxAngX) ? maxAngX : angX);

                ScreenPos = Input.mousePosition;
            }
        }
        
        return Quaternion.Euler(angle);
    }
}

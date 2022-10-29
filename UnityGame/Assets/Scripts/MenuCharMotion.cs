using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCharMotion : MonoBehaviour
{
    public Animator animator;//要操作的物件
    float idleSpeed = 1f;//動畫檔速度
    Vector3 startPos;//起始點
    float dizzyNum;//操作導致動作的判斷數值
    bool isDizzy;//是否暈眩
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        idleSpeed -= Time.deltaTime * 0.2f;//速度隨時間遞減
        idleSpeed = Mathf.Clamp(idleSpeed, 1f, 2.5f);//限制速度範圍
        animator.SetFloat("speed", idleSpeed);//刷新數據
    }

    private void OnMouseDown()
    {
        startPos = Input.mousePosition;//初始點擊位置
        animator.SetInteger("type", Random.Range(1, 3));//range為1 ~ 2
        Invoke("ResetAni", 0.5f);//還原idle動畫 0.5秒，invoke呼叫函式
        idleSpeed += 0.5f;
    }

    void ResetAni()
    {
        animator.SetInteger("type", 0);//idle動畫
    }

    private void OnMouseDrag()
    {
        if (isDizzy) return;//阻擋後續程式執行
        Vector3 tmpPos = Input.mousePosition - startPos;//拖曳中的位置紀錄
        //print(Mathf.Sign(tmpPos.x));//只顯示正負(對應起始點)
        transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y - tmpPos.x, 0);//人物角度
        if(dizzyNum >= 7)
        {
            animator.SetTrigger("dead");
            dizzyNum = 0;
            isDizzy = true;//確認觸發
            Invoke("ResetDizzy", 2f);//延遲觸發2秒
        }
        else if(tmpPos.x > 50f)
        {
            dizzyNum += 1f;
        }
        startPos = Input.mousePosition;//更新停止位置為起始位置
    }

    void ResetDizzy()
    {
        isDizzy = false;
    }

    private void OnMouseUp()
    {
        dizzyNum = 0;
    }
}

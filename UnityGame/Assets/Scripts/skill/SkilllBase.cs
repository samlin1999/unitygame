using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MoveType { None, Fly , Parabola}//列舉移動模式

public abstract class SkilllBase : MonoBehaviour
{
    [Header("移動行為")]
    public MoveType moveType;
    public float moveSpeed = 5;

    [Header("活動範圍限制")]
    public float liveRange = 5f;
    public float liveTime = 1;
    Vector3 startPos;

    [Header("拓展屬性")]
    [Tooltip("是否穿透")]
    public bool isPenet;
    [Tooltip("觸發次數")]
    public int hitCount = 1;
    [Tooltip("延遲時間")]
    public float duration = 2;

    [Header("觸發特效")]
    public GameObject hitEffect;
    public GameObject endEffect;

    [Header("附加功能")]
    public UnityEvent events;

    [Header("基本參數")]
    public float damage = 20;
    public float attackRange = 1;
    public float searchRange = 10;
    public float attackCD = 0.05f;
    protected MonsterCtrl getTarget = null;//最近目標的暫存
    Timer m_attackTimer;
    public Timer attackTimer
    {
        get
        {
            if (m_attackTimer == null) m_attackTimer = new Timer(attackCD);
            return m_attackTimer;
        }
    }

    public virtual void ActionStart()
    {
        //技能誕生點紀錄
        startPos = transform.position;
        GameManager.ctrl.PlaySFX(GetType().Name);
    }

    public virtual void Action()
    {
        //超出活動範圍銷毀
        if (Vector3.Distance(transform.position, startPos) >= liveRange)
            ActionEnd();
        //超出活動時間銷毀
        if (Time.deltaTime >= liveTime)
            ActionEnd();
        //移動資訊
        Move();
    }

    public virtual void ActionEnd()
    {
        GameManager.ctrl.PlaySFX(GetType().Name + "End");
        if (endEffect) Instantiate(endEffect,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
    void Move()
    {
        switch (moveType)
        {
            case MoveType.Fly:
                transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
                break;
            case MoveType.Parabola:
                break;
        }

    }
    public void Damage()
    {
        getTarget.Hurt(damage);
        if (hitEffect) Instantiate(hitEffect, getTarget.transform.position + Vector3.up, Quaternion.identity);
    }

    /// <summary>
    /// 單體攻擊
    /// </summary>
    /// <param name="isPenet"></param>
    public virtual void SingleAttack(bool isPenet)
    {
        float dis = 999f;//距離檢測標準值，一直縮短到最小為止
        //目標搜索與鎖定
        for (int i = 0; i < GameManager.ctrl.targetSystem.Count(); i++)
        {
            MonsterCtrl target = GameManager.ctrl.targetSystem.Keep(i);
            
            //半球體檢測範圍
            if (transform.CircleChecker(target.transform, searchRange, 180))
            {
                //取得當前符合攻擊範圍的目標，與打擊者(技能)的距離
                float getDis = Vector3.Distance(transform.position, target.transform.position);
                if(dis > getDis)//比對最短距離紀錄，更短就更新紀錄
                {
                    dis = getDis;
                    getTarget = target;
                }
            }
        }
        if (getTarget == null) return;
        transform.LookAt(getTarget.transform);//看向目標
        if(Vector3.Distance(transform.position,getTarget.transform.position) < attackRange)
        {
            //單體攻擊的觸發
            hitCount--;
            if (events.GetPersistentEventCount() < 0) events.Invoke();
            getTarget = null;
        }    
        if (hitCount <= 0)
        {
            ActionEnd();
        }
        else if(!isPenet)
        {
            ActionEnd();
        }
        
    }
    /// <summary>
    /// 範圍攻擊
    /// </summary>
    /// <param name="isPenet"></param>
    public virtual void AreaAttack(bool isPenet)
    {
        if (hitCount < 0) return;
        Queue<MonsterCtrl> monsters = new Queue<MonsterCtrl>();
        //目標搜索與鎖定
        for (int i = 0; i < GameManager.ctrl.targetSystem.Count(); i++)
        {
            MonsterCtrl target = GameManager.ctrl.targetSystem.Keep(i);
            //半球體檢測範圍
            if (transform.CircleChecker(target.transform, attackRange, 360))
            {
                monsters.Enqueue(target);
            }
        }
        //操作排隊的物件，操作後移除
        while (monsters.Count > 0)
        {
            getTarget = monsters.Dequeue();//取出(順便移除)
            events.Invoke();
        }
        //範圍攻擊的觸發位置
        hitCount--;
        if (hitCount <= 0)
        {
            ActionEnd();
        }
        else if (!isPenet)
        {
            ActionEnd();
        }
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterActionStatus { Idle,Move,Attack}
[RequireComponent(typeof(CharacterController))]
public class MonsterCtrl : MonoBehaviour {
    public MonsterActionStatus MAS;
    public float hp, hpMax = 100;
    public Animator animator;
    [Header("行動範圍參數")]
    public float spawnRange = 15f;//以重生點為中心的活動範圍
    public float searchRange = 10f;//搜尋範圍(找攻擊對象)
    public float idleRange = 5f;//怪物巡邏範圍(自主隨機移動)
    public bool isPatrol;//是否巡邏
    [Header("攻擊參數")]
    public float attackRange = 2f;
    public float speed = 1f;
    public int dropCount = 0, dropCountRange = 2; 
    public int dropMoney = 100, dropMoneyRange = 50;
    int giveExp = 100;
    public DropChanceList dropChance;
    Dice dropItemDice;
    float chanceTotal = 0;

    /// <summary>
    /// 角色控制物件
    /// </summary>
    public CharacterController player
    {
        get { return GetComponent<CharacterController>(); }
    }
    PlayerCtrl target
    {
        get { return PlayerCtrl.ctrl; }
    }
    /// <summary>
    /// 出生座標
    /// </summary>
    Vector3 spawnPos;
    Timer patrolCD = new Timer(1);
    Timer m_attackCD;
    Timer attackCD
    {
        get
        {
            if (m_attackCD == null)
            {
                for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
                {
                    if (animator.runtimeAnimatorController.animationClips[i].name == "Attack")
                    {
                        m_attackCD = new Timer(animator.runtimeAnimatorController.animationClips[i].length + 0.1f);
                        break;
                    }
                }
            }
            return m_attackCD;
        }
    }
    void Start () {
        //怪物上場時，通報目標管理系統
        GameManager.ctrl.targetSystem.Add(this);
        hp = hpMax;
        spawnPos = transform.position;//紀錄出生座標
        for(int i = 0;i < dropChance.dropInfos.Count;i++)//掉落機率加總
        {
            chanceTotal += dropChance.dropInfos[i].chance;
        }
        dropItemDice.SetDice(0, chanceTotal);//製作骰子
    }
    // 固定執行間隔Update
    void FixedUpdate () {
        if(!transform.position.DistanceChecker(spawnPos,spawnRange))
        {
            transform.position = spawnPos;
        }
        else if(transform.CircleChecker(target.transform, attackRange,360))
        {
            MAS = MonsterActionStatus.Attack;
            
        }
        else if (transform.CircleChecker(target.transform, searchRange, 360))
        {
            MAS = MonsterActionStatus.Move;
        }
        else
        {
            MAS = MonsterActionStatus.Idle;
        }
        Action();

    }
    void Action()
    {
        switch (MAS)
        {
            case MonsterActionStatus.Idle:
                if(isPatrol)
                {
                    animator.SetBool("RUN", true);
                    player.SimpleMove(transform.forward * speed * 0.7f);
                    if(!transform.position.DistanceChecker(spawnPos,idleRange))
                    {
                        if(patrolCD.isDone)
                        {
                            patrolCD.Start();
                            transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.y
                                + Random.Range(90, 360), Vector3.up);//隨機轉向
                        }
                    }
                }
                else
                {
                    animator.SetBool("RUN", false);
                }
                
                break;
            case MonsterActionStatus.Move:
                animator.SetBool("RUN", true);
                player.SimpleMove(transform.forward * speed);
                transform.LookAt(target.transform);
                break;
            case MonsterActionStatus.Attack:
                animator.SetBool("RUN", false);
                if(attackCD.isDone)
                {
                    attackCD.Start();
                    animator.SetTrigger("ATTACK");
                    PlayerCtrl.ctrl.Hurt(1);
                }
                break;
        }
    }

    public void Hurt(float damage)
    {
        hp -= damage;
        GameManager.ctrl.PlaySFX("普攻命中");
        if(hp <= 0)
        {
            DropItem(dropCount.DropItemCountRandom(dropCountRange));//依照數量掉落物品
            GameManager.ctrl.MoneyCtrl(dropMoney.DropMoneyRandom(dropMoneyRange));//掉落金幣
            GameManager.ctrl.GiveExp(giveExp);//給予經驗
            GameManager.ctrl.targetSystem.Remove(this);//目標系統清單移除
            GameManager.ctrl.PlaySFX("掉落金幣");
            Destroy(gameObject);
        }
    }

    public void DropItem(int count)
    {
        for(int i = 0;i < count;i++)
        {
            //機率運算
            float chance = dropItemDice.Report(); 
            for(int j = 0;j < dropChance.dropInfos.Count;j++)
            {
                if(chance < dropChance.dropInfos[j].chance)
                {
                    GameManager.ctrl.DropItem(dropChance.dropInfos[j].ItemID);
                    break;
                }
                else
                {
                    chance -= dropChance.dropInfos[j].chance;
                }
            }
        }
    }
}

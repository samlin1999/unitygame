using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionStatus { Normal, Attack}
[RequireComponent(typeof(CharacterController))]
public class PlayerCtrl : MonoBehaviour {
    public static PlayerCtrl ctrl;
    ActionStatus actionStatus;
    MonsterCtrl target; 
    public Animator animator;
    public float hp, hpMax = 100;
    /// <summary>
    /// 角色控制物件
    /// </summary>
    public CharacterController player
    {
        get { return GetComponent<CharacterController>(); }
    }
    [Range(1,10)]
    public float speed = 5f;
    //自動取得攻擊動畫時間，並且設定計時器間隔
    Timer m_attackCD;
    Timer attackCD
    {
        get
        {
            if (m_attackCD == null)
            {
                for(int i = 0;i < animator.runtimeAnimatorController.animationClips.Length;i++)
                {
                    if(animator.runtimeAnimatorController.animationClips[i].name == "Attack")
                    {
                        m_attackCD = new Timer(animator.runtimeAnimatorController.animationClips[i].length + 0.1f);
                    }
                }
            }
            return m_attackCD;
        }
    }
    //技能發射點
    public Vector3 skillSpawnPoint
    {
        get { return transform.position + transform.forward; }
    }
    void Awake()
    {
        ctrl = this;
    }
    Queue<MonsterCtrl> monsterQueue = new Queue<MonsterCtrl>();//隊列，先進先出
    void Start()
    {
        hp = hpMax;
        GameManager.ctrl.HpBarCtrl(hp, hpMax);
    }
    public void AutoAttack()
    {
        actionStatus = ActionStatus.Attack;
    }
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            actionStatus = ActionStatus.Attack;
        }
        else
        {
            actionStatus = ActionStatus.Normal;
        }
        switch(actionStatus)
        {
            case ActionStatus.Attack:
                Attack();
                break;
            case ActionStatus.Normal:
                if(attackCD.isDone) Move();
                break;
        }
	}
    /// <summary>
    /// 移動行為
    /// </summary>
    void Move()
    {
        if (JoystickSystem.ctrl.PadInUse)
        {
            animator.SetBool("RUN", true);
            player.SimpleMove(transform.forward * speed);
            transform.rotation = Quaternion.AngleAxis(CameraSystem.ctrl.angY + JoystickSystem.ctrl.padAng, Vector3.up);
        }
        else
        {
            animator.SetBool("RUN", false);
        }
    }
    /// <summary>
    /// 攻擊行為
    /// </summary>
    void Attack()
    {
        if(attackCD.isDone)
        {
            attackCD.Start();//開始計時
            animator.SetTrigger("ATTACK");
            //目標搜索與鎖定
            for(int i = 0;i < GameManager.ctrl.targetSystem.Count();i++)
            {
                target = GameManager.ctrl.targetSystem.Keep(i);
                if(transform.CircleChecker(target.transform,3,180))
                {
                    monsterQueue.Enqueue(target);//加入目標清單
                }
            }
            while (monsterQueue.Count > 0)
            {
                monsterQueue.Dequeue().Hurt(50);// => target.Hurt(50); queue中取出的目標可呼叫原本目標所有的屬性和函式
            }
            //print(transform.RectangleChecker(target,3));
            //print(transform.CircleChecker(target,5,360,2));
            //AreaCheckExtension.DistanceChecker(transform.position, target.position);
        }
        
    }
    public void Hurt(float damage)
    {
        hp -= damage;
        GameManager.ctrl.HpBarCtrl(hp, hpMax);
        if(hp <= 0)
        {

        }
    }
}

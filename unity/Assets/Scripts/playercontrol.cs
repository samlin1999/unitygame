using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 角色控制(自動放入CharacterController,Animator等零件)
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class playercontrol : ISystem {//繼承ISystem的函式
    public static playercontrol ctrl;
    public float moveSpeed = 5f;
    public List<MonsterSystem> monsters = new List<MonsterSystem>();
    public AudioClip SFX;
	public CharacterController charCtrl
    {
		get{return GetComponent<CharacterController> (); }
	}
	public Animator animator
	{
		get { return GetComponentInChildren<Animator>(); }
	}
	Vector3 joystick
	{
		get { return new Vector3(Input.GetAxis(H), 0, Input.GetAxis(V)); }
	}
    public characterInfo charinfo
    {
        get { return characterInfo.ctrl; }
    }
    float attackRange
    {
        get { return charinfo.returnAttackRange(); }
    }
    bool attackType
    {
        get { return charinfo.returnAttackType(); }
    }
    int attackDamage
    {
        get { return charinfo.returnDamage(); }
    }
	string H = "Horizontal";//橫軸
	string V = "Vertical";//縱軸

	void Start () {
		
	}
    private void Awake()
    {
        ctrl = this;
    }
    bool attacking;//判斷是否攻擊
    float timerAttack = 0;//計時
	// Update is called once per frame
	void Update () {
        if(!attacking)
            Move();
        attack();
	}
    /// <summary>
    /// 角色移動
    /// </summary>
    void Move()
    {
        if(joystick != Vector3.zero)
		{
			//transform.rotation = Quaternion.AngleAxis(CameraSystem.ctrl.angY + GetAxisAngle(), Vector3.up);
			transform.rotation = Quaternion.Lerp(transform.rotation, Facing(), Time.deltaTime * 5f);//使動作流暢,lerp:偏差
			charCtrl.SimpleMove(transform.forward * moveSpeed);//向前走
			animator.SetBool("RUN", true);
		}
		else
		{
			animator.SetBool("RUN", false);
		}
    }
    /// <summary>
    /// 角色攻擊動作
    /// </summary>
    void attack()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        if(!attacking && Input.GetMouseButton(0))
        {
            animator.SetTrigger("ATTACK");
            attacking = true;
            AudioManager.ctrl.PlaySFX(SFX);
        }
        if(attacking)
        {
            timerAttack += Time.deltaTime;//計算時間:deltaTime回傳 1/FPS 
            if(timerAttack >= 1)
            {
                timerAttack = 0;
                attacking = false;
            }
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public override void Inital()
    {
        moveSpeed = 5;
        transform.position = StartPoint.ctrl.pos;
        transform.rotation = StartPoint.ctrl.rotation;
        tag = "Player";
    }
    /// <summary>
    /// 角色造成傷害
    /// </summary>
    public void Damage()
    {
        int index = 0;

        while(index <  monsters.Count)
        {
            //monsters[index] => 索引是哪隻怪
            if (transform.CircleChecker(monsters[index].transform, attackRange, 90f))//摧毀怪
            {
                AudioManager.ctrl.PlaySFX(SFX);
                bool isDead = monsters[index].GetHurt(attackDamage);//怪物本身方法，提供是否死亡
                transform.LookAt(monsters[index].transform.position);
                if (isDead) monsters.Remove(monsters[index]);//死亡後將怪物從怪物list中移除
                index += isDead ? 0 : 1;//若死了，原地打下一隻：跳到下一隻
                if (attackType) break;
            }
            else
            {
                index++;
            }
        }
        FXSystem.ctrl.callFX(transform,(int)charinfo.classType);
    }
    /// <summary>
    /// 技能造成傷害
    /// </summary>
    /// <param name="skillObj"></param>
    /// <param name=""></param>
    public bool Damage(Transform skillObj,bool single,float checkRange, float angle = 360)
    {
        int index = 0;
        bool touch = false;//是否接觸
        while (index < monsters.Count)
        {
            //monsters[index] => 索引是哪隻怪
            if (skillObj.CircleChecker(monsters[index].transform, checkRange, angle))//摧毀怪
            {
                FXSystem.ctrl.callFX(monsters[index].transform, 0);
                bool isDead = monsters[index].GetHurt(attackDamage);//怪物本身方法，提供是否死亡
                if (isDead) monsters.Remove(monsters[index]);//死亡後將怪物從怪物list中移除
                index += isDead ? 0 : 1;//若死了，原地打下一隻：跳到下一隻
                touch = true;
                if (single)
                    break;
            }
            else
            {
                index++;
            }
        }
        return touch;
    }
    /// <summary>
    /// 是否有接觸
    /// </summary>
    /// <param name="skillObj"></param>
    /// <param name="single"></param>
    /// <param name="checkRange"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public bool TouchChecker(Transform skillObj, float checkRange, float angle = 360)
    {
        int index = 0;
        bool touch = false;//是否接觸
        while (index < monsters.Count)
        {
            //monsters[index] => 索引是哪隻怪
            if (skillObj.CircleChecker(monsters[index].transform, checkRange, angle))
            {
                touch = true;
                    break;
            }
            else
            {
                index++;
            }
        }
        return touch;
    }
    private GameObject skillobj;
    public void Setskillobj(GameObject obj)
    {
        skillobj = obj;
    }
    /// <summary>
    /// 抓取技能編號
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="skill"></param>
    /// <param name="num"></param>
    public void AniPlay(string trigger, string skill, int num)
    {
        animator.SetTrigger(trigger);
        animator.SetInteger(skill, num);
    }
    /// <summary>
    /// 創造技能物件
    /// </summary>
    public void skill()
    {
        if (skillobj)
            Instantiate(skillobj, transform.position + transform.forward, transform.rotation);
    }
    Quaternion Facing()
	{
		return Quaternion.AngleAxis(CameraSystem.ctrl.angY + GetAxisAngle(), Vector3.up);
		//攝影機的偏差角 + 操作方向以y軸旋轉
	}
    /// <summary>
    /// 搖桿數值
    /// </summary>
    /// <returns></returns>
	float GetAxisAngle()//不可進行運算，會破壞搖桿值
	{
		float ang = Vector3.Angle(Vector3.forward, joystick);
		//ang -= CameraSystem.ctrl.angY;
		return (Input.GetAxis(H) > 0) ? ang : -ang;
	}

}

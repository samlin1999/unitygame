using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MonsterSystem : MonoBehaviour {
    public float hp, hpMax, damage = 20;
    public float moveSpeed = 1;
    public float Exp = 1000;
    public string monstername;
    public DropChance dropMoney;
    DropChance dropItem;
    public Dictionary<int, ItemDropInfo> dropItems = new Dictionary<int, ItemDropInfo>();
    public DropInfoDB dropInfoDB;
    /// <summary>
    /// 角色控制器
    /// </summary>
    public CharacterController charCtrl
    {
        get { return GetComponent<CharacterController>(); }
    }
    /// <summary>
    /// 動作控制器
    /// </summary>
    public Animator animator
    {
        get { return GetComponentInChildren<Animator>(); }
    }
    playercontrol player
    {
        get { return GameManager.ctrl.player; }
    }
    bool attacking;//判斷是否攻擊
    bool isDead;
    float timerAttack = 0;//計時
    public float totalchance;//總掉落率
    public DropChance itemCount;
    void Start () {
        hp = hpMax;
        player.monsters.Add(this);
        SetDropInfo();//設定掉落率
        
    }
    /// <summary>
    /// 設定掉落機率
    /// </summary>
	void SetDropInfo()
    {
        for(int i = 0;i < dropInfoDB.dropList.Count;i++)
        {
            dropItems.Add(i, dropInfoDB.dropList[i]);//設定掉落物查詢字典
            totalchance += dropInfoDB.dropList[i].chance;//掉落率加總
        }
        dropItem.Set(0, totalchance);//設定物品掉落機率
    }
    /// <summary>
    /// 執行掉落物品
    /// </summary>
    /// <param name="count"></param>
    void DropItems(int count = 1)
    {
        for (int j = 0; j < count; j++)
        {
            float chance = dropItem.report(); //print(chance);
            for (int i = 0; i < dropItems.Count; i++)
            {
                if (chance <= dropItems[i].chance)
                {
                    //print(dropItems[i].itemID);//掉落物品ID
                    GameManager.ctrl.bagSystem.StoreUpItem(dropItems[i].itemID);
                    break;
                }
                else
                {
                    chance -= dropItems[i].chance;
                }
            }
        }
    }
	void Update () {
        if (isDead) return;
        if (!attacking)
            Move();
        attack();
    }
    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        if (transform.CircleChecker(player.transform, 10))
        {
            transform.LookAt(player.transform);
            charCtrl.SimpleMove(transform.forward * moveSpeed);//向前走
            animator.SetBool("RUN", true);
        }
        else
        {
            animator.SetBool("RUN", false);
        }
    }
    /// <summary>
    /// 攻擊動作
    /// </summary>
    void attack()
    {
        if (!attacking && transform.CircleChecker(player.transform, 2))
        {
            animator.SetTrigger("ATTACK");
            attacking = true;

        }
        if (attacking)
        {
            timerAttack += Time.deltaTime;//計算時間:deltaTime回傳 1/FPS 
            if (timerAttack >= 1)
            {
                timerAttack = 0;
                attacking = false;
            }
        }
    }
    /// <summary>
    /// 怪物受到傷害
    /// </summary>
    /// <param name="dmg"></param>
    /// <returns></returns>
    public bool GetHurt(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            GameManager.ctrl.stageManager.Check(monstername);
            player.charinfo.setEXp(+Exp);
            isDead = true;
            GameManager.ctrl.characterInfo.GotMoney(dropMoney.report());
            DropItems(itemCount.reportInt());//執行掉落
            animator.SetTrigger("DEAD");
            charCtrl.enabled = false;
            //Destroy(gameObject);
        }
        return isDead;
    }
    /// <summary>
    /// 造成傷害及文字呈現
    /// </summary>
    public void Damage()
    {
        player.charinfo.setHp(-damage);
        player.charinfo.PopUp(monstername + "<color=black> 對你造成</color><size=50>" + damage.ToString() + "</size> 的傷害" );
    }
}

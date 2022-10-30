using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffSort { Damage,Defense}
public enum BuffType { None, LimitTime}
public abstract class IBuff  {
    public BuffType buffType;
    public BuffSort buffSort;
    public Timer timer;
    public float duration, timeLeft;
    public string buffName;
    public virtual void Update()
    {
        if (buffType == BuffType.LimitTime) timeLeft = timer.timeNow;
    }
	
}

public class AttackBuff : IBuff
{
    public float addDamage;

    public AttackBuff(float addDamage, BuffType buffType, float duration,string buffName = "攻擊力")
    {
        buffSort = BuffSort.Damage;
        this.buffName = buffName;
        this.addDamage = addDamage;
        this.buffType = buffType;
        this.duration = duration;
        timer = new Timer(duration);
        timeLeft = timer.timeNow;
    }
}

public class DefenseBuff : IBuff
{
    public float addDefense;

    public DefenseBuff(float addDefense,BuffType buffType,float duration, string buffName = "防禦力")
    {
        buffSort = BuffSort.Defense;
        this.addDefense = addDefense;
        this.buffType = buffType;
        this.duration = duration;
        this.buffName = buffName;
        timer = new Timer(duration);
        timeLeft = timer.timeNow;
    }
}



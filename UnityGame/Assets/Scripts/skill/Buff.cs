using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// buff基本設定
/// </summary>
public class Buff {
    //持續時間
    public float duration;
    public UIBuffIcon buffIcon;
    public string IconCode;
    //時間結束
    public bool timeUp
    {
        get { return duration <= 0; }
    }
    //建構式(buff設定)
    public Buff(float duration)
    {
        this.duration = duration;
    }
	//倒計時功能
    public void Tick(float time)
    {
        duration -= time;
        buffIcon.UpdateBuffIcon(duration);
    }

}

public class MoveSpeedUp : Buff
{
    public const string skillIconCode = "Skill_001";
    public const float buffDuration = 5;//預設持續時間
    public float addition_moveSpeed = 1.3f;//移動加成倍率
    public MoveSpeedUp(float duration) : base(duration)
    {
        IconCode = skillIconCode;
        GameManager.ctrl.buffSystem.AddBuff(this);
    }
    public MoveSpeedUp() : base(buffDuration)
    {
        duration = buffDuration;
        IconCode = skillIconCode;
        GameManager.ctrl.buffSystem.AddBuff(this);
    }
}

public class AttackSpeedUp : Buff
{
    public AttackSpeedUp(float duration) : base(duration)
    {

    }
}


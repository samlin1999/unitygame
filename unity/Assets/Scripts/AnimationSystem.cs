using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : MonoBehaviour {
    /// <summary>
    /// 抓取playercontrol腳本為父腳本(讓本腳本也能使用playercontrol腳本)
    /// </summary>
    public playercontrol player
    {
        get { return GetComponentInParent<playercontrol>(); }
    }
	public void NormalAttack() {
        player.Damage();
	}
    public void Skill()
    {
        player.skill();
    }

}

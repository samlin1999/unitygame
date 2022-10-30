using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterActionSystem : MonoBehaviour {

    public MonsterSystem monster
    {
        get { return GetComponentInParent<MonsterSystem>(); }
    }
    public void NormalAttack()
    {
        monster.Damage();
    }
}

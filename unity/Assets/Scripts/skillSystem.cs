using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillSystem : MonoBehaviour {

    [Header("技能飛行")]
    public bool canFly;
    public float flySpeed = 5f;
    [Header("打擊範圍")]
    public bool single;
    public float checkRange = 3f;
    public float damageRange = 3f;
    [Header("打擊間隔")]
    public int hitTime = 2;
    public float frequtime = 1f;
    public float timer = 1f;
    public bool getdamage = false;
	void Start () {
		
	}
	

	void Update () {
        if(canFly)
            transform.Translate(Vector3.forward * Time.deltaTime * flySpeed);
        Damage();
	}
    void Damage()
    {
        if(!getdamage && hitTime > 0)
        {
            if (single)
                getdamage = playercontrol.ctrl.Damage(transform, single, checkRange);
            else if (playercontrol.ctrl.TouchChecker(transform, checkRange)) 
                getdamage = playercontrol.ctrl.Damage(transform, single, damageRange);
            if (getdamage)
            {
                hitTime--;
                timer = frequtime;
                if (hitTime <= 0)
                    Destroy(gameObject);
            }
        }
        else
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                    getdamage = false;
            }
                
        }
        
    }
}

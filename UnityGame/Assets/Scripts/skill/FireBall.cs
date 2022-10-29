using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : SkilllBase {
    public GameObject explosionObj;
    public override void ActionStart()
    {
        //events.AddListener(delegate { Damage(); });
        base.ActionStart();
    }
    public override void Action()
    {
        base.Action();
        if(attackTimer.isDone)
        {
            SingleAttack(isPenet);
            attackTimer.Start();
        }
    }

    public override void ActionEnd()
    {
        base.ActionEnd();
    }

    public void ExplosionTrigger()
    {
        Instantiate(explosionObj, transform.position, transform.rotation);
    }
}

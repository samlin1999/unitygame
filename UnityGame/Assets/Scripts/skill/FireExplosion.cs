using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosion : SkilllBase {

    public override void ActionStart()
    {
        base.ActionStart();
    }
    public override void Action()
    {
        base.Action();
        if (attackTimer.isDone)
        {
            AreaAttack(isPenet);
            attackTimer.Start();
        }
    }

    public override void ActionEnd()
    {
        Invoke("skillEnd", duration);//延遲銷毀
    }

    void skillEnd()
    {
        base.ActionEnd();
    }
}

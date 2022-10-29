using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpeedUp : SkilllBase {

    public override void ActionStart()
    {
        base.ActionStart();
        new MoveSpeedUp();
    }
}

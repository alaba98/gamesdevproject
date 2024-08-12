using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reloadState : actionBaseState

{
    public override void EnterState(actionManager actions)
    {
        actions.rightHand.weight = 0;
        actions.leftHandIK.weight = 0;  
        actions.animation.SetTrigger("Reload");
    }

    public override void UpdateState(actionManager actions)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimState : AimBaseState
{   // Start is called before the first frame update
    public override void EnterState(AimStateManager aim)
    {
        aim.animator.SetBool("Aim", true);
        aim.currentFov = aim.adsFov;
    }
    public override void UpdateState(AimStateManager aim)
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            aim.SwitchState(aim.Hipfire);
        }
    }
}

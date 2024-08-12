using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipFireState : AimBaseState
{
    public override void EnterState(AimStateManager aim)
    {
        aim.animator.SetBool("Aim", false);
        aim.currentFov = aim.hipFov;
    }
    public override void UpdateState(AimStateManager aim)
    {
        if (Input.GetKey(KeyCode.F))
        {
            aim.SwitchState(aim.Aiming);

        }
    }
}

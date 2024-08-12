using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defaultState : actionBaseState
{
    public override void EnterState(actionManager actions)
    {
        actions.rightHand.weight = 1;
        actions.leftHandIK.weight = 1;
        //after a successful aim animation, we must reset back to our initial state 
        //so we can still aim


    }

    public override void UpdateState(actionManager actions)
    {
        actions.rightHand.weight = Mathf.Lerp(actions.rightHand.weight, 1, 5 * Time.deltaTime);
        actions.leftHandIK.weight = Mathf.Lerp(actions.leftHandIK.weight, 1, 5 * Time.deltaTime);
        // both lines help in smoothing transition from reloading back to aiming
        if (Input.GetKeyDown(KeyCode.R) && canReload(actions))
        {
            actions.switchState(actions.Reload);
        }
    }

    bool canReload(actionManager action)//method defining when an action can be performed so we dont get redundant animation
    {
        if (action.ammo.currentAmmo == action.ammo.clipSize)
        {
            return false;
        }

        else if (action.ammo.clipAmmo == 0)
        {
            return false;
        }

        else return true;
    }
}

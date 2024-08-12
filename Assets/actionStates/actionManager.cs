using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class actionManager : MonoBehaviour
{
    public GameObject thisWeapon;
    public actionBaseState thisState;
    public reloadState Reload = new reloadState();
    public defaultState Default = new defaultState();
    
    [HideInInspector] public weaponAmmo ammo;
    [HideInInspector] public Animator animation;
    AudioSource audioSource;

    public MultiAimConstraint rightHand;
    public TwoBoneIKConstraint leftHandIK;
    //These two variables needed to fix our reload animation due to LHIK
    //Being a child of Rhand

    void Start()
    {
        switchState(Default);
        ammo = thisWeapon.GetComponent<weaponAmmo>();
        audioSource = thisWeapon.GetComponent<AudioSource>();
        animation = GetComponent<Animator>();
    }

    
    void Update()
    {
        thisState.UpdateState(this);    
    }

    public void switchState(actionBaseState state)
    {
        thisState = state;
        thisState.EnterState(this);
    }
    public void weaponReloaded()
    {
        ammo.Reload();
        switchState(Default);//switch out of reload animation after succesful reload
    }

    public void reloadSfx()
    {
        audioSource.PlayOneShot(ammo.reloadSfx);
    }
}

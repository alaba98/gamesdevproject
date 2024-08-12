using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponAmmo : MonoBehaviour
{
    public int clipAmmo;
    public int clipSize;
    public int currentAmmo;
    public AudioClip reloadSfx;
    public Text ammoDisplay;
    void Start()
    {
        currentAmmo = clipSize;
    }

    // Update is called once per frame
    void Update()
    {
        ammoDisplay.text = currentAmmo.ToString() + "/" + clipAmmo.ToString();
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    public void Reload()
    {
        if (clipAmmo >= clipSize)
        { 
            int ammoToReload = clipSize - currentAmmo;
            clipAmmo -= ammoToReload;
            currentAmmo += ammoToReload;
            //clip size used to determine how much 
            //to reload and how much to subtract from reserve clips
        }
        else if(clipAmmo>0) 
        {
            if (clipAmmo + currentAmmo > clipSize)
            {
                int lastClipAmmo = clipAmmo + currentAmmo - clipSize;
                clipAmmo = lastClipAmmo;
                currentAmmo = clipSize;
                // on our last magazine, we set the current ammo to the clip size
                // and set the bullets in our last magazine to the leftover ammo
            }

            else
            {
                currentAmmo += clipAmmo;
                clipAmmo = 0;
            }
        }
        
    }
}

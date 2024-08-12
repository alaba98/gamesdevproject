using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class weaponManager : MonoBehaviour
{
    
   
    [SerializeField] float fireRate;
    float fireRateTimer; //keeps track of the time elapsed since the last shot was fired.
    [SerializeField] bool semiAutomatic;


    [SerializeField] AudioClip gunShot;
    [SerializeField] AudioClip noAmmo;
    AudioSource audioSource;
    [Header("Bullet variables")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPosition;
    [SerializeField] float bulletSpeed;
    public float damage = 20;
    [SerializeField] int bps;//tells us how many bullets should be shot at one time
    AimStateManager aim;
    bool noAmmoSoundPlayed = false; //boolean flag to track if the no ammo sound has already been played
    weaponAmmo ammo;
    actionManager actions;
    [SerializeField] float lightReturnSpeed = 30;
    Light muzzleFlashLight;
    float lightIntensity;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        

        fireRateTimer = fireRate; // Ready to fire as soon as game starts

        aim = GetComponentInParent<AimStateManager>();
    

        ammo = GetComponent<weaponAmmo>();
       
        muzzleFlashLight = GetComponentInChildren<Light>();
        lightIntensity  = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
        actions = GetComponentInParent<actionManager>();
       
    }


    // Update is called once per frame
    void Update()
    {
        if (shouldFire())
        {
            Fire();
            Debug.Log(ammo.currentAmmo);
        }
        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity,0, lightReturnSpeed * Time.deltaTime);
    }

    bool shouldFire()
    {
        fireRateTimer += Time.deltaTime;

        if (fireRateTimer < fireRate)
        {
            return false; // Prevents firing if the timer hasn't reached the fire rate
        }

        if (!aim.IsAiming())
        {
            return false; // Prevents firing if the player is not in the aiming state
        }
        if(actions.thisState == actions.Reload) //prevents firing when you are reloading
        {
            return false;
        }
       
        if (semiAutomatic && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (ammo.currentAmmo == 0)
            {
                if (!noAmmoSoundPlayed)//evaluates to true
                {
                    audioSource.PlayOneShot(noAmmo); // Play out-of-ammo sound effect
                    noAmmoSoundPlayed = true; //NOT operator turns our flag to false, to this if statement no longer runs
                }
                return false; // Don't proceed with firing if out of ammo
            }
            return true;// Fires if the weapon is semi - automatic and the player clicks the mouse button

        }

        if (!semiAutomatic && Input.GetKey(KeyCode.Mouse0))
        {
            if (ammo.currentAmmo == 0)
            {
                if (!noAmmoSoundPlayed)
                {
                    audioSource.PlayOneShot(noAmmo); // Play out-of-ammo sound effect
                    noAmmoSoundPlayed = true;
                }
                return false; // Don't proceed with firing if out of ammo
            }
            return true;
        }
        //when held down, it continuously fires

        // Reset the noAmmoSoundPlayed flag when the fire button is released
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            noAmmoSoundPlayed = false; // resets flag to false once we stop trying to shoot
        }

        return false;

    }
    void Fire()

    {
        Debug.Log("Firing weapon.");
        fireRateTimer = 0;// Resets the timer so you cannot shot without pause
        barrelPosition.LookAt(aim.aimPosition);

       
        audioSource.PlayOneShot(gunShot);
        muzzleFlashTrigger();
        //lookAt method rotates the weapon barrel towards where we aim
        ammo.currentAmmo--;
        for (int i = 0; i < bps; i++)
        {

            GameObject thisBullet = Instantiate(bullet, barrelPosition.position, barrelPosition.rotation);

            bullet bulletStuff = thisBullet.GetComponent<bullet>();
            bulletStuff.weapon = this;
            //instantiates a new bullet at the barrel w/ same positio nand rotation
            Rigidbody rb = thisBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPosition.forward * bulletSpeed, ForceMode.Impulse);
        }

    }
    void muzzleFlashTrigger()
    {
        muzzleFlashLight.intensity = lightIntensity;
    }

}

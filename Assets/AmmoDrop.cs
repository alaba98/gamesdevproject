using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    public int ammoAmount = 20;
    [SerializeField] private AudioClip pickupSound; // Sound for picking up ammo

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Initialize the AudioSource
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                weaponAmmo weaponAmmo = player.GetComponentInChildren<weaponAmmo>();
                if (weaponAmmo != null && weaponAmmo.clipAmmo < 120)
                {
                    player.RefillAmmo(ammoAmount);
                    // Play pickup sound
                    audioSource.PlayOneShot(pickupSound);
                    // Destroy(gameObject); // Destroy the ammo drop object after collecting it
                    Invoke("DestroyAfterDelay", pickupSound.length);
                }
            }
        }
    }


    private void DestroyAfterDelay()
    {
        Destroy(gameObject);
    }

}




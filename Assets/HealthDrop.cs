using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public int healthAmount = 40; // Amount of health to restore
    [SerializeField] private AudioClip pickupSound; // Sound for picking up health

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
            if (player != null && player.Health < 100)
            {
                // Increase player's health
                player.IncreaseHealth(healthAmount);
                audioSource.PlayOneShot(pickupSound);
                // Destroy the health drop object after the sound finishes
                Invoke("DestroyAfterDelay", pickupSound.length);
            }
        }
    }

    private void DestroyAfterDelay()
    {
        Destroy(gameObject);
    }
}

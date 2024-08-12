using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    private Animator animator;
    [SerializeField] public int Health = 100;
    [SerializeField] private int maxHealth = 100; // Maximum health value
    [SerializeField] private Image healthBar; // Reference to the health bar UI Image
    public TextMeshProUGUI playerHealth;
    private bool isDead = false;

    // Variables for flashing effect when hit
    public float blinkDuration;
    float blinkTimer;
    public float blinkIntensity;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Color[] originalColors;

    // Disable all of these scripts upon death
    private weaponManager weaponManager;
    private AimStateManager asm;
    private CameraController cameraController;
    [SerializeField] private AudioClip playerHurt;
    [SerializeField] private AudioClip playerDeath;
    private AudioSource audioSource;

    private void Start()
    {
        playerHealth.text = $"Health: {Health}";
        animator = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        weaponManager = GetComponentInChildren<weaponManager>();
        asm = GetComponentInChildren<AimStateManager>();
        audioSource = GetComponent<AudioSource>();
        cameraController = Camera.main.GetComponent<CameraController>();

        // Store the original colors of all materials
        Material[] materials = skinnedMeshRenderer.materials;
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }

        // Initialize health bar
        UpdateHealthBar();
    }

    public void takeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        Health -= damage;
        if (Health <= 0)
        {
            Health = 0; // Ensure health doesn't go below 0
            isDead = true;
            animator.SetTrigger("playerDeath");
            Death();
            audioSource.PlayOneShot(playerDeath);
        }
        else
        {
            audioSource.PlayOneShot(playerHurt);
        }

        // Update UI
        //playerHealth.text = $"Health: {Health}";
        UpdateHealthBar(); // Update the health bar with the new health value

        // Start the blink timer for flashing effect
        blinkTimer = blinkDuration;
    }

    public void RefillAmmo(int amount)
    {
        weaponAmmo weaponAmmo = GetComponentInChildren<weaponAmmo>();
        if (weaponAmmo != null)
        {
            weaponAmmo.clipAmmo += amount;
            if (weaponAmmo.clipAmmo > 120)
            {
                weaponAmmo.clipAmmo = 120; // Cap the clip ammo at 120
            }
        }
    }

    public void IncreaseHealth(int amount)
    {
        if (isDead)
        {
            return;
        }

        Health += amount;
        if (Health > maxHealth)
        {
            Health = maxHealth; // Cap the health at maximum
        }
        //playerHealth.text = $"Health: {Health}";
        UpdateHealthBar(); // Update the health bar with the new health value
    }

    private void Update()
    {
        // Decrease blink timer
        blinkTimer -= Time.deltaTime;

        // Calculate flash intensity based on remaining blink timer
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;

        // Apply flash effect to all materials
        Material[] materials = skinnedMeshRenderer.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            Color flashColor = Color.red * intensity;
            Color blendedColor = Color.Lerp(originalColors[i], flashColor, intensity);
            materials[i].color = blendedColor;
        }

        // Reset to original colors when blink timer is done
        if (blinkTimer <= 0)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = originalColors[i];
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombiehand"))
        {
            Enemy zombie = other.GetComponentInParent<Enemy>();

            if (zombie != null && !zombie.isDead)
            {
                takeDamage(20);
            }
        }
    }

    private void Death()
    {
        GetComponent<PlayerController>().enabled = false;
        weaponManager.enabled = false;
        cameraController.enabled = false;
        asm.enabled = false;
        //playerHealth.gameObject.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float healthPercentage = (float)Health / maxHealth;
            healthBar.fillAmount = healthPercentage;
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float Health = 100f;
    private Animator animator;
    private NavMeshAgent agent;
    public bool isDead = false;

    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;

    [SerializeField] private GameObject ammoDropPrefab;
    [SerializeField] private GameObject healthDropPrefab;
    [SerializeField] private float ammoDropChance = 0.4f; // 30% chance
    [SerializeField] private float healthDropChance = 0.15f; // 30% chance

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    public void takeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Damage");
            audioSource.Stop();
            audioSource.PlayOneShot(hurtSound);
        }
    }

    private void DropAmmo()
    {
        if (ammoDropPrefab != null && Random.value <= ammoDropChance)
        {
           GameObject ammoDrop =  Instantiate(ammoDropPrefab, transform.position, Quaternion.identity);
           Destroy(ammoDrop, 65f);
        }
    }

    private void DropHealth()
    {
        if (healthDropPrefab != null && Random.value <= healthDropChance)
        {
            GameObject healthDrop = Instantiate(healthDropPrefab, transform.position, Quaternion.identity);
            Destroy(healthDrop, 65f);
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        //agent.enabled = false;
        animator.SetTrigger("Death");
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        DropAmmo();
        DropHealth();
        Destroy(gameObject, 3f);
   
    }
}

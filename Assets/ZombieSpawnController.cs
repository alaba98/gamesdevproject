using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZombieSpawnController : MonoBehaviour
{
    public int zpw = 2; // Zombies in a wave
    public float spawnDelay = 0.5f;
    public float waveTime = 15.0f; // Time between waves
    public bool inCooldown;
    public float cooldownTimer = 0;
    public int currentWave = 0;

    public List<Enemy> zombiesAlive;
    public GameObject zPrefab;
    public Transform[] spawnPoints; // Array of spawn points

    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI cooldownCounterUI;
    public TextMeshProUGUI currentWaveUI;

    private void Start()
    {
        nextWave();
    }

    private void Update()
    {
        List<Enemy> zombiesToBeDestroyed = new List<Enemy>();
        foreach (Enemy enemy in zombiesAlive)
        {
            if (enemy.isDead)
            {
                zombiesToBeDestroyed.Add(enemy);
            }
        }
        foreach (Enemy enemy in zombiesToBeDestroyed)
        {
            zombiesAlive.Remove(enemy);
        }
        zombiesToBeDestroyed.Clear();

        // If all zombies are dead and not in cooldown
        if (zombiesAlive.Count == 0 && !inCooldown)
        {
            StartCoroutine(WaveCooldown());
        }

        // Update cooldown timer
        if (inCooldown)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            cooldownTimer = waveTime; // Reset cooldown timer
        }
        cooldownCounterUI.text = cooldownTimer.ToString("F0");
    }

    private void nextWave()
    {
        zombiesAlive.Clear();

        currentWave++;
        currentWaveUI.text = "Wave: " + currentWave.ToString();
        StartCoroutine(spawnZombies());
        zpw += 1;
    }

    private IEnumerator spawnZombies()
    {
        // Iterate through each spawn point
        foreach (Transform spawnPoint in spawnPoints)
        {
            for (int i = 0; i < zpw; i++)
            {
                // Generate random offset within a range
                Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                Vector3 spawnPosition = spawnPoint.position + spawnOffset;

                // Instantiate enemy prefab at the calculated position
                GameObject zombie = Instantiate(zPrefab, spawnPosition, Quaternion.identity);
                Enemy enemy = zombie.GetComponent<Enemy>();

                // Add spawned enemy to the list of zombies alive
                zombiesAlive.Add(enemy);

                // Wait for spawn delay before spawning the next zombie
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }

    private IEnumerator WaveCooldown()
    {
        //display the UI of the wave counter
        waveOverUI.gameObject.SetActive(true);
        inCooldown = true;
        yield return new WaitForSeconds(waveTime);
        inCooldown = false;
        waveOverUI.gameObject.SetActive(false);//close it when it is over
        nextWave();
    }
}
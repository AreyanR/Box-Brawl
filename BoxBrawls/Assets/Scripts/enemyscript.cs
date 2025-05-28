using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class enemyscript : MonoBehaviour
{
    // this script handles enemy behavior, including movement, health management, and powerup interactions.
    // Components and variables
    private Rigidbody rb;
    public float health = 200f;
    private int damage = 20;
    private int count = 1;
    public float speedup;

    public playercontroller playercount;
    public NavMeshAgent enemy;
    public Transform player;
    public GameObject ridgebox;

    private Rigidbody Rigidbody;
    public float enemySpeed = 3.0f;

    public TextMeshProUGUI enemyhealth;
    public TextMeshProUGUI enemypower;
    [SerializeField] private Image healthbar;

    private List<GameObject> powerups = new List<GameObject>();
    public float maxhealth;
    public bool isFrozen = false;
    private float freezeTimer = 0f;
    public GameObject iceCubePrefab; // Assign this in the Inspector on the enemy
    private GameObject iceCubeInstance;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Rigidbody = ridgebox.GetComponent<Rigidbody>();

        // Set health from GameSettings
        health = GameSettings.enemyHealth;
        maxhealth = health;

        enemy.speed = enemySpeed;
        setenemyhealth();
        updatehealthbar();
        setenemypower();
    }


    void Update()
    {
    if (isFrozen)
    {
        freezeTimer -= Time.deltaTime;
        enemy.isStopped = true;
        enemy.ResetPath();

        if (freezeTimer <= 0f)
        {
            isFrozen = false;
            enemy.isStopped = false;

            // Destroy the ice cube visual when unfreezing
            if (iceCubeInstance != null)
            {
                Destroy(iceCubeInstance);
                iceCubeInstance = null;
            }
        }
        return; // Skip AI/movement logic while frozen
        }
        // Enemy behavior based on player stats
        if ((count <= playercount.count && health <= playercount.health))
        {
            // Chase the player if weaker
            enemy.SetDestination(player.position);
        }
        else
        {
            // Look for powerups if stronger
            initializePowerups();
            Transform closestPowerup = FindClosestPowerup();
            if (closestPowerup != null)
            {
                enemy.SetDestination(closestPowerup.position);
            }
            else
            {
                enemy.SetDestination(player.position);
            }
        }
        //  enemy death
        if (health <= 0)
        {
            health = 0;
            gameObject.SetActive(false);
            GameManager.Instance.EnemyDied();
        }
        // Update UI every frame
        setenemyhealth();
        updatehealthbar();
        setenemypower();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle different powerup pickups and collisions
        if (other.gameObject.CompareTag("Strength PU"))
        {
            other.gameObject.SetActive(false);
            transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            health += 15;
            damage += 10;
            count += 1;
            Rigidbody.mass += 1;
            Rigidbody.linearDamping += 1;
            Rigidbody.angularDamping += 1;
            maxhealth += 15;
            setenemyhealth();
            updatehealthbar();
            setenemypower();
        }
        else if (other.gameObject.CompareTag("Health PU"))
        {
            other.gameObject.SetActive(false);
            health += 50;
            maxhealth += 50;
            setenemyhealth();
            updatehealthbar();
        }
        else if (other.gameObject.CompareTag("Speed PU"))
        {

            other.gameObject.SetActive(false);
            enemySpeed += speedup;
            enemy.speed = enemySpeed;
        }
        else if (other.CompareTag("box"))
        {
            playercount.health -= damage;
            Vector3 knockbackDirection = (transform.position - other.transform.position).normalized;
            float knockbackForce = 3f;
            float scaleFactor = transform.localScale.magnitude; // Approximate size multiplier
            float adjustedForce = knockbackForce * scaleFactor;

            Rigidbody.AddForce(knockbackDirection * adjustedForce, ForceMode.Impulse);

        }
        else if (other.CompareTag("Slowmo PU"))
        {
            other.gameObject.SetActive(false);
            SlowmoManager.Instance.TriggerSlowmo();
        }
        else if (other.CompareTag("Freeze PU"))
    {
        other.gameObject.SetActive(false);
        // Freeze the player instead of self
        if (playercount != null)
        {
            playercount.Freeze(3f);
        }
        // You can add a sound effect here if you want
    }

    }

    Transform FindClosestPowerup()
    // Find the closest active powerup
    {
        Transform closestPowerup = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject powerupObject in powerups)
        {
            if (powerupObject != null && powerupObject.activeSelf)
            {
                Transform powerupTransform = powerupObject.transform;
                float distance = Vector3.Distance(transform.position, powerupTransform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPowerup = powerupTransform;
                }
            }
        }

        return closestPowerup;
    }

    void initializePowerups()
    {
        // Refresh list of all available powerups
        powerups.Clear();

        GameObject[] healthPUs = GameObject.FindGameObjectsWithTag("Health PU");
        GameObject[] speedPUs = GameObject.FindGameObjectsWithTag("Speed PU");
        GameObject[] strengthPUs = GameObject.FindGameObjectsWithTag("Strength PU");
        GameObject[] slowmoPUs = GameObject.FindGameObjectsWithTag("Slowmo PU");
        GameObject[] freezePUs = GameObject.FindGameObjectsWithTag("Freeze PU");


        powerups.AddRange(healthPUs);
        powerups.AddRange(speedPUs);
        powerups.AddRange(strengthPUs);
        powerups.AddRange(slowmoPUs);
        powerups.AddRange(freezePUs);
    }

    void setenemyhealth()
    {
        enemyhealth.text = "Health: " + health.ToString("F0");
    }

    void setenemypower()
    {
        enemypower.text = "Power: " + count.ToString();
    }

    private void updatehealthbar()
    {
        healthbar.fillAmount = health / maxhealth;
    }

public void Freeze(float duration)
{
    isFrozen = true;
    freezeTimer = duration;
    enemy.isStopped = true;
    enemy.ResetPath();

    // Spawn ice cube ONCE
    if (iceCubePrefab != null && iceCubeInstance == null)
    {
        iceCubeInstance = Instantiate(iceCubePrefab, transform.position, Quaternion.identity, transform);
    }
}



}

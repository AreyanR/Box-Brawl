using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class enemyscript : MonoBehaviour
{
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
    private float maxHealth; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Rigidbody = ridgebox.GetComponent<Rigidbody>();

        maxHealth = health; 

        enemy.speed = enemySpeed;
        setenemyhealth();
        updatehealthbar();
        setenemypower();
    }

    void Update()
    {
        if ((count <= playercount.count && health <= playercount.health))
        {
            enemy.SetDestination(player.position);
        }
        else
        {
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

        if (health <= 0)
        {
            health = 0;
            gameObject.SetActive(false);
            GameManager.Instance.EnemyDied();
        }

        setenemyhealth();
        updatehealthbar();
        setenemypower();
    }

    private void OnTriggerEnter(Collider other)
    {
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
            maxHealth += 15;
            setenemyhealth();
            updatehealthbar();
            setenemypower();
        }
        else if(other.gameObject.CompareTag("Health PU"))
        {
            other.gameObject.SetActive(false);
            health += 50;
            maxHealth += 50;
            setenemyhealth();
            updatehealthbar();
        }
        else if(other.gameObject.CompareTag("Speed PU"))
        {
            
            other.gameObject.SetActive(false); 
            enemySpeed += speedup; 
            enemy.speed = enemySpeed; 
        }
        else if (other.CompareTag("box"))
        {
            playercount.health -= damage;
        }
    }

    Transform FindClosestPowerup()
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
        powerups.Clear();

        GameObject[] healthPUs = GameObject.FindGameObjectsWithTag("Health PU");
        GameObject[] speedPUs = GameObject.FindGameObjectsWithTag("Speed PU");
        GameObject[] strengthPUs = GameObject.FindGameObjectsWithTag("Strength PU");

        powerups.AddRange(healthPUs);
        powerups.AddRange(speedPUs);
        powerups.AddRange(strengthPUs);
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
        healthbar.fillAmount = health / maxHealth; 
    }
}

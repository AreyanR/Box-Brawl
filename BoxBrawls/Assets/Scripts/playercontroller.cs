using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playercontroller : MonoBehaviour
{
    // This script handles the player's movement, health, power-ups, and dash mechanics.

    //  Player refs and stats 
    private Rigidbody rb;
    public int count;
    public float speed;
    public float speedup;
    public float health;
    private int damage;

    public TextMeshProUGUI PlayerHealth;
    public TextMeshProUGUI playerpower;
    public enemyscript enemy;
    public float rotationSpeed = 1.0f;

    [SerializeField] private Image healthbar;
    [SerializeField] private Image stamwheel;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip dodgeSound;
    [SerializeField] private AudioClip hitSound;

    public float maxhealth = 200f;

    // Dodge / Dash variables
    private bool isDashing = false;
    private float dashDuration = 0.2f;
    private float dashForce = 2.0f;
    private float originalSpeed;
    private float currentStamina;
    public float maxStamina = 10f;
    public float staminaRegenerationRate = 2f;
    public float dashStaminaCost = 10f;
    public bool isFrozen = false;
    private float freezeTimer = 0f;
    public GameObject iceCubePrefab; // Assign in Inspector on the player
    private GameObject iceCubeInstance;


    void Start()
    {
        rb = GetComponent<Rigidbody>(); //get Rigidbody
        count = 0;//initialize powerup
        health = maxhealth;//max out player health
        damage = 10;//set damage
        originalSpeed = speed;//set original movement speed
        currentStamina = maxStamina;//max out stamina bar

        sethealthtext();
        setplayerpower();
        updatehealthbar();
        updatestamwheel();
    }

    void FixedUpdate()
{
    if (isFrozen || isBeingLaunched)
    {
        rb.linearVelocity = Vector3.zero;
        return;
    }

    // ...rest of your movement code...
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

    Vector3 horizontalForce = moveDirection * speed;
    horizontalForce.y = rb.linearVelocity.y;
    rb.linearVelocity = horizontalForce;
    

    if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
}


    void Update()
{
    // Handle freezing
    if (isFrozen)
    {
        freezeTimer -= Time.deltaTime;

        // Optionally, you can prevent all input/movement here (just by returning)
        if (freezeTimer <= 0f)
        {
            isFrozen = false;

            // Destroy ice cube
            if (iceCubeInstance != null)
            {
                Destroy(iceCubeInstance);
                iceCubeInstance = null;
            }
        }
        return; // Skip rest of Update while frozen
    }

    // Handle player death
    if (health <= 0)
    {
        health = 0;
        gameObject.SetActive(false);
        GameManager.Instance.PlayerDied();
    }

    // Handle Dodge input
    if (!isDashing)
    {
        if (Input.GetButtonDown("Jump") && CanDash())
        {
            StartCoroutine(Dash());
        }
    }

    // Regenerate stamina over time
    if (currentStamina < maxStamina)
    {
        currentStamina += staminaRegenerationRate * Time.deltaTime * 3; // Faster regen
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        updatestamwheel();
    }

    sethealthtext();
    setplayerpower();
    updatehealthbar();
}


    private void OnTriggerEnter(Collider other)
    {  // Handle powerup pickups and enemy collisions
        if (other.gameObject.CompareTag("Speed PU"))
        {
            other.gameObject.SetActive(false);
            speed += speedup;
            setplayerpower();
            updatehealthbar();
            audioSource.PlayOneShot(pickupSound);
        }
        else if (other.gameObject.CompareTag("Strength PU"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            health += 15;
            maxhealth += 15;
            damage += 5;
            rb.mass += 1;
            rb.linearDamping += 1;
            rb.angularDamping += 1;
            setplayerpower();
            updatehealthbar();
            audioSource.PlayOneShot(pickupSound);
        }
        else if (other.gameObject.CompareTag("Health PU"))
        {
            other.gameObject.SetActive(false);
            health += 50;
            maxhealth += 50;
            setplayerpower();
            updatehealthbar();
            audioSource.PlayOneShot(pickupSound);
        }
        else if (other.CompareTag("enemy"))
        {
            audioSource.PlayOneShot(hitSound);
            enemy.health -= damage;
        }
        else if (other.CompareTag("Slowmo PU"))
        {
            other.gameObject.SetActive(false);
            SlowmoManager.Instance.TriggerSlowmo();
            audioSource.PlayOneShot(pickupSound);
        }
        else if (other.CompareTag("Freeze PU"))
        {
            other.gameObject.SetActive(false);
            enemy.Freeze(3f); // Call the freeze method on enemy
            audioSource.PlayOneShot(pickupSound);
        }
else if (other.gameObject.CompareTag("Homing PU"))
    {
        other.gameObject.SetActive(false);
        if (launchCoroutine != null)
            StopCoroutine(launchCoroutine);

        launchCoroutine = StartCoroutine(LaunchToEnemyRoutine(0.8f)); // Only launch, no return
        audioSource.PlayOneShot(pickupSound);
    }







    }

    private IEnumerator Dash()
    {
        if (CanDash())
        {
            isDashing = true;
            currentStamina -= dashStaminaCost;
            audioSource.PlayOneShot(dodgeSound);

            speed *= 2f; // Double the speed during dash

            Vector3 dashDirection = rb.linearVelocity.normalized;
            rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

            yield return new WaitForSeconds(dashDuration);

            // Smoothly return speed to original
            while (speed > originalSpeed)
            {
                speed -= 50f * Time.deltaTime;
                yield return null;
            }

            speed = originalSpeed;
            isDashing = false;
        }
    }

    private bool CanDash()
    {
        return currentStamina >= dashStaminaCost;
    }

    void sethealthtext()
    {
        PlayerHealth.text = "Health: " + health.ToString();
    }

    void setplayerpower()
    {
        playerpower.text = "Power: " + count.ToString();
    }

    private void updatehealthbar()
    {
        float normalizedHealth = health / maxhealth;
        healthbar.fillAmount = normalizedHealth;
    }

    private void updatestamwheel()
    {
        float normalizedStamina = currentStamina / maxStamina;
        stamwheel.fillAmount = normalizedStamina;
    }

public void Freeze(float duration)
{
    isFrozen = true;
    freezeTimer = duration;

    // Spawn ice cube if not already there
    if (iceCubePrefab != null && iceCubeInstance == null)
    {
        iceCubeInstance = Instantiate(iceCubePrefab, transform.position, Quaternion.identity, transform);
    }
}

private bool isBeingLaunched = false;
private Coroutine launchCoroutine;

private IEnumerator LaunchToEnemyRoutine(float launchDuration = 0.8f)
{
    if (enemy == null) yield break;

    isBeingLaunched = true;
    rb.linearVelocity = Vector3.zero;

    Vector3 start = transform.position;
    Vector3 end = enemy.transform.position;
    end.y = start.y; // Keep on the ground

    float timer = 0f;
    while (timer < launchDuration)
    {
        rb.MovePosition(Vector3.Lerp(start, end, timer / launchDuration));
        timer += Time.fixedDeltaTime;
        yield return new WaitForFixedUpdate();
    }
    rb.MovePosition(end); // Snap to the enemy's position

    // (Optional: Wait a moment for "impact" effect)
    yield return new WaitForSeconds(0.08f);

    isBeingLaunched = false;
}



}

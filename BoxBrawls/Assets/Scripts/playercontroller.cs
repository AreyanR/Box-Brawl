using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playercontroller : MonoBehaviour
{
    private Rigidbody rb;
    public int count;
    public float speed;
    public float health;
    private int damage;

    public TextMeshProUGUI PlayerHealth;
    public TextMeshProUGUI playerpower;
    public enemyscript enemy;
    public float rotationSpeed = 1.0f;

    [SerializeField] private Image healthbar;
    [SerializeField] private Image stamwheel;

    private float maxhealth = 200f;

    // Dodge / Dash variables
    private bool isDashing = false;
    private float dashDuration = 0.2f;
    private float dashForce = 2.0f;
    private float originalSpeed;
    private float currentStamina;
    public float maxStamina = 10f;
    public float staminaRegenerationRate = 2f;
    public float dashStaminaCost = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        health = maxhealth;
        damage = 10;
        originalSpeed = speed;
        currentStamina = maxStamina;

        sethealthtext();
        setplayerpower();
        updatehealthbar();
        updatestamwheel();
    }

    void FixedUpdate()
    {
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
            currentStamina += staminaRegenerationRate * Time.deltaTime * 3; // Faster regen like your original
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            updatestamwheel();
        }

        sethealthtext();
        setplayerpower();
        updatehealthbar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pickup"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            health += 15;
            damage += 5;
            rb.mass += 1;
            rb.linearDamping += 1;
            rb.angularDamping += 1;
            setplayerpower();
            updatehealthbar();
        }
        else if (other.CompareTag("enemy"))
        {
            enemy.health -= damage;
        }
    }

    private IEnumerator Dash()
    {
        if (CanDash())
        {
            isDashing = true;
            currentStamina -= dashStaminaCost;
            updatestamwheel();

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
}

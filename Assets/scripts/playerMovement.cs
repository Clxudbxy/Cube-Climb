using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int enemyDamage = 20;
    [SerializeField] private float damageCooldown = 1f;

    [Header("UI")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text healthText;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool stick = false;

    public int coinCount = 0;
    public int currentHealth;
    private float lastDamageTime = -999f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        UpdateCoinUI();
        UpdateHealthUI();
    }

    void Update()
    {
        float moveInput = 0f;
        
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                moveInput = -1f;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                moveInput = 1f;
            
            if (isGrounded)
            {
                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                }
            }
            
            if (stick)
            {
                // stick to wall for 3 seconds and then fall
            }
        }
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("wall"))
        {
            stick = true;
        }

        if (collision.gameObject.CompareTag("enemy"))
        {
            TakeDamage(enemyDamage);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.CompareTag("wall"))
        {
            stick = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("coin"))
        {
            coinCount += 1;
            UpdateCoinUI();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("enemy"))
        {
            TakeDamage(enemyDamage);
        }
    }

    public void TakeDamage(int amount)
    {
        // Prevent rapid multi-hit drain in the same fraction of a second
        if (Time.time - lastDamageTime < damageCooldown) return;

        lastDamageTime = Time.time;
        currentHealth = Mathf.Max(0, currentHealth - amount);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log("Player defeated!");
        }
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coinCount;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }
}
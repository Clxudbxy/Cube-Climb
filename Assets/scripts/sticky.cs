using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class sticky : MonoBehaviour
{
    [Header("Wall Stick Settings")]
    [Tooltip("Maximum time in seconds the player can stick to a wall.")]
    [SerializeField] private float maxStickTime = 3f;

    [Tooltip("Tag assigned to climbable/stickable walls.")]
    [SerializeField] private string wallTag = "Wall";

    [Tooltip("Cooldown before the player can stick again after letting go.")]
    [SerializeField] private float regrabCooldown = 0.25f;

    private Rigidbody2D rb;
    private float defaultGravity;
    private float stickTimer;
    private float cooldownTimer;
    private bool isSticking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (isSticking)
        {
            stickTimer -= Time.deltaTime;

            // Keep velocity zeroed while stuck so the player doesn't slide
            rb.linearVelocity = Vector2.zero;

            // Check for Spacebar or Gamepad jump press using the new Input System
            bool jumpPressed = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) ||
                               (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);

            // Release if 3 seconds expire or if the player jumps off
            if (stickTimer <= 0f || jumpPressed)
            {
                ReleaseWall();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isSticking && cooldownTimer <= 0f && collision.gameObject.CompareTag(wallTag))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    StickToWall();
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isSticking && collision.gameObject.CompareTag(wallTag))
        {
            ReleaseWall();
        }
    }

    private void StickToWall()
    {
        isSticking = true;
        stickTimer = maxStickTime;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
    }

    private void ReleaseWall()
    {
        isSticking = false;
        rb.gravityScale = defaultGravity;
        cooldownTimer = regrabCooldown;
    }
}
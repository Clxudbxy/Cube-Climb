using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;


    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool stick = false;

    public int coinCount = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
                //stick to wall for 3 seconds and then fall
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

        if(collision.gameObject.tag == "coin")
        {
            coinCount += 1;
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


}
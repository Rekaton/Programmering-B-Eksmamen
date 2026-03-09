using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Bevægelse")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Jord Tjek")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isDashing;
    private bool canDash = true;
    private float lastFacingDirection = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Gør intet, hvis vi dasher
        if (groundCheck == null || isDashing) return;

        // Tjekker om vi rammer jorden (og ikke os selv)
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = (hit != null && hit.gameObject != gameObject);

        // Giver dash tilbage, når vi lander
        if (isGrounded)
        {
            canDash = true;
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        // Husker hvilken vej vi kigger
        if (moveInput.x > 0) lastFacingDirection = 1f;
        else if (moveInput.x < 0) lastFacingDirection = -1f;
    }

    void OnJump(InputValue value)
    {
        // Hop kun hvis vi er på jorden og ikke dasher
        if (value.isPressed && isGrounded && !isDashing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false; // Forhindrer dobbelt-hop
        }
    }

    void OnDash(InputValue value)
    {
        // Dash kun hvis vi har lov og trykker på knappen
        if (value.isPressed && canDash && !isDashing)
        {
            Vector2 dashDir = moveInput;

            // Brug gemt retning hvis vi står stille
            if (dashDir == Vector2.zero)
            {
                dashDir = new Vector2(lastFacingDirection, 0);
            }
            else
            {
                dashDir = dashDir.normalized; // Samme fart i alle retninger
            }

            StartCoroutine(PerformDash(dashDir));
        }
    }

    void FixedUpdate()
    {
        // Normal bevægelse (hvis vi ikke dasher)
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        isDashing = true;
        canDash = false; // Brug vores dash

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; // Slå tyngdekraft fra

        rb.linearVelocity = direction * dashSpeed; // Flyv afsted

        yield return new WaitForSeconds(dashDuration); // Vent

        rb.linearVelocity = Vector2.zero; // Stop brat
        rb.gravityScale = originalGravity; // Tyngdekraft på igen
        isDashing = false;
    }

}
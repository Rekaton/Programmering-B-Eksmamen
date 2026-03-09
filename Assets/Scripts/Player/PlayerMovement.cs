using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Indstillinger for Hop")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.15f; // Gjort lidt kortere for et mere "snappy" Celeste-feel

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private bool isDashing;
    private bool canDash = true;
    private float lastFacingDirection = 1f;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Tjekker om vi rører jorden
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            // Nulstil dash hvis vi rører jorden og ikke dasher
            if (isGrounded && !isDashing)
            {
                canDash = true;
            }
        }
    }

    void OnJump(InputValue value)
    {
        // Hopper kun hvis vi trykker og rører jorden
        if (value.isPressed && isGrounded)
        {
            // Vi sætter y-velocity præcist til jumpForce, og bevarer x-farten
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput.x > 0)
        {
            lastFacingDirection = 1f;
        }
        else if (moveInput.x < 0)
        {
            lastFacingDirection = -1f;
        }
    }

    void OnDash(InputValue value)
    {
        if (value.isPressed && canDash && !isDashing)
        {
            Vector2 dashDir = moveInput;

            if (dashDir == Vector2.zero)
            {
                dashDir = new Vector2(lastFacingDirection, 0);
            }
            else
            {
                // Her gennemtvinger vi normalisering, så W+D (skråt) ikke er hurtigere
                dashDir = dashDir.normalized;
            }

            StartCoroutine(PerformDash(dashDir));
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        isDashing = true;
        canDash = false;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Tvinger dash-farten
        rb.linearVelocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        // VIGTIGT: Vi stopper spilleren helt (dræber momentum), så man ikke flyver videre op i luften!
        rb.linearVelocity = Vector2.zero;

        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    // NYT: Denne funktion tegner en rød cirkel i din Scene-view ved spillerens fødder
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
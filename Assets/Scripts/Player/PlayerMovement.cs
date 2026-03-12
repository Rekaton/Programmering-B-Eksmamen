using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Bevægelse")]
    public float moveSpeed = 5f;  // Fart på løb
    public float jumpForce = 10f; // Højde på hop

    [Header("Dash")]
    public float dashSpeed = 20f;      // Fart på dash
    public float dashDuration = 0.15f; // Længde på dash

    private Rigidbody2D rb;
    private Vector2 moveInput;

    // Spillerens tilstand lige nu
    public bool isDashing; // public for haptics
    private bool canDash;
    private bool canJump;
    private float lastFacingDirection = 1f; // 1 er højre og -1 er venstre

    // Wall jump integration
    private PlayerWallJump wallJump;

    private void Start()
    {
        // Hent fysikken. Den skal bruges til at flytte spilleren.
        rb = GetComponent<Rigidbody2D>();

        // Hent wall jump scriptet hvis det findes på samme GameObject.
        wallJump = GetComponent<PlayerWallJump>();
    }

    private void OnMove(InputValue value)
    {
        // Gem spillerens input.
        moveInput = value.Get<Vector2>();

        // Husk hvilken vej vi kigger. Det bruges til dash fra stilstand.
        if (moveInput.x != 0)
        {
            lastFacingDirection = Mathf.Sign(moveInput.x);
        }
    }

    private void OnJump(InputValue value)
    {
        if (!value.isPressed || isDashing)
        {
            return;
        }

        // Giv wall jump første prioritet.
        // Hvis et wall jump blev udført, stop her.
        if (wallJump != null)
        {
            bool didWallJump = wallJump.TryWallJump();
            if (didWallJump)
            {
                return;
            }
        }

        // Tjek om vi må hoppe. Hvis ja så hop.
        if (canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void OnDash(InputValue value)
    {
        // Tjek om vi må dashe.
        if (value.isPressed && canDash && !isDashing)
        {
            // Find retning. Står vi stille så brug den gemte retning.
            Vector2 dashDir;
            if (moveInput == Vector2.zero)
            {
                dashDir = new Vector2(lastFacingDirection, 0);
            }
            else
            {
                dashDir = moveInput.normalized;
            }

            // Start selve dashet.
            StartCoroutine(PerformDash(dashDir));
        }
    }

    private void FixedUpdate()
    {
        // Tjek om wall jump styrer bevægelsen lige nu.
        bool wallJumpActive = false;
        if (wallJump != null)
        {
            wallJumpActive = wallJump.IsWallJumping;
        }

        // Gå normalt hvis vi ikke dasher og wall jump ikke er aktiv.
        if (!isDashing && !wallJumpActive)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        isDashing = true;
        canDash = false; // Dash er nu brugt.

        // Sluk tyngdekraften. Flyv lige frem.
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = direction * dashSpeed;

        // Vent imens vi dasher.
        yield return new WaitForSeconds(dashDuration);

        // Stop dash og tænd tyngdekraften igen.
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Hvis vi rører jorden, så må vi gerne hoppe og dashe igen.
        if (collision.gameObject.CompareTag("Ground") && !isDashing)
        {
            // Wall jump tæller ikke som landing. WallJump scriptet styrer det selv.
            bool isWallJumping = false;
            if (wallJump != null)
            {
                isWallJumping = wallJump.IsWallJumping;
            }

            if (!isWallJumping)
            {
                canJump = true;
                canDash = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Hvis man forlader ground, så sæt jump til false.
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }
    }
}
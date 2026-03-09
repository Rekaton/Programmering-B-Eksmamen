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
    public bool isDashing; //public for haptics
    private bool canDash;
    private bool canJump;
    private float lastFacingDirection = 1f; // 1 er højre og -1 er venstre

    private void Start()
    {
        // Hent fysikken. Den skal bruges til at flytte spilleren.
        rb = GetComponent<Rigidbody2D>();
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
        // Tjek om vi må hoppe. Hvis ja så hop.
        if (value.isPressed && canJump && !isDashing)
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
            Vector2 dashDir = moveInput == Vector2.zero
                ? new Vector2(lastFacingDirection, 0) : moveInput.normalized;

            // Start selve dashet.
            StartCoroutine(PerformDash(dashDir));
        }
    }

    private void FixedUpdate()
    {
        // Gå normalt hvis vi ikke dasher.
        if (!isDashing)
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
            canJump = true;
            canDash = true;
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
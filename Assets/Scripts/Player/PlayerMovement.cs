using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Bevægelse")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    // State variabler
    private bool isDashing;
    private bool canDash = true;
    private bool canJump = true;
    private float lastFacingDirection = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        // Gemmer retningen (1 for højre, -1 for venstre), så vi kan dashe fra stilstand
        if (moveInput.x != 0)
        {
            lastFacingDirection = Mathf.Sign(moveInput.x);
        }
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed && canJump && !isDashing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canJump = false;
        }
    }

    private void OnDash(InputValue value)
    {
        if (value.isPressed && canDash && !isDashing)
        {
            // Sætter retningen kort og præcist. Hvis vi står stille, brug lastFacingDirection.
            Vector2 dashDir = moveInput == Vector2.zero
                ? new Vector2(lastFacingDirection, 0)
                : moveInput.normalized;

            StartCoroutine(PerformDash(dashDir));
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        isDashing = true;
        canDash = false; // Brugt med det samme

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    // --- JORD-TJEK MED TAGS ---

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Giver hop og dash tilbage, hvis vi rører "Ground", ikke dasher, og ikke flyver opad
        if (collision.gameObject.CompareTag("Ground") && !isDashing && rb.linearVelocity.y <= 0.01f)
        {
            canJump = true;
            canDash = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Fjerner hoppet i det sekund, vi forlader "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Bevægelse")]
    public float moveSpeed = 5f;  // Fart på løb
    public float jumpForce = 10f; // Højde på hop

    // Spillere trykker næsten altid hop for sent
    // Det føles uretfærdigt at falde ned
    // Coyote time fikser dette og gør styringen god
    public float coyoteTime = 0.2f;

    [Header("Dash")]
    public float dashSpeed = 20f;      // Fart på dash
    public float dashDuration = 0.15f; // Længde på dash

    private Rigidbody2D rb;
    private Vector2 moveInput;

    // Spillerens tilstand lige nu
    public bool isDashing; // public for haptics
    private bool canDash;

    // Tæller ned når kanten forlades
    private float coyoteTimeCounter;
    private float lastFacingDirection = 1f; // 1 er højre og -1 er venstre

    private void Start()
    {
        // Hent fysik
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Tiden forsvinder lidt efter lidt
        coyoteTimeCounter -= Time.deltaTime;
    }

    private void OnMove(InputValue value)
    {
        // Gem spillerens input
        moveInput = value.Get<Vector2>();

        // Husk hvilken vej der kigges
        if (moveInput.x != 0)
        {
            lastFacingDirection = Mathf.Sign(moveInput.x);
        }
    }

    private void OnJump(InputValue value)
    {
        if (!value.isPressed || isDashing) return;

        // Tillad hoppet hvis tælleren ikke er nul
        // Det er her spilleren reddes af coyote time
        if (coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            coyoteTimeCounter = 0f; // Slet tiden så der ikke dobbelthoppes
        }
    }

    private void OnDash(InputValue value)
    {
        // Tjek om der må dashes
        if (value.isPressed && canDash && !isDashing)
        {
            // Find retning
            // Brug den gemte hvis der stås stille
            Vector2 dashDir = moveInput == Vector2.zero
                ? new Vector2(lastFacingDirection, 0) : moveInput.normalized;

            // Start selve dashet
            StartCoroutine(PerformDash(dashDir));
        }
    }

    private void FixedUpdate()
    {
        // Gå normalt hvis der ikke dashes
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        isDashing = true;
        canDash = false; // Dash er nu brugt

        // Sluk tyngdekraften og flyv lige frem
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = direction * dashSpeed;

        // Vent imens der dashes
        yield return new WaitForSeconds(dashDuration);

        // Stop dash og tænd tyngdekraften igen
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Hvis jorden røres og der ikke dashes
        if (collision.gameObject.CompareTag("Ground") && !isDashing)
        {
            // Fyld tiden op mens jorden røres
            // Så er der fuld tid til at hoppe bagefter
            coyoteTimeCounter = coyoteTime;
            canDash = true;
        }
    }
}
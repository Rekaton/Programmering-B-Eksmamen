using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveJumpDash : MonoBehaviour
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
    public Vector2 moveInput;

    // Spillerens tilstand lige nu
    public bool isDashing; // public for haptics
    private bool canDash;

    // Tæller ned når kanten forlades
    private float coyoteTimeCounter;

    private float lastFacingDirection = 1f; // 1 er højre og -1 er venstre

    // Wall jump integration
    private PlayerWallJump wallJump;

    private void Start()
    {
        // Hent fysik
        rb = GetComponent<Rigidbody2D>();

        // Hent wall jump scriptet hvis det findes på samme GameObject
        wallJump = GetComponent<PlayerWallJump>();
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
        if (!value.isPressed || isDashing)
        {
            return;
        }

        // Giv wall jump første prioritet
        // Hvis et wall jump blev udført, stop her
        if (wallJump != null)
        {
            bool didWallJump = wallJump.TryWallJump();
            if (didWallJump)
            {
                return;
            }
        }

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
            Vector2 dashDir;
            if (moveInput == Vector2.zero)
            {
                dashDir = new Vector2(lastFacingDirection, 0);
            }
            else
            {
                dashDir = moveInput.normalized;
            }

            // Start selve dashet
            StartCoroutine(PerformDash(dashDir));
        }
    }

    private void FixedUpdate()
    {
        // Tjek om wall jump styrer bevægelsen lige nu
        bool wallJumpActive = false;
        if (wallJump != null)
        {
            wallJumpActive = wallJump.IsWallJumping;
        }

        // Gå normalt hvis der ikke dashes og wall jump ikke er aktiv
        if (!isDashing && !wallJumpActive)
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
            // Wall jump tæller ikke som landing
            // WallJump scriptet styrer det selv
            bool isWallJumping = false;
            if (wallJump != null)
            {
                isWallJumping = wallJump.IsWallJumping;
            }

            if (!isWallJumping)
            {
                // Fyld tiden op mens jorden røres
                // Så er der fuld tid til at hoppe bagefter
                coyoteTimeCounter = coyoteTime;
                canDash = true;
            }
        }
    }
}
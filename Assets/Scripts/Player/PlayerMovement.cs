using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // skal bruge den her for at det nye input system virker

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float moveSpeed = 5f; // farten når man bare går


    [Header("Indstillinger for Hop")]
    [SerializeField] private float jumpForce = 10f; // Hvor kraftigt vi hopper opad
    [SerializeField] private Transform groundCheck; // Et tomt objekt vi placerer ved fødderne
    [SerializeField] private float groundCheckRadius = 0.2f; // Hvor stor tjek-cirklen er
    [SerializeField] private LayerMask groundLayer; // Hvilke ting i spillet er "jorden"?

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f; // hvor hurtigt man flyver afsted
    [SerializeField] private float dashDuration = 0.2f;    // hvor lang tid selve rykket tager
    [SerializeField] private float dashCooldown = 1f;      // ventetid før man kan gøre det igen

    // variabler til at holde styr på ting
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing;
    private float nextDashTime; // holder øje med hvornår pausen er slut
    private Vector2 dashDirection;
    private bool isGrounded;   // Vi holder styr på om vi må hoppe

    void Start()
    {
        // henter rigidbody så vi kan skubbe til spilleren
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Vi hopper kun hvis knappen er trykket ned OG vi rører jorden
        // Her tjekker vi hele tiden, om vi rører jorden. Ingen knap-tjek her!
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }
    //kører når man trykker på jump knappen
    void OnJump(InputValue value)
    {
        // Vi hopper kun hvis knappen er trykket ned OG vi rører jorden
        if (value.isPressed && isGrounded)
        {
            // Vi beholder farten til siden, men ændrer farten opad til vores hoppe-kraft
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

    }
        void OnMove(InputValue value)
    {

        // gemmer retningen man prøver at gå i
        moveInput = value.Get<Vector2>();
    }

    // kører når man trykker på dash knappen
    void OnDash(InputValue value)
    {
        // tjekker om knappen er nede

        if (value.isPressed)
        {
            if (Time.time >= nextDashTime && !isDashing)
            {
                // Hvis man dasher uden at holde nogen knapper nede, sætter vi en standardretning
                // Her kan du evt. bygge videre, så den dasher den vej karakteren kigger
                Vector2 dashDirection = moveInput;
                if (dashDirection == Vector2.zero)
                {
                    dashDirection = new Vector2(Mathf.Sign(transform.localScale.x), 0); // Dasher den vej vi vender
                }

                StartCoroutine(PerformDash(dashDirection.normalized));
            }
        }

    }

    void FixedUpdate()
    {
        if (isDashing) return;

        // Her er fiksede vi det med at gå opad! Vi bruger kun moveInput.x nu.
        // På Y-pladsen siger vi rb.linearVelocity.y, som lader tyngdekraften gøre sit arbejde.
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    // det her styrer selve dashet og ventetiden
    private IEnumerator PerformDash(Vector2 direction)
    {
        isDashing = true;
        nextDashTime = Time.time + dashCooldown;

        // Gemmer tyngdekraften og slukker for den, så vi ikke falder mens vi dasher
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Sætter farten for dashet
        rb.linearVelocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        // Tænder for tyngdekraften igen og afslutter dash
        rb.gravityScale = originalGravity;
        isDashing = false;

    }

}

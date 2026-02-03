using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // skal bruge den her for at det nye input system virker

public class PlayerMovement : MonoBehaviour
{
    [Header("Indstillinger for Bevægelse")]
    [SerializeField] private float moveSpeed = 5f; // farten når man bare går

    [Header("Indstillinger for Dash")]
    [SerializeField] private float dashSpeed = 20f; // hvor hurtigt man flyver afsted
    [SerializeField] private float dashDuration = 0.2f;    // hvor lang tid selve rykket tager
    [SerializeField] private float dashCooldown = 1f;      // ventetid før man kan gøre det igen

    // variabler til at holde styr på ting
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing;
    private float nextDashTime; // holder øje med hvornår pausen er slut

    void Start()
    {
        // henter rigidbody så vi kan skubbe til spilleren
        rb = GetComponent<Rigidbody2D>();
    }

    // bruger ikke update da det nye system klarer det selv, men lader den stå hvis jeg skal bruge den
    void Update()
    {

    }

    // den her kører af sig selv når man trykker på knapperne
    void OnMove(InputValue value)
    {
        // hvis man dasher skal man ikke kunne styre imens
        if (isDashing) return;

        // gemmer retningen man prøver at gå i
        moveInput = value.Get<Vector2>();
    }

    // kører når man trykker på dash knappen
    void OnDash(InputValue value)
    {
        // tjekker om knappen er nede
        if (value.isPressed)
        {
            // tjekker om cooldown er færdig og om man er i gang med at dashe
            if (Time.time >= nextDashTime && !isDashing)
            {
                // man skal bevæge sig for at kunne dashe ellers sker der ikke noget
                if (moveInput != Vector2.zero)
                {
                    StartCoroutine(PerformDash());
                }
            }
        }
    }

    void FixedUpdate()
    {
        // fysikken skal ikke gøre noget hvis vi dasher
        if (isDashing) return;

        // her flytter vi faktisk spilleren med farten
        rb.linearVelocity = moveInput * moveSpeed;
    }

    // det her styrer selve dashet og ventetiden
    private IEnumerator PerformDash()
    {
        isDashing = true;

        // sætter tiden for hvornår man må igen
        nextDashTime = Time.time + dashCooldown;

        // giver spilleren fuld fart i retningen
        rb.linearVelocity = moveInput * dashSpeed;

        // venter lige mens dashet sker
        yield return new WaitForSeconds(dashDuration);

        // nu er det slut
        isDashing = false;

        // stopper spilleren helt så man ikke glider videre
        rb.linearVelocity = Vector2.zero;
    }
}
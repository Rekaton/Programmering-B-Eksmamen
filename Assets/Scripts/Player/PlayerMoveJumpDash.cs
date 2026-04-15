using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveJumpDash : MonoBehaviour
{
    [Header("Bevægelse")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float coyoteTime = 0.2f;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashDeceleration = 40f;

    private Rigidbody2D rb;
    public Vector2 moveInput;
    public bool isDashing;
    private bool canDash;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float lastFacingDirection = 1f;

    private PlayerWallJumpnSlide wallJump;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallJump = GetComponent<PlayerWallJumpnSlide>();
    }
    private void Update()
    {
        coyoteTimeCounter -= Time.deltaTime;
    }
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput.x != 0)
        {
            lastFacingDirection = Mathf.Sign(moveInput.x);
            transform.localScale = new Vector3(lastFacingDirection, transform.localScale.y, transform.localScale.z);
        }
    }
    private void OnJump(InputValue value)
    {
        if (!value.isPressed || isDashing)
        {
            return;
        }

        if (wallJump != null)
        {
            bool didWallJump = wallJump.TryWallJump();
            if (didWallJump)
            {
                return;
            }
        }

        if (coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            coyoteTimeCounter = 0f;
        }
    }

    private void OnDash(InputValue value)
    {
        if (canDash && !isDashing)
        {
            Vector2 dashDir;
            if (moveInput == Vector2.zero)
            {
                dashDir = new Vector2(lastFacingDirection, 0);
            }
            else
            {
                dashDir = moveInput.normalized;
            }

            StartCoroutine(PerformDash(dashDir));
        }
    }
    private void FixedUpdate()
    {
        bool wallJumpActive = false;
        if (wallJump != null)
        {
            wallJumpActive = wallJump.isWallJumping;
        }

        if (!isDashing && !wallJumpActive)
        {
            float targetX = moveInput.x * moveSpeed;

            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(targetX, rb.linearVelocity.y);
            }
            else
            {
                if (Mathf.Abs(rb.linearVelocity.x) > Mathf.Abs(targetX))
                {
                    float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetX, dashDeceleration * Time.fixedDeltaTime);
                    rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
                }
                else
                {
                    rb.linearVelocity = new Vector2(targetX, rb.linearVelocity.y);
                }
            }
        }
    }
    private IEnumerator PerformDash(Vector2 direction)
    {
        isDashing = true;
        canDash = false;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            if (!isDashing)
            {
                bool isWallJumping = false;
                if (wallJump != null)
                {
                    isWallJumping = wallJump.isWallJumping;
                }

                if (!isWallJumping)
                {
                    coyoteTimeCounter = coyoteTime;
                    canDash = true;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
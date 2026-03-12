using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerWallJump : MonoBehaviour
{
    [Header("Wall Slide")]
    public float wallSlideGravity = 1.5f;
    public float normalGravity = 5f;
    public float maxWallTime = 3f;

    [Header("Wall Jump")]
    public float wallJumpForceX = 10f;
    public float wallJumpForceY = 14f;
    public float wallJumpDuration = 0.25f;

    [Header("Detection")]
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask wallLayer;
    public LayerMask groundLayer;

    public bool IsWallJumping;
    public bool IsWallSliding;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private PlayerMovement playerMovement;

    private int wallDirection;
    private float wallTimer;
    private float wallJumpTimer;

    private readonly Color colorNormal = Color.white;
    private readonly Color colorDanger = Color.red;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (playerMovement != null && playerMovement.isDashing)
        {
            return;
        }

        CheckWall();
        HandleWallSlide();
        HandleWallJumpTimer();
        UpdateColor();
    }

    public bool TryWallJump()
    {
        if (!IsWallSliding)
        {
            return false;
        }

        Vector2 jumpVelocity = new Vector2(-wallDirection * wallJumpForceX, wallJumpForceY);
        rb.linearVelocity = jumpVelocity;

        IsWallJumping = true;
        wallJumpTimer = wallJumpDuration;

        ResetWallState();
        return true;
    }

    void CheckWall()
    {
        bool leftWall = Physics2D.OverlapCircle(wallCheckLeft.position, checkRadius, wallLayer);
        bool rightWall = Physics2D.OverlapCircle(wallCheckRight.position, checkRadius, wallLayer);

        bool touchingWall;
        if (leftWall || rightWall)
        {
            touchingWall = true;
        }
        else
        {
            touchingWall = false;
        }

        if (rightWall)
        {
            wallDirection = 1;
        }
        else if (leftWall)
        {
            wallDirection = -1;
        }
        else
        {
            wallDirection = 0;
        }

        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        bool canSlide = touchingWall && !isGrounded && !IsWallJumping && wallTimer < maxWallTime;

        if (canSlide)
        {
            IsWallSliding = true;
            wallTimer += Time.deltaTime;
            rb.gravityScale = wallSlideGravity;

            if (rb.linearVelocity.y < 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -2f));
            }
        }
        else
        {
            IsWallSliding = false;

            if (!IsWallJumping)
            {
                rb.gravityScale = normalGravity;
            }

            if (isGrounded && !IsWallJumping)
            {
                ResetWallState();
            }
        }
    }

    void HandleWallSlide() { }

    void HandleWallJumpTimer()
    {
        if (!IsWallJumping) return;

        wallJumpTimer -= Time.deltaTime;
        if (wallJumpTimer <= 0f)
        {
            IsWallJumping = false;
            rb.gravityScale = normalGravity;
        }
    }

    void UpdateColor()
    {
        if (IsWallSliding && maxWallTime > 0f)
        {
            float t = Mathf.Clamp01(wallTimer / maxWallTime);
            sr.color = Color.Lerp(colorNormal, colorDanger, t);
        }
        else
        {
            sr.color = colorNormal;
        }
    }

    void ResetWallState()
    {
        wallTimer = 0f;
        IsWallSliding = false;
        sr.color = colorNormal;
        rb.gravityScale = normalGravity;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
        Gizmos.color = Color.blue;
        if (wallCheckLeft != null)
        {
            Gizmos.DrawWireSphere(wallCheckLeft.position, checkRadius);
        }
        if (wallCheckRight != null)
        {
            Gizmos.DrawWireSphere(wallCheckRight.position, checkRadius);
        }
    }
}
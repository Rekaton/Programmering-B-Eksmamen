using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerWallJumpnSlide : MonoBehaviour
{
    [Header("Wall Slide")]
    public float wallSlideGravity = 1.5f;
    public float normalGravity = 5f;
    public float maxWallTime = 3f;

    [Header("Wall Jump")]
    public float wallJumpForceX = 10f;
    public float wallJumpForceY = 14f;
    public float wallJumpDuration = 0.25f;

    public float wallCoyoteTime = 0.15f;

    [Header("Detection")]
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask wallLayer;
    public LayerMask groundLayer;

    public bool isWallJumping;
    public bool isWallSliding;

    private Rigidbody2D rb;
    private PlayerMoveJumpDash playerMovement;

    private int wallDirection;
    private int lastWallDirection;
    public float wallTimer;
    private float wallJumpTimer;
    private bool canSlide;

    private float wallCoyoteCounter;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMoveJumpDash>();
    }

    void Update()
    {
        if (playerMovement != null && playerMovement.isDashing)
        {
            return;
        }
        wallCoyoteCounter -= Time.deltaTime;
        CheckWall();
        HandleWallJumpTimer();
    }

    public bool TryWallJump()
    {
        bool canWallJump = isWallSliding || wallCoyoteCounter > 0f;

        if (!canWallJump)
        {
            return false;
        }

        int jumpDirection;
        if (isWallSliding)
        {
            jumpDirection = wallDirection;
        }
        else
        {
            jumpDirection = lastWallDirection;
        }

        Vector2 jumpVelocity = new Vector2(-jumpDirection * wallJumpForceX, wallJumpForceY);
        rb.linearVelocity = jumpVelocity;

        isWallJumping = true;
        wallJumpTimer = wallJumpDuration;

        wallCoyoteCounter = 0f;

        ResetWallState();
        return true;
    }

    void CheckWall()
    {
        bool rawLeft = Physics2D.OverlapCircle(wallCheckLeft.position, checkRadius, wallLayer);
        bool rawRight = Physics2D.OverlapCircle(wallCheckRight.position, checkRadius, wallLayer);

        bool flipped = transform.localScale.x < 0;
        bool leftWall;
        bool rightWall;

        if (flipped)
        {
            leftWall = rawRight;
            rightWall = rawLeft;
        }
        else
        {
            leftWall = rawLeft;
            rightWall = rawRight;
        }
        
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

        bool holdingTowardWall = false;
        if (playerMovement != null)
        {
            bool hasInput = playerMovement.moveInput.x != 0f;
            bool sameDirection = Mathf.Sign(playerMovement.moveInput.x) == Mathf.Sign(wallDirection);
            holdingTowardWall = hasInput && sameDirection;
        }

        if (touchingWall && !isGrounded && !isWallJumping && wallTimer < maxWallTime && rb.linearVelocity.y <= 0f && holdingTowardWall)
        {
            canSlide = true;
        }
        else
        {
            canSlide = false;
        }

        if (canSlide)
        {
            isWallSliding = true;

            lastWallDirection = wallDirection;
            wallCoyoteCounter = wallCoyoteTime;

            wallTimer += Time.deltaTime;
            rb.gravityScale = wallSlideGravity;

            if (rb.linearVelocity.y < 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -2f));
            }
        }
        else
        {
            isWallSliding = false;

            if (!isWallJumping)
            {
                rb.gravityScale = normalGravity;
            }

            if (isGrounded && !isWallJumping)
            {
                wallCoyoteCounter = 0f;
                ResetWallState();
            }
        }
    }

    void HandleWallJumpTimer()
    {
        if (!isWallJumping)
        {
            return;
        }

        wallJumpTimer -= Time.deltaTime;
        if (wallJumpTimer <= 0f)
        {
            isWallJumping = false;
            rb.gravityScale = normalGravity;
        }
    }

    void ResetWallState()
    {
        wallTimer = 0f;
        isWallSliding = false;
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
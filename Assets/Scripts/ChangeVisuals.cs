using UnityEngine;

public class ChangeVisuals : MonoBehaviour
{
    [Header("Referencer")]
    public PlayerMoveJumpDash movementScript;
    public PlayerWallJumpnSlide wallJumpScript;
    public PlayerHealthDamageRespawn healthScript;
    public SpriteRenderer sprite;

    [Header("Farver")]
    public Color dashColor;
    public Color wallSlideColor = Color.cyan;
    public Color checkpointActiveColor;
    public Color checkpointNormalColor;

    private Color normalColor;
    private Transform lastCheckpoint;
    private void Start()
    {
        normalColor = sprite.color;
    }
    private void Update()
    {
        if (movementScript.isDashing)
        {
            sprite.color = dashColor;
        }
        else if (wallJumpScript != null && wallJumpScript.isWallSliding)
        {
            float t = Mathf.Clamp01(wallJumpScript.wallTimer / wallJumpScript.maxWallTime);
            sprite.color = Color.Lerp(normalColor, wallSlideColor, t);
        }
        else
        {
            sprite.color = normalColor;
        }

        if (healthScript != null && healthScript.currentCheckpoint != null)
        {
            if (healthScript.currentCheckpoint != lastCheckpoint)
            {
                if (lastCheckpoint != null)
                {
                    SpriteRenderer oldSprite = lastCheckpoint.GetComponent<SpriteRenderer>();
                    if (oldSprite != null)
                    {
                        oldSprite.color = checkpointNormalColor;
                    }
                }

                SpriteRenderer newSprite = healthScript.currentCheckpoint.GetComponent<SpriteRenderer>();
                if (newSprite != null)
                {
                    newSprite.color = checkpointActiveColor;
                }

                lastCheckpoint = healthScript.currentCheckpoint;
            }
        }
    }
}
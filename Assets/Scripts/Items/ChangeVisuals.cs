using UnityEngine;

public class ChangeVisuals : MonoBehaviour
{
    [Header("Referencer")]
    public PlayerMoveJumpDash movementScript;
    public PlayerWallJumpnSlide wallJumpScript; // NY: Reference til wall slide scriptet
    public PlayerHealthDamageRespawn healthScript;
    public SpriteRenderer sprite;

    [Header("Farver")]
    public Color dashColor;
    public Color wallSlideColor = Color.cyan; // NY: Farve til wall slide (husk alpha til 255)
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
        // --- VISUALS FOR SPILLEREN ---
        // Vi prioriterer dash højest. Hvis der ikke dashes, tjekker vi for wall slide.
        if (movementScript.isDashing)
        {
            sprite.color = dashColor;
        }
        else if (wallJumpScript != null && wallJumpScript.IsWallSliding)
        {
            sprite.color = wallSlideColor; // Skift til wall slide farve
        }
        else
        {
            sprite.color = normalColor; // Normal farve
        }

        // --- CHECKPOINT VISUALS ---
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
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
        // Vi gemmer spillerens normale farve (ofte bare helt hvid)
        normalColor = sprite.color;
    }

    private void Update()
    {
        // --- VISUALS FOR SPILLEREN ---
        if (movementScript.isDashing)
        {
            sprite.color = dashColor;
        }
        else if (wallJumpScript != null && wallJumpScript.IsWallSliding)
        {
            // 1. Regn ud hvor lang tid vi har hængt på væggen (fra 0.0 til 1.0)
            float t = Mathf.Clamp01(wallJumpScript.wallTimer / wallJumpScript.maxWallTime);

            // 2. Blend farverne. Den starter som wallSlideColor og glider over i normalColor.
            // Når 't' er 1 (tiden er gået), er karakteren helt tilbage til sin normale farve.
            sprite.color = Color.Lerp(wallSlideColor, normalColor, t);
        }
        else
        {
            // Sørg for at farven altid er normal, når vi ikke bruger evner
            sprite.color = normalColor;
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
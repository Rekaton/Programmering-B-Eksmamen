using UnityEngine;

public class ChangeVisuals : MonoBehaviour
{
    [Header("Referencer")]
    public PlayerMoveJumpDash movementScript;
    public PlayerHealthDamageRespawn healthScript; // Tilføjet for at kunne se det aktive checkpoint
    public SpriteRenderer sprite;

    [Header("Farver")]
    public Color dashColor; // Husk at sætte A (Alpha) til 255 i Unity!
    public Color checkpointActiveColor = Color.blue;
    public Color checkpointNormalColor = Color.white;

    private Color normalColor;
    private Transform lastCheckpoint; // Holder styr på det forrige checkpoint

    private void Start()
    {
        // Gem den farve spilleren starter med
        normalColor = sprite.color;
    }

    private void Update()
    {
        // --- DASH VISUALS ---
        if (movementScript.isDashing)
        {
            sprite.color = dashColor;
        }
        else
        {
            sprite.color = normalColor;
        }

        // --- CHECKPOINT VISUALS ---
        // Tjek om vi har fat i health scriptet, og at der findes et checkpoint
        if (healthScript != null && healthScript.currentCheckpoint != null)
        {
            // Hvis det nuværende checkpoint ikke er det samme som det vi sidst gemte
            if (healthScript.currentCheckpoint != lastCheckpoint)
            {
                // Hvis vi havde et gammelt checkpoint, gør vi det hvidt (normalt) igen
                if (lastCheckpoint != null)
                {
                    SpriteRenderer oldSprite = lastCheckpoint.GetComponent<SpriteRenderer>();
                    if (oldSprite != null)
                    {
                        oldSprite.color = checkpointNormalColor;
                    }
                }

                // Gør det nye aktive checkpoint blåt
                SpriteRenderer newSprite = healthScript.currentCheckpoint.GetComponent<SpriteRenderer>();
                if (newSprite != null)
                {
                    newSprite.color = checkpointActiveColor;
                }

                // Gem det nye checkpoint som "lastCheckpoint", så vi ikke skifter farve på det hver eneste frame
                lastCheckpoint = healthScript.currentCheckpoint;
            }
        }
    }
}
using UnityEngine;

public class DashHaptics : MonoBehaviour
{
    [Header("Referencer")]
    public PlayerMovement movementScript;
    public SpriteRenderer sprite;

    [Header("Farver")]
    public Color dashColor = Color.blue; // Den blå farve til dash
    private Color normalColor; // Den normale farve gemmes her

    private void Start()
    {
        // Gem den farve spilleren starter med.
        normalColor = sprite.color;
    }

    private void Update()
    {
        // Tjek om spilleren dasher lige nu.
        if (movementScript.isDashing)
        {
            // Skift til den blå dash farve.
            sprite.color = dashColor;
        }
        else
        {
            // Skift tilbage til normal farve.
            sprite.color = normalColor;
        }
    }
}
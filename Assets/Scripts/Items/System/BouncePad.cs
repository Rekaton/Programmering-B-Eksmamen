using UnityEngine;

public class BouncePad : Platform
{
    public float bounceForce = 15f;

    public override void OnPlayerContact(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
        }
    }
}
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Platform : Obstacle
{
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerContact(collision.gameObject);
        }
    }
}
using UnityEngine;

public class DamageObstacle : Obstacle
{
    public int damage = 1;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPlayerContact(collision.gameObject);
        }
    }

    public override void OnPlayerContact(GameObject player)
    {
        PlayerHealthDamageRespawn health = player.GetComponent<PlayerHealthDamageRespawn>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
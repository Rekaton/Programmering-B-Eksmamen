using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Respawn Settings")]
    public Transform currentCheckpoint;

    void Start()
    {
        // Vi starter spillet med fuldt liv
        currentHealth = maxHealth;
    }

    // Denne metode kaldes, når spilleren støder ind i en anden Collider2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vi tjekker, om det vi rammer, har tagget "obstacle"
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1);
        }
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player hit. Current health: " + currentHealth);

        // Hvis livet er 0 eller under, skal vi respawne
        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        Debug.Log("Player dead. Respawns at checkpoint");

        // Flyt spilleren tilbage til vores checkpoint Transform
        transform.position = currentCheckpoint.position;

        // Nulstil helbredet igen
        currentHealth = maxHealth;

        // Vi nulstiller spillerens fart, så man ikke flyver afsted efter et respawn
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

}
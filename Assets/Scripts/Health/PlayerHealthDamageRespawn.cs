using UnityEngine;
using UnityEngine.UI; // Vigtigt for at bruge UI og Sliders

public class PlayerHealthDamageRespawn : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI Settings")]
    public Slider healthSlider; // Reference til din health bar

    [Header("Respawn Settings")]
    public Transform currentCheckpoint;

    void Start()
    {
        // Start spillet med fuldt liv
        currentHealth = maxHealth;

        // Sæt sliderens max værdi og fyld den helt op
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        // Flyt spilleren til det valgte checkpoint når banen starter
        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Tjek om det der rammes har tagget obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Gem det nye checkpoint hvis der løbes ind i et
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;
            Debug.Log("Nyt checkpoint gemt");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player hit Current health " + currentHealth);

        // Opdater slideren så den viser det nye liv
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Hvis livet er nul eller under skal der respawnes
        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        Debug.Log("Player dead Respawns at checkpoint");

        // Flyt spilleren tilbage til vores checkpoint transform
        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
        }

        // Nulstil helbredet igen
        currentHealth = maxHealth;

        // Fyld slideren helt op igen når der respawnes
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Nulstil spillerens fart så der ikke flyves afsted efter et respawn
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthDamageRespawn : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI Settings")]
    public Slider healthSlider;

    [Header("Respawn Settings")]
    public Transform currentCheckpoint;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            TakeDamage(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
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

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        Debug.Log("Player dead Respawns at checkpoint");

        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
        }

        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        if (DeathCounter.Instance != null)
        {
            DeathCounter.Instance.AddDeath();
        }
    }

}
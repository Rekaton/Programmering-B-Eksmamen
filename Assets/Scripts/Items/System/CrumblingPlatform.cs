using UnityEngine;
using System.Collections;

public class CrumblingPlatform : Platform
{
    public float delay = 1f;
    public float respawnDelay = 3f;

    private bool triggered = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnPlayerContact(GameObject player)
    {
        if (!triggered)
        {
            triggered = true;
            StartCoroutine(Crumble());
        }
    }

    private IEnumerator Crumble()
    {
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsed < delay)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / delay);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        GetComponent<Collider2D>().enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        GetComponent<Collider2D>().enabled = true;
        spriteRenderer.enabled = true;
        spriteRenderer.color = originalColor;
        triggered = false;
    }
}
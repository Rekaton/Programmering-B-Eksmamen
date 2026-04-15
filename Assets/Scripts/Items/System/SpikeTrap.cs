using UnityEngine;
using System.Collections;

public class SpikeTrap : DamageObstacle
{
    public float interval = 2f;
    public float warningTime = 1f;
    public SpriteRenderer warningSprite;

    private bool active = false;

    void Start()
    {
        StartCoroutine(TrapLoop());
    }

    IEnumerator TrapLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            
            float t = 0;
            while (t < warningTime)
            {
                t += Time.deltaTime;
                float alpha = t / warningTime;
                warningSprite.color = new Color(1, 1, 1, alpha);
                yield return null;
            }
            
            active = true;
            yield return new WaitForSeconds(1f);

            active = false;
            warningSprite.color = new Color(1, 1, 1, 0);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active) return;
        base.OnTriggerEnter2D(collision);
    }
}
using UnityEngine;
using System.Collections.Generic;

public class MovingPlatform : Platform
{
    public List<Vector2> points = new List<Vector2>();
    public float speed = 2f;

    private int currentIndex = 0;
    private Vector2 lastPosition;

    void Start()
    {

        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (points.Count == 0) return;

        lastPosition = transform.position;

        Vector2 target = points[currentIndex];
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.01f)
        {
            currentIndex = (currentIndex + 1) % points.Count;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector2 delta = (Vector2)transform.position - lastPosition;

        rb.position += delta;
    }
}
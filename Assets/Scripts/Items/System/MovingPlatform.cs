using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public List<Vector2> points = new List<Vector2>();
    public float speed = 2f;
    private int currentIndex = 0;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;

        if (points.Count == 0)
        {
            Debug.LogWarning("Du mangler at tilføje punkter til MovingPlatform!");
        }
    }

    void FixedUpdate()
    {
        if (points.Count == 0) return;

        Vector2 target = points[currentIndex];

        Vector2 direction = (target - rb.position).normalized;

        rb.linearVelocity = direction * speed;

        if (Vector2.Distance(rb.position, target) < 0.1f)
        {
            currentIndex = (currentIndex + 1) % points.Count;
        }
    }
}
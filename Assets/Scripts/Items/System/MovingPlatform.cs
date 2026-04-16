using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : Platform
{
    public List<Vector2> points;
    public float speed = 2f;

    private int currentIndex = 0;
    private bool movingForward = true;

    private Rigidbody2D rb;
    private Rigidbody2D carriedPlayerRb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void FixedUpdate()
    {
        if (points.Count == 0) return;

        Vector2 current = rb.position;
        Vector2 target = points[currentIndex];
        Vector2 next = Vector2.MoveTowards(current, target, speed * Time.fixedDeltaTime);

        rb.MovePosition(next);
        
        if (Vector2.Distance(next, target) < 0.01f)
        {
            AdvanceWaypoint();
        }
    }

    private void AdvanceWaypoint()
    {
        if (movingForward)
        {
            currentIndex++;
            if (currentIndex >= points.Count)
            {
                currentIndex = points.Count - 2;
                movingForward = false;
            }
        }
        else
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = 1;
                movingForward = true;
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (IsPlayerLandingOnTop(col))
        {
            carriedPlayerRb = col.rigidbody;
            carriedPlayerRb.transform.SetParent(transform);
        }
    }

    protected void OnCollisionExit2D(Collision2D col)
    {
        if (col.rigidbody == carriedPlayerRb)
        {
            carriedPlayerRb.transform.SetParent(null);
            carriedPlayerRb = null;
        }
    }

    private bool IsPlayerLandingOnTop(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return false;

        foreach (ContactPoint2D contact in col.contacts)
        {
            if (contact.normal.y > 0.5f)
                return true;
        }
        return false;
    }
}
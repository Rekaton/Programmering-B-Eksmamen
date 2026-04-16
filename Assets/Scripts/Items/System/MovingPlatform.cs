using UnityEngine;
using System.Collections.Generic;

public class MovingPlatform : Platform
{
    public List<Vector2> points;
    public float speed = 2f;

    private int currentIndex = 0;
    private bool movingForward = true;

    private Vector2 previousPosition;
    private Transform carriedPlayer;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        if (points.Count == 0) return;

        Transform t = transform;
        Vector2 target = points[currentIndex];

        t.position = Vector2.MoveTowards(t.position, target, speed * Time.deltaTime);
        Vector2 delta = (Vector2)t.position - previousPosition;
        if (carriedPlayer != null && delta != Vector2.zero)
        {
            carriedPlayer.position += (Vector3)delta;
        }
        previousPosition = t.position;

        if (Vector2.Distance(t.position, target) < 0.1f)
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
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (IsPlayerLandingOnTop(col))
            carriedPlayer = col.transform;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.transform == carriedPlayer)
            carriedPlayer = null;
    }

    // Only carry the player when they're on top — not when hitting the sides or bottom
    private bool IsPlayerLandingOnTop(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return false;

        foreach (ContactPoint2D contact in col.contacts)
        {
            if (contact.normal.y < -0.5f) // Normal points down → player is above
                return true;
        }
        return false;
    }
}
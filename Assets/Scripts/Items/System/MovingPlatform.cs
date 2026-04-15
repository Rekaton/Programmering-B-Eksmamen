using UnityEngine;
using System.Collections.Generic;

public class MovingPlatform : Platform
{
    public List<Vector2> points;
    public float speed = 2f;

    private int currentIndex = 0;
    private bool movingForward = true;

    void Update()
    {
        if (points.Count == 0) return;

        Transform t = transform;
        Vector2 target = points[currentIndex];

        t.position = Vector2.MoveTowards(t.position, target, speed * Time.deltaTime);

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
}
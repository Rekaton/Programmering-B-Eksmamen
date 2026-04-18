using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : aPlatform
{
    public List<Vector2> points = new List<Vector2>();
    public float moveSpeed = 2f;

    private int currentIndex = 0;
    private int direction = 1;

    void Start()
    {
        if (points.Count > 0)
            transform.position = points[0];
    }

    void Update()
    {
        if (points.Count < 2) return;

        Vector2 nextPosition = points[currentIndex];

        transform.position = Vector2.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

        if ((Vector2)transform.position == nextPosition)
        {
            if (currentIndex == points.Count - 1)
                direction = -1;
            else if (currentIndex == 0)
                direction = 1;

            currentIndex += direction;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
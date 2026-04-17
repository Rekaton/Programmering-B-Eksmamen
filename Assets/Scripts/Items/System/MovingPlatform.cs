using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MovingPlatform : Platform
{
    public List<Vector2> points = new List<Vector2>();
    public float speed = 2f;
    private int currentIndex = 0;

    void Start()
    {
        if (points.Count == 0)
        {
            Debug.Log("No points");
        }
    }

    void Update()
    {

        for (int i = 0; i < points.Count; i++)
        {
            if (i == currentIndex)
            {
                Vector2 target = points[i];
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, target) < 0.01f)
                {
                    currentIndex = (currentIndex + 1) %  points.Count;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }
}
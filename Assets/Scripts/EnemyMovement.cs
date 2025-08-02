using System.Linq;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Locations")]
    [SerializeField] private Transform[] points;

    [Header("Stats")]
    [SerializeField] private int moveSpeed;
    private Rigidbody2D rb;

    private int curPoint = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Vector2 dist = points[curPoint].position - transform.position;
        dist.Normalize();

        rb.linearVelocity = dist * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyPoint")) 
        {
            curPoint++;

            if (curPoint == points.Length) 
            {
                curPoint = 0;
            }
        }
    }
}

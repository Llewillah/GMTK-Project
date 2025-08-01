using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private float damageAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Timer>().ReduceTimer(damageAmount);
        }
    }
}

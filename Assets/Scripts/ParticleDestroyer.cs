using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    [SerializeField] private float lifetime;

    private void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime < 0)
        {
            Destroy(gameObject);
        }
    }
}

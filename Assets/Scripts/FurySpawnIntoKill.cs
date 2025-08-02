using UnityEngine;

public class FurySpawnIntoKill : MonoBehaviour
{
    private Transform player;
    private float moveSpeed = 3.0f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.transform;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }
}

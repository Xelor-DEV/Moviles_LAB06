using UnityEngine;

public class Enemy : DestructibleEntity
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthData playerHealth = other.GetComponent<PlayerController>().HealthData;
            playerHealth.TakeDamage(attackerStats.Damage);
            ReturnToStaticPool();
        }

        if (other.CompareTag("DeleterObstacle"))
        {
            ReturnToStaticPool();
        }
    }
}
using UnityEngine;

public class Obstacle : DestructibleEntity
{
    [SerializeField] private ScoreData scoreData;
    [SerializeField] protected HealthData healthTemplate;

    protected int currentHealth;
    protected int maxHealth;

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value <= 0)
            {
                currentHealth = 0;
            }
            else if (value > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth = value;
            }

            scoreData.CurrentScore = scoreData.CurrentScore + attackerStats.Score;

            if (currentHealth <= 0)
            {
                ReturnToStaticPool();
            }
        }
    }

    public override void Init()
    {
        base.Init();
        maxHealth = healthTemplate.MaxHealth;
        currentHealth = maxHealth;
    }



    public void TakeDamage(int amount)
    {
        CurrentHealth = CurrentHealth - amount;
    }

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
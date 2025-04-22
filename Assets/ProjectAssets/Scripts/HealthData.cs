using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Health Data", menuName = "ScriptableObjects/Player Data/Health Data", order = 1)]
public class HealthData : ScriptableObject
{
    public Action<int> onHealthChanged;
    public Action onDeath;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

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

            onHealthChanged?.Invoke(currentHealth);

            if (currentHealth <= 0)
            {
                onDeath?.Invoke();
            }
        }
    }

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }

    public void Initialize(int maxHealth)
    {
        this.maxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = CurrentHealth - amount;
    }
}

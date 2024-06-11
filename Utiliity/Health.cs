using UnityEngine;

public class Health 
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public Health(int maxHealth)
    {
        if (maxHealth < 0)
        {
            Debug.LogError("MaxHealth cant be less than zero!");
            MaxHealth = 0;
            CurrentHealth = 0;
        }
        else
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        } 
    }
    public Health(int maxHealth, int currentHealth)
    {
        if (maxHealth < 0)
        {
            Debug.LogError("Max health cant be less than zero!");
            MaxHealth = 0;
        }
        else
        {
            MaxHealth = maxHealth;
        }

        if (currentHealth < 0 || currentHealth > maxHealth)
        {
            Debug.LogError("Current health must be more than zero and less than max health!");
            CurrentHealth = 0;
        }
        else
        { 
            CurrentHealth = currentHealth; 
        } 
    }
    public void Damage(int amount)
    {
        if (CurrentHealth - amount < 0)
        {
            CurrentHealth = 0;
        }
        else if (CurrentHealth - amount > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        else
        {
            CurrentHealth -= amount;
        }
    }
}

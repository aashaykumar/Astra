using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/Enemy")]
public class EnemyStats : ScriptableObject
{
    public int maxHealth = 200;
    public int currentHealth = 100;
    public int EnemyLevel = 1;
    public int EnemyMaxLevel = 10;
    public int maxArmor = 20;
    public int ArmorMultiplerPerLevel = 2;
    public int HealthMultiplerPerLevel = 10;
    public int armor = 0;

    public int Health
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value != currentHealth)
            {
                currentHealth = Mathf.Max(value, 0);
            }
        }
    }

    public void UpdateEnemyStats()
    {
        if (EnemyLevel < EnemyMaxLevel)
        {
            EnemyLevel = EnemyLevel + 1;
            currentHealth = (currentHealth + EnemyLevel * HealthMultiplerPerLevel) >= maxHealth ? maxHealth : (currentHealth + EnemyLevel * HealthMultiplerPerLevel);
            armor = (EnemyLevel * ArmorMultiplerPerLevel) > maxArmor ? maxArmor : (EnemyLevel * ArmorMultiplerPerLevel);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        damageAmount -= armor;
        Health -= damageAmount;
    }

}

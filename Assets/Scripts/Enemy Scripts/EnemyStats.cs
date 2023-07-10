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
    public float patrolSpeed = 0.5f;
    public float chaseSpeed = 1f;
    public int attackDamage = 10;
    public float attackRange = 6;

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

    public void UpdateEnemyStats(int currentLevel)
    {
        EnemyLevel = currentLevel;
        if (EnemyLevel <= EnemyMaxLevel)
        {
            currentHealth = (currentHealth + (EnemyLevel - 1) * HealthMultiplerPerLevel) >= maxHealth ? maxHealth : (currentHealth + (EnemyLevel - 1) * HealthMultiplerPerLevel);
            armor = ((EnemyLevel - 1) * ArmorMultiplerPerLevel) > maxArmor ? maxArmor : ((EnemyLevel - 1) * ArmorMultiplerPerLevel);
            attackDamage = attackDamage + (EnemyLevel - 1) * 5;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        damageAmount -= armor;
        Health -= damageAmount;
    }

    public void ResetEnemyAllStats()
    {
        maxHealth = 200;
        currentHealth = 100;
        EnemyLevel = 1;
        EnemyMaxLevel = 10;
        maxArmor = 20;
        ArmorMultiplerPerLevel = 2;
        HealthMultiplerPerLevel = 10;
        armor = 0;
        patrolSpeed = 0.5f;
        chaseSpeed = 1f;
        attackDamage = 10;
        attackRange = 6;
    }
}

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/Player")]
public class PlayerStats : ScriptableObject
{
    public int maxHealth = 200;
    public int currentHealth = 100;
    public int playerLevel = 1;
    public int playerMaxLevel = 10;
    public int playerTempXP = 0;
    public int playerMaxXpPerLevel = 150;
    public int playerCurrentXP = 0;
    public int maxArmor = 50;
    public int armor = 5;
    public float playerAccuracy = 0;
    public float playerKDA = 0;
    public int totalEnemyKillCount = 0;
    public int totalArrowFired = 0;
    public int totalArrowHitEnemy = 0;
    public int totalPlayerDeathCount = 0;
    public int totalGoldCoin = 0;

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
                HealthChanged?.Invoke(currentHealth);
            }
        }
    }

    public UnityAction<int> HealthChanged;


    public void checkPlayerPrefsKeysAndValue()
    {
       //PlayerPrefs.DeleteAll();
        ResetPlayerTempXP();
        if (!PlayerPrefs.HasKey("PlayerHealth"))
            PlayerPrefs.SetInt("PlayerHealth", currentHealth);
        else
            currentHealth = PlayerPrefs.GetInt("PlayerHealth");

        if (!PlayerPrefs.HasKey("PlayerLevel"))
            PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        else
            playerLevel = PlayerPrefs.GetInt("PlayerLevel");

        if (!PlayerPrefs.HasKey("PlayerXP"))
            PlayerPrefs.SetInt("PlayerXP", playerCurrentXP);
        else
            playerCurrentXP = PlayerPrefs.GetInt("PlayerXP");

        if (!PlayerPrefs.HasKey("PlayerArmor"))
            PlayerPrefs.SetInt("PlayerArmor", armor);
        else
            armor = PlayerPrefs.GetInt("PlayerArmor");

        if (!PlayerPrefs.HasKey("ArrowCount"))
            PlayerPrefs.SetInt("ArrowCount", totalArrowFired);
        else
            totalArrowFired = PlayerPrefs.GetInt("ArrowCount");

        if (!PlayerPrefs.HasKey("PlayerAccuracy"))
            PlayerPrefs.SetFloat("PlayerAccuracy", playerAccuracy);
        else
            playerAccuracy = PlayerPrefs.GetInt("PlayerAccuracy");

        if (!PlayerPrefs.HasKey("TotalEnemyKillCount"))
            PlayerPrefs.SetInt("TotalEnemyKillCount", totalEnemyKillCount);
        else
            totalEnemyKillCount = PlayerPrefs.GetInt("TotalEnemyKillCount");

        if (!PlayerPrefs.HasKey("TotalPlayerDeathCount"))
            PlayerPrefs.SetInt("TotalPlayerDeathCount", totalPlayerDeathCount);
        else
            totalPlayerDeathCount = PlayerPrefs.GetInt("TotalPlayerDeathCount");

        if (!PlayerPrefs.HasKey("TotalArrowHitEnemy"))
            PlayerPrefs.SetInt("TotalArrowHitEnemy", totalArrowHitEnemy);
        else
            totalArrowHitEnemy = PlayerPrefs.GetInt("TotalArrowHitEnemy");

        if (!PlayerPrefs.HasKey("TotalGoldCoin"))
            PlayerPrefs.SetInt("TotalGoldCoin", totalGoldCoin);
        else
            totalGoldCoin = PlayerPrefs.GetInt("TotalGoldCoin");

        PlayerPrefs.Save();
        CalcuateAccuracy();
        CalculateKDA();
    }


    public void printPlayerValue()
    {
        Debug.Log(PlayerPrefs.GetInt("PlayerHealth"));
        Debug.Log(PlayerPrefs.GetInt("PlayerLevel"));
        Debug.Log(PlayerPrefs.GetInt("PlayerXP"));
        Debug.Log(PlayerPrefs.GetInt("PlayerArmor"));
        Debug.Log(PlayerPrefs.GetInt("ArrowCount"));
        Debug.Log(PlayerPrefs.GetInt("TotalEnemyKillCount"));
        Debug.Log(PlayerPrefs.GetInt("TotalPlayerDeathCount"));
        Debug.Log(PlayerPrefs.GetFloat("PlayerAccuracy"));
        Debug.Log(PlayerPrefs.GetFloat("TotalArrowHitEnemy"));
    }
    public void CalculateKDA()
    {
        if (totalPlayerDeathCount > 0)
            playerKDA = totalEnemyKillCount / totalPlayerDeathCount;
        else
            playerKDA = totalPlayerDeathCount;
    }

    private void CalcuateAccuracy()
    {
        if (totalArrowFired > 0)
        {
            playerAccuracy = totalArrowHitEnemy / totalArrowFired;
            PlayerPrefs.SetFloat("PlayerAccuracy", playerAccuracy);
            PlayerPrefs.Save();
        }
    }

    public void UpdateArrowCount()
    {
        totalArrowFired++;
        CalcuateAccuracy();
    }
    public void UpdateEnemyHitCount()
    {
        totalArrowHitEnemy++;
        CalcuateAccuracy();
    }

    public void UpdatePlayerDeathCount()
    {
        totalPlayerDeathCount++;
        PlayerPrefs.SetInt("TotalPlayerDeathCount", totalPlayerDeathCount);
        PlayerPrefs.Save();
        CalculateKDA();

    }

    public void UpdatePlayerStatsOnEnemyKill(int Enemycost)
    {
        totalEnemyKillCount++;
        playerTempXP = playerTempXP + (50 * Enemycost);
        totalGoldCoin = totalGoldCoin + (200 * Enemycost);
        PlayerPrefs.SetInt("TotalEnemyKillCount", totalEnemyKillCount);
        PlayerPrefs.SetInt("TotalGoldCoin", totalGoldCoin);
        PlayerPrefs.Save();
        CalculateKDA();
    }

    public void ResetPlayerTempXP()
    {
        playerTempXP = 0;
    }

    public void UpdatePlayerStats(int gameLevel)
    {
        if (playerLevel < playerMaxLevel)
        {
            if (playerTempXP >= playerMaxXpPerLevel)
            {
                playerTempXP = playerTempXP - playerMaxXpPerLevel;
                playerLevel = playerLevel + 1;
                currentHealth = (currentHealth +  playerLevel * 20) >= maxHealth ? maxHealth : (currentHealth + playerLevel * 20);
                armor = (playerLevel * 5) > maxArmor ? maxArmor : (playerLevel * 5);
                playerMaxXpPerLevel = (playerMaxXpPerLevel * gameLevel);
                PlayerPrefs.SetInt("PlayerLevel", playerLevel);
                PlayerPrefs.SetInt("PlayerXP", playerMaxXpPerLevel);
                PlayerPrefs.SetInt("PlayerArmor", armor);
                PlayerPrefs.SetInt("PlayerHealth", currentHealth);
                PlayerPrefs.Save();
                UpdatePlayerStats(gameLevel);
            }
            else if (playerTempXP < playerMaxXpPerLevel)
            {
                playerCurrentXP = playerTempXP;
                PlayerPrefs.SetInt("PlayerXP", playerCurrentXP);
                PlayerPrefs.Save();
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        damageAmount -= armor;
        Health -= damageAmount;
    }

}

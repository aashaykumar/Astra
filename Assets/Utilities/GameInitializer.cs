using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    public TextMeshProUGUI txtGoldValue;
    public TextMeshProUGUI txtXPValue;
    public TextMeshProUGUI txtPlayerLevel;
    public TextMeshProUGUI txtKills;
    public TextMeshProUGUI txtAccuracy;
    public TextMeshProUGUI txtKD;

    private void Awake()
    {
        InitGame();
    }

    private void InitGame()
    {
        stats.ResetPlayerAllStats();
    }

    public void UpdatePlayerStatsText()
    {
        stats.printPlayerValue();
        txtGoldValue.text = stats.totalGoldCoin.ToString();
        txtPlayerLevel.text = stats.playerLevel.ToString();
        txtXPValue.text = stats.playerCurrentXP.ToString() + "/" + stats.playerMaxXpPerLevel.ToString();
        txtKills.text = "Kills : " + stats.totalEnemyKillCount.ToString();
        txtAccuracy.text = "Accuracy : " + stats.playerAccuracy.ToString();
        txtKD.text = "KD : " + stats.playerKDA.ToString();
    }
}

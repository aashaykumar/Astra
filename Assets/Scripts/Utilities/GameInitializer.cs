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
        //InitGame();
    }

    private void InitGame()
    {
        stats.ResetPlayerAllStats();
    }
}
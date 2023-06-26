using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    GetReady,
    Playing,
    GameOver,
    GamePaused
}

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject resultScreen;
    [SerializeField] GameObject mainMenuScreen;
    [SerializeField] GameObject PauseScreen;
    [SerializeField] GameObject Stars;
    [SerializeField] GameObject NextLevel;

    public TextMeshProUGUI txtGoldValue;
    public TextMeshProUGUI txtXPValue;
    public TextMeshProUGUI txtPlayerLevel;
    public TextMeshProUGUI txtcurrentObjective;
    public TextMeshProUGUI txtTimer;
    public TextMeshProUGUI txtResultStatus;
    public TextMeshProUGUI txtResultMsg;
    public GameState currentState = GameState.GetReady;

    private int currentLevel = 1;
    private int currentObjective = 0;
    private int currentLevelObjective = 2;
    private float timer = 120f;

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private EnemyStats bossStats;

    private void Awake()
    {
        UpdateGameData();
        txtcurrentObjective.text = currentObjective.ToString() + "/" + currentLevelObjective.ToString();
        txtTimer.text = "2:00";
    }

    private void UpdateGameData()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        currentLevelObjective = currentLevel * 2;
    }

    private void Start()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (currentState == GameState.Playing)
            TimerCountDown();
    }


    public void TimerCountDown()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(timer % 60);
            int min = Mathf.FloorToInt(timer / 60);
            txtTimer.text = min.ToString() + ":" + seconds.ToString();
        }
        else if (timer <= 0)
        {
            Gamelose();
        }
    }

    public void checkforLevelObjective()
    {
        currentObjective = currentObjective + 1;
        txtcurrentObjective.text = currentObjective.ToString()+ "/" + currentLevelObjective.ToString();
        if (currentObjective == currentLevelObjective)
        {
            GameWon();
        }
    }

    public void Gamelose()
    {
        currentState = GameState.GameOver;
        AudioSystem.Instance.Play("GameLose");
        txtResultStatus.text = "DEFEAT";
        txtResultMsg.text = "YOU LOST!";
        txtGoldValue.text = playerStats.totalGoldCoin.ToString();
        txtPlayerLevel.text = playerStats.playerLevel.ToString();
        txtXPValue.text = playerStats.playerCurrentXP.ToString() + "/" + playerStats.playerMaxXpPerLevel.ToString();
        Stars.SetActive(false);
        NextLevel.SetActive(false);
        mainMenuScreen.SetActive(false);
        resultScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameWon()
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f; 
        currentLevel = currentLevel + 1;
        AudioSystem.Instance?.Play("GameWin");
        txtResultStatus.text = "VICTORY";
        txtResultMsg.text = "YOU WON!";
        txtGoldValue.text = playerStats.totalGoldCoin.ToString();
        txtPlayerLevel.text = playerStats.playerLevel.ToString();
        Stars.SetActive(true);
        NextLevel.SetActive(true);
        mainMenuScreen.SetActive(false);
        resultScreen.SetActive(true);
        playerStats.UpdatePlayerStats(currentLevel);
        enemyStats.UpdateEnemyStats();
        if ((currentLevel % 5) == 0)
            bossStats.UpdateEnemyStats();
    }

    public void GamePause()
    {
        currentState = GameState.GamePaused;
        Time.timeScale = 0f;
        PauseScreen.SetActive(true);
    }

    public void GameUnpause()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        PauseScreen.SetActive(false);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void PlayNextLevel()
    {
        //SceneManager.LoadScene("Level_1");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenuScreen() 
    {
        SceneManager.LoadScene("ScreenTransitions");
    }

    public int GetCurrentLevel() 
    {
        return currentLevel;
    }
}

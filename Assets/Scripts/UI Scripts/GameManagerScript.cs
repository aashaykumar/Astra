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
    public TextMeshProUGUI txtcurrentObjective;
    public TextMeshProUGUI txtTimer;
    public GameState currentState = GameState.GetReady;
    public TextMeshProUGUI txtResultStatus;
    public TextMeshProUGUI txtResultMsg;

    private int currentGoldValue = 0;
    private int currentLevel = 1;
    private int currentObjective = 0;
    private int currentLevelObjective = 1;
    private float timer = 120f;

    private void Awake()
    {
        txtcurrentObjective.text = currentObjective.ToString() + "/" + currentLevelObjective.ToString();
        txtTimer.text = "2:00";
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

    public void updatePlayerStats()
    {

    }

    public void Gamelose()
    {
        txtResultStatus.text = "DEFEAT";
        txtResultMsg.text = "YOU LOST!";
        Stars.SetActive(false);
        NextLevel.SetActive(false);
        currentState = GameState.GameOver;
        mainMenuScreen.SetActive(false);
        resultScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameWon()
    {
        txtResultStatus.text = "VICTORY";
        txtResultMsg.text = "YOU WON!";
        Stars.SetActive(true);
        NextLevel.SetActive(true);
        currentState = GameState.GameOver;
        currentLevel = currentLevel + 1;
        mainMenuScreen.SetActive(false);
        resultScreen.SetActive(true);
        Time.timeScale = 0f;
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
        SceneManager.LoadScene("Level_1");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenuScreen() 
    {
        SceneManager.LoadScene("ScreenTransitions");
    }
}

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
        enemyStats.UpdateEnemyStats(currentLevel);
        if (currentLevel % 3 == 0)
        {
            bossStats.UpdateEnemyStats(currentLevel);
        }
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
           GameWon();
    }

    public void Gamelose()
    {
        currentState = GameState.GameOver;
        systems.Instance.GetComponentInChildren<AudioSystem>().Play("GameLose");
        Time.timeScale = 0f;
        txtResultStatus.text = "DEFEAT";
        txtResultMsg.text = "YOU LOST!";
        txtGoldValue.text = playerStats.totalGoldCoin.ToString();
        txtPlayerLevel.text = playerStats.playerLevel.ToString();
        txtXPValue.text = playerStats.playerCurrentXP.ToString() + "/" + playerStats.playerMaxXpPerLevel.ToString();
        Stars.SetActive(false);
        NextLevel.SetActive(false);
        resultScreen.SetActive(true);
    }

    public void GameWon()
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        if (currentLevel < 5 && currentLevel == playerStats.GameCurrentLevel)
        {
            playerStats.GameCurrentLevel = playerStats.GameCurrentLevel + 1;
            playerStats.UpdatePlayerStats(currentLevel, false);
        }
        else if ((currentLevel < 5 && currentLevel != playerStats.GameCurrentLevel))
        {
            playerStats.UpdatePlayerStats(currentLevel,true);
        }
        else if (currentLevel == 5)
        {
            currentLevel = 1;
            playerStats.GameCurrentLevel = 1;
            playerStats.ResetPlayerAllStats();
            enemyStats.ResetEnemyAllStats();
        }
        systems.Instance.GetComponentInChildren<AudioSystem>().Play("GameWin");
        txtResultStatus.text = "VICTORY";
        txtResultMsg.text = "YOU WON!";
        txtGoldValue.text = playerStats.totalGoldCoin.ToString();
        txtPlayerLevel.text = playerStats.playerLevel.ToString();
        txtXPValue.text = playerStats.playerCurrentXP.ToString() + "/" + playerStats.playerMaxXpPerLevel.ToString();
        Stars.SetActive(true);
        NextLevel.SetActive(true);
        resultScreen.SetActive(true);
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
        playerStats.ResetPlayerTempXP();
        systems.Instance.GetComponentInChildren<GUIScript>().LoadScene(currentLevel);
        //SceneManager.LoadScene(currentLevel);
    }

    public void PlayNextLevel()
    {
        playerStats.ResetPlayerTempXP();
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            playerStats.ResetPlayerAllStats();
            systems.Instance.GetComponentInChildren<GUIScript>().LoadScene(1);
            //SceneManager.LoadScene("Level_1");
        }
        else
            systems.Instance.GetComponentInChildren<GUIScript>().LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenuScreen() 
    {
        systems.Instance.GetComponentInChildren<GUIScript>().LoadScene(0);
        //SceneManager.LoadScene("ScreenTransitions");
        systems.Instance.GetComponentInChildren<GUIScript>().LoadMainMenuScreen();
    }
    
    public void LoadSettingScreen()
    {
        systems.Instance.GetComponentInChildren<GUIScript>().LoadSettingScreen();
    }

    public int GetCurrentLevel() 
    {
        return currentLevel;
    }
}

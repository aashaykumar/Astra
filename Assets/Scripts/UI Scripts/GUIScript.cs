using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GUIScript : MonoBehaviour
{
    [SerializeField] GameObject MainMenuScreen;
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] GameObject SettingsScreen;
    [SerializeField] GameObject StoreScreen;

    [SerializeField] PlayerStats playerStats;

    private int currentLevel;

    public TextMeshProUGUI txtGoldValue;
    public TextMeshProUGUI txtXPValue;
    public TextMeshProUGUI txtPlayerLevel;
    public TextMeshProUGUI txtKills;
    public TextMeshProUGUI txtAccuracy;
    public TextMeshProUGUI txtKD;
    public int progress = 0;
    public UnityEngine.UI.Slider loadingBar;


    public void UpdateLoadingProgress()
    {
        progress++;
        loadingBar.value = progress;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        playerStats.ResetPlayerTempXP();
        currentLevel = playerStats.GameCurrentLevel;
        LoadScene(currentLevel);
        // SceneManager.LoadScene(currentLevel);
    }

    public void LoadSettingScreen()
    {
        MainMenuScreen.SetActive(false);
        SettingsScreen.SetActive(true);
    }

    public void LoadMainMenuScreen()
    {
        MainMenuScreen.SetActive(true);
    }

    public void CloseSettingScreen()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index == 0)
        {
            MainMenuScreen.SetActive(true);
            SettingsScreen.SetActive(false);
        }
        else
        {
            SettingsScreen.SetActive(false);
        }
    }

    public async void LoadScene(int index)
    {
        loadingBar.value = 0f;
        var scene = SceneManager.LoadSceneAsync(index);
        scene.allowSceneActivation = false;
        MainMenuScreen.SetActive(false);
        LoadingScreen.SetActive(true);

        do
        {
            await Task.Delay(100);
            loadingBar.value = scene.progress;
        } while (scene.progress < 0.9f);

        await Task.Delay(500);
        scene.allowSceneActivation = true;
        LoadingScreen.SetActive(false);
    }

    public void UpdatePlayerStatsText()
    {
        playerStats.printPlayerValue();
        txtGoldValue.text = playerStats.totalGoldCoin.ToString();
        txtPlayerLevel.text = playerStats.playerLevel.ToString();
        txtXPValue.text = playerStats.playerCurrentXP.ToString() + "/" + playerStats.playerMaxXpPerLevel.ToString();
        txtKills.text = "Kills : " + playerStats.totalEnemyKillCount.ToString();
        txtAccuracy.text = "Accuracy : " + playerStats.playerAccuracy.ToString();
        txtKD.text = "KD : " + playerStats.playerKDA.ToString();
    }
}

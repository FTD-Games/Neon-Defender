using TMPro; // Required for TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI versionsText; // Reference to the TextMeshPro text field
    public GameObject levelSelectionDisplay;
    public GameObject settingsMenu;
    public GameObject upgradeMenu;

    private void Start()
    {
        settingsMenu.GetComponent<Settings>().LoadSettings();
        UpdateVersionText();
        SceneManager.sceneLoaded += GameControl.control.OnSceneLoaded;
    }

    /// <summary>
    /// Starts the game and loads the target scene.
    /// </summary>
    public void StartGame()
    {
        CloseAll();
        levelSelectionDisplay.SetActive(true);
    }

    /// <summary>
    /// Open the settings menu (or popup).
    /// </summary>
    public void OpenSettings()
    {
        CloseAll();
        settingsMenu.SetActive(true);
        settingsMenu.GetComponent<Settings>().ShowPause(false);
    }

    /// <summary>
    /// Open the upgrade menu (or popup).
    /// </summary>
    public void OpenUpgrades()
    {
        CloseAll();
        upgradeMenu.SetActive(true);
    }

    /// <summary>
    /// Stops the application and ends the game.
    /// </summary>
    public void QuitGame() => Application.Quit();

    private void CloseAll()
    {
        levelSelectionDisplay.SetActive(false);
        settingsMenu.SetActive(false);
        upgradeMenu.SetActive(false);
    }

    /// <summary>
    /// Update the version text in the UI with the current GameControl data.
    /// </summary>
    private void UpdateVersionText() => versionsText.text = $"Version: {GameControl.control.profileVersion}{GameControl.control.gameVersion}";
}

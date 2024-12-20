using TMPro; // Required for TextMeshPro
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("UI References")] public TextMeshProUGUI versionsText; // Reference to the TextMeshPro text field

    private void Start()
    {
        UpdateVersionText();
    }

    /// <summary>
    /// Starts the game and loads the target scene.
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Start Game");
        GameControl.control.sceneLoader.LoadScene((int)Enums.E_Levels.LevelOne, $"Loading: {Enums.E_Levels.LevelOne}");
    }

    /// <summary>
    /// Open the settings menu (or popup).
    /// </summary>
    public void OpenSettings()
    {
        Debug.Log("Open Settings");
    }

    /// <summary>
    /// Stops the application and ends the game.
    /// </summary>
    public void QuitGame() => Application.Quit();

    /// <summary>
    /// Update the version text in the UI with the current GameControl data.
    /// </summary>
    private void UpdateVersionText() => versionsText.text = $"Version: {GameControl.control.profileVersion}{GameControl.control.gameVersion}";
}

using System;
using TMPro; // Required for TextMeshPro
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("UI References")] public TextMeshProUGUI versionsText; // Reference to the TextMeshPro text field

    private void Start()
    {
        System.Action updateVersionsText = () =>
        {
            versionsText.text = $"Version: {GameControl.control.profileVersion}{GameControl.control.gameVersion}";
        };
        updateVersionsText();
    }

    /// <summary>
    /// Starts the game and loads the target scene.
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Start Game");
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
}

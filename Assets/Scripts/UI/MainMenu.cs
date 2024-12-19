using System;
using TMPro; // Required for TextMeshPro
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("UI References")] public TextMeshProUGUI versionsText; // Reference to the TextMeshPro text field

    private void Start()
    {
        // Check if the text field and GameControl instance exist
        if (versionsText != null && GameControl.control != null)
        {
            // Set the text of the text field to the game version from GameControl
            versionsText.text = "Version: " + GameControl.control.profileVersion + GameControl.control.gameVersion;
        }
        else
        {
            // Warning if any reference is missing
            Debug.LogWarning("Version Text UI or GameControl is not assigned properly!");
        }
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

using UnityEngine;

public class MainMenu : MonoBehaviour
{
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

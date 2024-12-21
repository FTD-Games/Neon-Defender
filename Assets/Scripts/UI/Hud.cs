using UnityEngine;

public class Hud : MonoBehaviour
{
    private static bool _gameIsPaused;
    public static bool GameIsPaused
    {
        get { return _gameIsPaused; }
        set
        {
            _gameIsPaused = value;
            Time.timeScale = value ? 0 : 1;
        }
    }

    private bool _isPaused;
    public LevelDisplay levelDisplay;
    public GameObject fpsDisplay;
    public Settings settingsMenu;

    private void Start()
    {
        settingsMenu.LoadSettings();
        fpsDisplay.SetActive(settingsMenu.toggleFpsDisplay.isOn);
        levelDisplay.SetExpProgress(100, 75);
        levelDisplay.SetLevel(44);
    }

    public void SetPause()
    {
        GameIsPaused = !GameIsPaused;
        settingsMenu.gameObject.SetActive(GameIsPaused);
        if (!GameIsPaused)
            return;
        settingsMenu.ShowPause(true);
    }
}

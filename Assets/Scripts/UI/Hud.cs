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
    [SerializeField]
    private LevelDisplay levelDisplay;
    public GameObject fpsDisplay;
    public Settings settingsMenu;

    private void Start()
    {
        settingsMenu.LoadSettings();
        fpsDisplay.SetActive(settingsMenu.toggleFpsDisplay.isOn);
    }

    public void SetPause()
    {
        GameIsPaused = !GameIsPaused;
        settingsMenu.gameObject.SetActive(GameIsPaused);
        if (!GameIsPaused)
            return;
        settingsMenu.ShowPause(true);
    }

    /// <summary>
    /// Refresh the displayed exp.
    /// </summary>
    public void RefreshExpProgress(float currentExp, float levelUpExp) => levelDisplay.SetExpProgress(currentExp, levelUpExp);

    /// <summary>
    /// Refresh the displayed level.
    /// </summary>
    public void RefreshLevel(int currentLvl) => levelDisplay.SetLevel(currentLvl);
}

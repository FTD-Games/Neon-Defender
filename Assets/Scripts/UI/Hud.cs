using System;
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
    [SerializeField]
    private GameTime gameTime;
    [SerializeField]
    private WaveTime waveTime;
    public GameObject fpsDisplay;
    public GameObject statsDisplay;
    private StatsDisplay _statsDisplay;
    public Settings settingsMenu;
    [SerializeField]
    private Sprite monsterIcon;
    [SerializeField]
    private Sprite bossIcon;
    [SerializeField]
    private Sprite idleIcon;

    private void Awake()
    {
        _statsDisplay = statsDisplay.GetComponent<StatsDisplay>();
    }

    private void Start()
    {
        settingsMenu.LoadSettings();
        fpsDisplay.SetActive(settingsMenu.toggleFpsDisplay.isOn);
        gameTime.StartStopGameTime(true);
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

    /// <summary>
    /// Setup new wave for display.
    /// </summary>
    public void SetNewWave(Enums.E_SpawnState state, int duration)
    {
        var newTime = new TimeSpan(0, 0, duration);
        switch (state)
        {
            case Enums.E_SpawnState.Idle:
                waveTime.SetNewWave(idleIcon, newTime);
                break;
            case Enums.E_SpawnState.Monster:
                waveTime.SetNewWave(monsterIcon, newTime);
                break;
            case Enums.E_SpawnState.Boss:
                waveTime.SetNewWave(bossIcon, newTime);
                break;
        }
    }

    /// <summary>
    /// Refreshs the displayed time on the wave time display on hud.
    /// </summary>
    public void RefreshWaveTime(int duration) => waveTime.RefreshTime(new TimeSpan(0, 0, duration));

    public void RefreshHealthStat(float value) => _statsDisplay.RefreshHealthStat(value);
    public void RefreshArmorStat(float value) => _statsDisplay.RefreshArmorStat(value);
    public void RefreshSpeedStat(float value) => _statsDisplay.RefreshSpeedStat(value);
    public void RefreshDamageStat(float value) => _statsDisplay.RefreshDamageStat(value);
    public void RefreshExperienceStat(float value) => _statsDisplay.RefreshExperienceStat(value);
    public void RefreshCritChanceStat(float value) => _statsDisplay.RefreshCritChanceStat(value);
    public void RefreshCritDamageStat(float value) => _statsDisplay.RefreshCritDamageStat(value);
}

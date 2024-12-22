using System;
using TMPro;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private bool _active;

    private int _currMin;
    private float _nextTimeStep;
    public static string CurrentGameTime;
    private TextMeshProUGUI _timeText;
    private static TimeSpan _currTimeSpan = new TimeSpan(0, 15, 0);

    // Use this for initialization
    void Start()
    {
        CurrentGameTime = $"15:00";
        _timeText = GetComponent<TextMeshProUGUI>();
        _nextTimeStep = Time.unscaledTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_active)
            return;

        // Timecounting
        if (Time.unscaledTime > _nextTimeStep && !Hud.GameIsPaused)
        {
            _currTimeSpan = _currTimeSpan.Subtract(TimeSpan.FromSeconds(1));
            var timeStr = _currTimeSpan.ToString(@"mm\:ss");
            _timeText.text = timeStr;
            CurrentGameTime = timeStr;
            _nextTimeStep = Time.unscaledTime + 1f;
            if (_currMin < GetPlaytimeInMinutes())
                _currMin = GetPlaytimeInMinutes();
        }
    }

    public static int GetPlaytimeInMinutes() => (int)_currTimeSpan.TotalMinutes;

    public static int GetPlaytimeInSeconds() => (int)_currTimeSpan.TotalSeconds;

    public void StartStopGameTime(bool active) => _active = active;
}

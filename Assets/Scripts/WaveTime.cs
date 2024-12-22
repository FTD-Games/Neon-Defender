using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveTime : MonoBehaviour
{
    public Image waveIcon;
    private TextMeshProUGUI _timeText;

    // Use this for initialization
    void Awake()
    {
        _timeText = GetComponent<TextMeshProUGUI>();
    }

    public void SetNewWave(Sprite icon, TimeSpan time)
    {
        waveIcon.sprite = icon;
        _timeText.text = time.ToString(@"mm\:ss");
    }

    public void RefreshTime(TimeSpan time) => _timeText.text = time.ToString(@"mm\:ss");
}

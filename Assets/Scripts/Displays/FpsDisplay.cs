using TMPro;
using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
    private float _deltaTime;
    private float interval = 0.25f;
    private TextMeshProUGUI _display;

    void Awake()
    {
        _display = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        // calculation
        _deltaTime += (deltaTime - _deltaTime) * 0.1f;

        // visual display
        if (interval > 0)
        {
            interval -= deltaTime;
            return;
        }

        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        float fps = 1.0f / _deltaTime;
        interval = 0.25f;
        _display.text = $"{Mathf.Ceil(fps)} FPS";
    }
}

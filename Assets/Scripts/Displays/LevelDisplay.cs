using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    
    public Image ExpProgress;
    public TextMeshProUGUI LevelText;

    /// <summary>
    /// Updates the ExpProgress with the maximum and current values
    /// </summary>
    public void SetExpProgress(float maximum, float current) => ExpProgress.fillAmount = Mathf.Clamp01((1 / maximum) * current);

    /// <summary>
    /// Updates the Level Text.
    /// </summary>
    public void SetLevel(int level) => LevelText.text = $"Level: {level}";

}

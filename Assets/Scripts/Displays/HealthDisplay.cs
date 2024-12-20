using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    
    public Image healthProgress;

    /// <summary>
    /// Updates the HealthDisplay with the maximum and current values
    /// </summary>
    public void SetHealth(float maximum, float current) => healthProgress.fillAmount = Mathf.Clamp01((1 / maximum) * current);

}

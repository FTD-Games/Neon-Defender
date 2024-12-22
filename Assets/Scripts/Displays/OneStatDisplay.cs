using TMPro;
using UnityEngine;

public class OneStatDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI valueText;

    public void RefreshStat(float value) => valueText.text = $"{value:f2}";
}

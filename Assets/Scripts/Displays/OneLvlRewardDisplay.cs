using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OneLvlRewardDisplay : MonoBehaviour
{
    #region UI REFERENCES
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI descText;
    [SerializeField]
    private TextMeshProUGUI valueText;
    [SerializeField]
    private TextMeshProUGUI lvlText;
    #endregion UI REFERENCES

    private RewardController _rewardController;
    private int _rewardNr;
    private GameControl.RewardData _rewardData;

    /// <summary>
    /// Initializes the reward display data.
    /// </summary>
    public void SetupRewardDisplay(RewardController controller, int rewardNr)
    {
        _rewardController = controller;
        _rewardNr = rewardNr;
    }

    /// <summary>
    /// Set a new reward to display.
    /// </summary>
    public void SetRewardDisplay(GameControl.RewardData data)
    {
        _rewardData = data;
        icon.sprite = _rewardData.GetIcon();
        titleText.text = _rewardData.GetTitle();
        descText.text = _rewardData.GetDescription();
        valueText.text = $"{_rewardData.Value}";
        lvlText.text = $"Lvl. {_rewardData.Level}";
    }

    /// <summary>
    /// Called by pressing one of the take buttons from the rewards
    /// </summary>
    public void TakeReward() => _rewardController.TriggerRewardTaken(_rewardData);
}

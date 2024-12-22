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
    public void SetRewardDisplay()
    {

    }

    /// <summary>
    /// Called by pressing one of the take buttons from the rewards
    /// </summary>
    /// <param name="rewardNr">Assigned in inspector</param>
    public void TakeReward() => _rewardController.TriggerRewardTaken(_rewardNr);
}

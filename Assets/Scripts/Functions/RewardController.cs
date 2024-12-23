using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    public event Action<GameControl.RewardData> onRewardTaken;
    private List<OneLvlRewardDisplay> _rewardDisplays = new List<OneLvlRewardDisplay>();
    [SerializeField]
    private TextMeshProUGUI prevLvlText;
    [SerializeField]
    private TextMeshProUGUI nextLvlText;
    [SerializeField]
    private Animator anim;

    /// <summary>
    /// Initializes the reward controller, that also initializes the single reward displays.
    /// </summary>
    public void SetupRewardController(Action<GameControl.RewardData> playerCallback)
    {
        _rewardDisplays.AddRange(GetComponentsInChildren<OneLvlRewardDisplay>(includeInactive: true));
        onRewardTaken += playerCallback;
        for (int i = 0; i < _rewardDisplays.Count; i++)
        {
            _rewardDisplays[i].SetupRewardDisplay(this, i);
        }
    }

    /// <summary>
    /// Generates 5 new reward displays with data to pick.
    /// </summary>
    public void SetLevelUp(int newLvl)
    {
        prevLvlText.text = $"{newLvl - 1}.";
        nextLvlText.text = $"{newLvl}.";
        gameObject.SetActive(true);
        anim.Play("LevelUp");
        for (int i = 0; i < _rewardDisplays.Count; i++)
        {
            _rewardDisplays[i].SetRewardDisplay(GenerateRandomReward());
        }
    }

    private GameControl.RewardData GenerateRandomReward()
    {
        var reward = GameControl.control.rewardData[UnityEngine.Random.Range(0, GameControl.control.rewardData.Count)];
        reward.Level = 1;
        reward.Value = 1;
        return reward;
    }

    /// <summary>
    /// Called by one of the reward displays.
    /// </summary>
    public void TriggerRewardTaken(GameControl.RewardData reward) => onRewardTaken?.Invoke(reward);

    public void Close() => gameObject.SetActive(false);
}

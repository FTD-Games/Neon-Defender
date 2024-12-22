using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    public event Action<int> onRewardTaken;
    private List<OneLvlRewardDisplay> _rewardDisplays = new List<OneLvlRewardDisplay>();

    /// <summary>
    /// Initializes the reward controller, that also initializes the single reward displays.
    /// </summary>
    public void SetupRewardController(Action<int> playerCallback)
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
        gameObject.SetActive(true);
        for (int i = 0; i < _rewardDisplays.Count; i++)
        {
            _rewardDisplays[i].SetRewardDisplay();
        }
    }

    /// <summary>
    /// Called by one of the reward displays.
    /// </summary>
    /// <param name="rewardNr"></param>
    public void TriggerRewardTaken(int rewardNr) => onRewardTaken?.Invoke(rewardNr);

    public void Close() => gameObject.SetActive(false);
}

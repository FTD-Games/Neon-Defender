using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField]
    private OneStatDisplay _healthStat;
    [SerializeField]
    private OneStatDisplay _armorStat;
    [SerializeField]
    private OneStatDisplay _speedStat;
    [SerializeField]
    private OneStatDisplay _damageStat;
    [SerializeField]
    private OneStatDisplay _experienceStat;
    [SerializeField]
    private OneStatDisplay _critChanceStat;
    [SerializeField]
    private OneStatDisplay _critDamageStat;

    public void RefreshHealthStat(float value) => _healthStat.RefreshStat(value);
    public void RefreshArmorStat(float value) => _armorStat.RefreshStat(value);
    public void RefreshSpeedStat(float value) => _speedStat.RefreshStat(value);
    public void RefreshDamageStat(float value) => _damageStat.RefreshStat(value);
    public void RefreshExperienceStat(float value) => _experienceStat.RefreshStat(value);
    public void RefreshCritChanceStat(float value) => _critChanceStat.RefreshStat(value);
    public void RefreshCritDamageStat(float value) => _critDamageStat.RefreshStat(value);
}

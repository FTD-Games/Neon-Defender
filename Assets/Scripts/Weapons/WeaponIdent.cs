using System.Collections.Generic;
using UnityEngine;

public class WeaponIdent : MonoBehaviour
{
    /// <summary>
    /// The sequence order the weapon is attached to the player.
    /// </summary>
    public int SequenceNr { get; set; }
    /// <summary>
    /// Rewards with that enum are affecting this weapon.
    /// </summary>
    public Enums.E_Weapon type;
    /// <summary>
    /// Rewards with that enum are affecting this weapon.
    /// </summary>
    public List<Enums.E_Reward> matchingRewards;
}

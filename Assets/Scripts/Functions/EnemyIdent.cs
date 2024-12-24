using UnityEngine;

public class EnemyIdent : MonoBehaviour
{
    public Enums.E_Monster monster;
    public Enums.E_Bosses boss;

    public bool IsBoss() => boss != Enums.E_Bosses.None;
}

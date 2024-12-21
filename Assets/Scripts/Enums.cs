public class Enums
{
    public enum E_Version {Alpha, Beta, EarlyAccess, FullRelease}
    /// <summary>
    /// This enum contains all levels. Attention: The enum has to match the Scene NUMBER in build sequence.
    /// </summary>
    public enum E_Levels { None, LevelOne, LevelTwo, LevelThree }
    public enum E_Monster { None, Verox}
    /// <summary>
    /// Current action of the spawner.
    /// </summary>
    public enum E_SpawnState { Idle, Monster, Boss }
    public enum E_Difficulty { Easy, Normal, Hard, Insane }
}

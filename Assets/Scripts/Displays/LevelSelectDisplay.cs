using UnityEngine;

public class LevelSelectDisplay : MonoBehaviour
{
    public Enums.E_Levels level;

    public void StartLevel() => GameControl.control.SceneLoader.LoadScene((int)level, $"Loading: {level}");
}

using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl control;

    /// <summary>
    /// State of development. (ALPHA, BETA, ...)
    /// </summary>
    public Enums.E_Version developmentState;
    /// <summary>
    /// Version of the profile that is saved with A, B, ...
    /// </summary>
    public string profileVersion;
    /// <summary>
    /// Game version of the build 001, 002, ...
    /// </summary>
    public string gameVersion;
    /// <summary>
    /// Scene Manager for changing scenes or general scene management.
    /// </summary>
    public SceneLoader SceneLoader {  get; set; }

    void Awake()
    {
        // consistant game controller with data
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }
}

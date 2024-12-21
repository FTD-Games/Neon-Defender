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
    public SceneLoader SceneLoader {  get; set; } // Property so the Unity inspector is not showing it.
    /// <summary>
    /// Loaded Player Profile
    /// </summary>
    public Profile.SaveData profile;
    /// <summary>
    /// Selected difficulty is used on start (or in runtime) for the difficulty.
    /// Set on level selection.
    /// </summary>
    public Enums.E_Difficulty selectedDifficulty;

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

        InitData();
    }

    private void Start()
    {
        
    }

    private void InitData()
    {
        var fileHandler = new Profile(true);

        if (!fileHandler.ProfileExists()) {
            var data = fileHandler.CreateNewProfile();
            fileHandler.SaveProfile(data);
        }

        control.profile = fileHandler.LoadProfile();
        control.profile.IsMigrationChecking = true;
        
        if (!fileHandler.NeedsMigration(control.profile)) {
            return;
        }

        fileHandler.Migrate(control.profile);
        fileHandler.SaveProfile(control.profile);
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    /// <summary>
    /// Current in game HUD.
    /// </summary>
    public GameObject CurrentHUD { get; set; } // Property so the Unity inspector is not showing it.
    /// <summary>
    /// List of ExpOrb data. 
    /// </summary>
    public List<OrbData> OrbDataList = new List<OrbData>();

    #region PREFABS THAT CAN BE INSTANTIATED
    public GameObject hudPrefab;
    #endregion PREFABS THAT CAN BE INSTANTIATED

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

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) // init the gameobjects
    {
        if (scene.buildIndex == 0) // Currently return on main menu
            return;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        CurrentHUD = Instantiate(hudPrefab, null);
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

    [Serializable]
    public class OrbData
    {
        public Enums.E_ExpOrbType Type;
        public Color Color;
        public float Experience;
        public float Size;
    }

}

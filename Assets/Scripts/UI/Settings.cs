using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public class Settings : MonoBehaviour
{
    public Slider sliderVol;
    public Slider sliderMus;
    public Slider sliderSounds;
    public Toggle toggleFpsDisplay;
    public Toggle toggleStatsDisplay;
    public AudioMixer audioMixer;
    public List<GameObject> inGameUiElements;

    public void LoadSettings()
    {
        // Set or get overall Volume
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", sliderVol.value);
        }
        else
        {
            sliderVol.value = PlayerPrefs.GetFloat("Volume");
        }
        SetMasterVolume(sliderVol.value);
        // Set or get Musicvolume
        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetFloat("Music", sliderMus.value);
        }
        else
        {
            sliderMus.value = PlayerPrefs.GetFloat("Music");
        }
        SetMusicVolume(sliderMus.value);
        // Set or get Soundsvolume
        if (!PlayerPrefs.HasKey("Sounds"))
        {
            PlayerPrefs.SetFloat("Sounds", sliderSounds.value);
        }
        else
        {
            sliderSounds.value = PlayerPrefs.GetFloat("Sounds");
        }
        SetSoundsVolume(sliderSounds.value);
        // Set or get displaying fps
        if (!PlayerPrefs.HasKey("FPS_display"))
        {
            PlayerPrefs.SetInt("FPS_display", 1);
            toggleFpsDisplay.isOn = true;
        }
        else
        {
            toggleFpsDisplay.isOn = PlayerPrefs.GetInt("FPS_display") == 1;
        }
        // Set or get displaying stats
        if (!PlayerPrefs.HasKey("STATS_display"))
        {
            PlayerPrefs.SetInt("STATS_display", 1);
            toggleStatsDisplay.isOn = true;
        }
        else
        {
            toggleStatsDisplay.isOn = PlayerPrefs.GetInt("STATS_display") == 1;
        }
    }

    /// <summary>
    /// Shows specific content for example: In game -> leave button.
    /// </summary>
    public void ShowPause(bool isInGame)
    {
        foreach (var element in inGameUiElements)
        {
            element.SetActive(isInGame);
        }
    }

    public void SetMasterVolume(float inValue)
    {
        var targetVol = (20 * inValue) - 20;
        if (targetVol == -20)
        {
            audioMixer.SetFloat("VOLUME", -80);
            PlayerPrefs.SetFloat("Volume", inValue);
        }
        else
        {
            audioMixer.SetFloat("VOLUME", targetVol);
            PlayerPrefs.SetFloat("Volume", inValue);
        }
    }

    public void SetMusicVolume(float inValue)
    {
        var targetVol = (20 * inValue) - 20;
        if (targetVol == -20)
        {
            audioMixer.SetFloat("MUSIC", -80);
            PlayerPrefs.SetFloat("Music", inValue);
        }
        else
        {
            audioMixer.SetFloat("MUSIC", targetVol);
            PlayerPrefs.SetFloat("Music", inValue);
        }
    }

    public void SetSoundsVolume(float inValue)
    {
        var targetVol = (20 * inValue) - 20;
        if (targetVol == -20)
        {
            audioMixer.SetFloat("SOUND", -80);
            PlayerPrefs.SetFloat("Sounds", inValue);
        }
        else
        {
            audioMixer.SetFloat("SOUND", targetVol);
            PlayerPrefs.SetFloat("Sounds", inValue);
        }
    }

    public void SetFPSDisplay(bool isOn)
    {
        PlayerPrefs.SetInt("FPS_display", isOn ? 1 : 0);
        // set the hud display
        if (GameControl.control.CurrentHUD != null && GameControl.control.CurrentHUD.TryGetComponent(out Hud theHud))
            theHud.fpsDisplay.SetActive(isOn);
    }

    public void SetSTATSDisplay(bool isOn)
    {
        PlayerPrefs.SetInt("STATS_display", isOn ? 1 : 0);
        // set the hud display
        if (GameControl.control.CurrentHUD != null && GameControl.control.CurrentHUD.TryGetComponent(out Hud theHud))
            theHud.statsDisplay.SetActive(isOn);
    }

    public void LeaveGame()
    {
        Hud.GameIsPaused = false;
        GameControl.control.SceneLoader.LoadScene((int)Enums.E_Levels.None, "Back to main menu...");
    }

    public void QuitGame() => Application.Quit();
}

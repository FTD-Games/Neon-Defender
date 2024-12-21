using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class AudioDataCollection : MonoBehaviour
{
    private AudioSource _lastPlaying;
    public List<AudioData> soundData = new List<AudioData>();

    public void InitAudio()
    {
        enabled = false;
    }

    private void Update()
    {
        if (!_lastPlaying.isPlaying)
            Destroy(gameObject);
    }

    /// <summary>
    /// Picks a random sound from the category and plays it once.
    /// </summary>
    public void PlayTargetSound(Enums.E_Sound toPlay)
    {
        var foundData = soundData.Where(x => x.sound == toPlay).ToArray();
        if (foundData == null || foundData.Length == 0)
        {
            Debug.Log($"Sounds not found for #_{toPlay}");
            return;
        }
        var randomIndex = UnityEngine.Random.Range(0, foundData.Length);
        foundData[randomIndex].audioSource.pitch = 1f;
        foundData[randomIndex].audioSource.Play();
    }

    /// <summary>
    /// Picks a random sound from the category and plays it once.
    /// </summary>
    public void PlayTargetSoundWithRandomPitch(Enums.E_Sound toPlay)
    {
        var foundData = soundData.Where(x => x.sound == toPlay).ToArray();
        if (foundData == null || foundData.Length == 0)
        {
            Debug.Log($"Sounds not found for #_{toPlay}");
            return;
        }
        var randomIndex = UnityEngine.Random.Range(0, foundData.Length);
        foundData[randomIndex].audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        foundData[randomIndex].audioSource.Play();
    }

    /// <summary>
    /// Looks up if a sound is playing from the category
    /// </summary>
    public bool IsPlaying(Enums.E_Sound toCheck)
    {
        var foundData = soundData.Where(x => x.sound == toCheck).ToArray();
        if (foundData == null || foundData.Length == 0)
        {
            Debug.Log($"Sounds not found for #_{toCheck}");
            return false;
        }
        return foundData.Where(x => x.audioSource.isPlaying).Any();
    }

    /// <summary>
    /// Sets parent to null, enables the update and delete the audio collection after playing target sound.
    /// </summary>
    public void PlayTargetSoundAndDestroyAudioCollection(Enums.E_Sound toPlay)
    {
        transform.SetParent(null, true);
        var foundData = soundData.Where(x => x.sound == toPlay).ToArray();
        if (foundData == null || foundData.Length == 0)
        {
            Debug.Log($"Sound not found for #_{toPlay}");
            return;
        }
        var randomIndex = UnityEngine.Random.Range(0, foundData.Length);
        _lastPlaying = foundData[randomIndex].audioSource;
        _lastPlaying.Play();
        enabled = true;
    }

    /// <summary>
    /// Looks up the current playing sound from target sounds and stops it.
    /// </summary>
    public void StopTargetSound(Enums.E_Sound toStop)
    {
        var foundData = soundData.Where(x => x.sound == toStop).ToArray();
        if (foundData == null || foundData.Length == 0)
        {
            Debug.Log($"Sounds not found for #_{toStop}");
            return;
        }
        var targetSound = foundData.FirstOrDefault(x => x.audioSource.isPlaying);
        if (targetSound == null)
        {
            Debug.Log($"Playing sound not found for #_{toStop}");
            return;
        }
        targetSound.audioSource.Stop();
    }

    /// <summary>
    /// Stops all target sounds from target sound.
    /// </summary>
    public void StopAllTargetSounds(Enums.E_Sound toStop)
    {
        var foundData = soundData.Where(x => x.sound == toStop).ToArray();
        if (foundData == null || foundData.Length == 0)
        {
            Debug.Log($"Sound not found for #_{toStop}");
            return;
        }
        foreach (var data in foundData)
        {
            data.audioSource.Stop();
        }
    }

    /// <summary>
    /// Set the pitch of the sound categorie
    /// </summary>
    public void SetPitchOfSound(Enums.E_Sound toPitch, float value)
    {
        var foundData = soundData.Where(x => x.sound == toPitch).ToArray();
        if (foundData == null || foundData.Length == 0)
        {
            Debug.Log($"Sound not found for #_{toPitch}");
            return;
        }
        foreach (var data in foundData)
        {
            data.audioSource.pitch = value;
        }
    }

    /// <summary>
    /// Get the current pitch of the current playing sound of the category
    /// </summary>
    public float GetPitchOfSound(Enums.E_Sound getPitch)
    {
        var foundData = soundData.Where(x => x.sound == getPitch).ToArray();
        if (foundData == null || foundData.Length == 0)
        {
            Debug.Log($"Sounds not found for #_{getPitch}");
            return 0f;
        }
        var targetSound = foundData.FirstOrDefault(x => x.audioSource.isPlaying);
        if (targetSound == null)
        {
            Debug.Log($"Playing sound not found for #_{getPitch}");
            return 0f;
        }
        return targetSound.audioSource.pitch;
    }

    [Serializable]
    public class AudioData
    {
        public Enums.E_Sound sound;
        public AudioSource audioSource;
    }
}
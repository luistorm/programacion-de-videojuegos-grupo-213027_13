using UnityEngine;
using TMPro;

/// <summary>
/// Monitors UI changes and game states to trigger audio events with volume control.
/// </summary>
public class AudioObserver : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource; 
    public AudioSource sfxSource;   

    [Header("Volume Settings")]
    [Range(0f, 1f)] 
    public float musicVolume = 0.2f; // Set to 20% by default for background music
    [Range(0f, 1f)] 
    public float sfxVolume = 1.0f;   // SFX remains at full volume for impact

    [Header("Music Clips")]
    public AudioClip introMusic;    

    [Header("SFX Clips")]
    public AudioClip countdownBeep; 
    public AudioClip launchSound;   

    [Header("UI References")]
    public TextMeshProUGUI timerText; 

    private string lastTextValue;
    private GameManager.GameState lastState;

    void Start()
    {
        // Apply initial volumes to the sources
        if (musicSource != null) musicSource.volume = musicVolume;
        if (sfxSource != null) sfxSource.volume = sfxVolume;

        if (GameManager.Instance != null)
        {
            lastState = GameManager.Instance.currentState;
            if (lastState == GameManager.GameState.Playing)
            {
                PlayMusic(introMusic, true);
            }
        }

        if (timerText != null) lastTextValue = timerText.text;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        GameManager.GameState currentState = GameManager.Instance.currentState;

        if (currentState != lastState)
        {
            HandleStateChange(currentState);
            lastState = currentState;
        }

        if (currentState == GameManager.GameState.Playing && timerText != null)
        {
            if (timerText.text != lastTextValue)
            {
                PlayTickSound();
                lastTextValue = timerText.text;
            }
        }
    }

    private void HandleStateChange(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.Playing:
                PlayMusic(introMusic, true);
                break;

            case GameManager.GameState.Launching:
                if (musicSource.isPlaying) musicSource.Stop();
                sfxSource.PlayOneShot(launchSound, sfxVolume);
                break;

            case GameManager.GameState.End:
                if (musicSource.isPlaying) musicSource.Stop();
                break;
        }
    }

    private void PlayTickSound()
    {
        if (countdownBeep != null && sfxSource != null)
        {
            // PlayOneShot uses the second parameter as a volume scale
            sfxSource.PlayOneShot(countdownBeep, sfxVolume);
        }
    }

    private void PlayMusic(AudioClip clip, bool loop)
    {
        if (clip == null || musicSource == null) return;
        
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume; // Force the volume to the specified value
        musicSource.Play();
    }
}
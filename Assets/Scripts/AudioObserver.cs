using UnityEngine;
using TMPro;

/// <summary>
/// Monitors game states and UI to trigger background music, 
/// countdown ticks, and engine loops during launch.
/// </summary>
public class AudioObserver : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource; 
    public AudioSource sfxSource;   

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.2f;
    [Range(0f, 1f)] public float sfxVolume = 1.0f;

    [Header("Music & Loops")]
    public AudioClip introMusic;    
    public AudioClip engineLoop;    // New clip for the continuous engine sound

    [Header("SFX Clips")]
    public AudioClip countdownBeep; 
    public AudioClip launchSound;   

    [Header("UI References")]
    public TextMeshProUGUI timerText; 

    private string lastTextValue;
    private GameManager.GameState lastState;

    void Start()
    {
        if (musicSource != null) musicSource.volume = musicVolume;
        if (sfxSource != null) sfxSource.volume = sfxVolume;

        if (GameManager.Instance != null)
        {
            lastState = GameManager.Instance.currentState;
            if (lastState == GameManager.GameState.Playing)
            {
                PlayLoopingAudio(introMusic);
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
                PlayLoopingAudio(introMusic);
                break;

            case GameManager.GameState.Launching:
                // 1. Stop intro music
                if (musicSource.isPlaying) musicSource.Stop();
                
                // 2. Play the ignition one-shot (explosion/start)
                if (launchSound != null) sfxSource.PlayOneShot(launchSound, sfxVolume);
                
                // 3. Start the continuous engine loop
                PlayLoopingAudio(engineLoop);
                break;

            case GameManager.GameState.Falling:
                // Stop the engine loop when rockets start falling
                if (musicSource.isPlaying) musicSource.Stop();
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
            sfxSource.PlayOneShot(countdownBeep, sfxVolume);
        }
    }

    private void PlayLoopingAudio(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;
        
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }
}
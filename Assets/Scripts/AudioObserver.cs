using UnityEngine;
using TMPro;

/// <summary>
/// Monitors game states and UI to trigger background music, 
/// countdown ticks, launch loops, and final game-over sounds.
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
    public AudioClip engineLoop;    

    [Header("SFX Clips")]
    public AudioClip countdownBeep; 
    public AudioClip launchSound;   
    public AudioClip victorySound;  // New clip for Team A Victory
    public AudioClip defeatSound;   // New clip for Team A Defeat
    public AudioClip tieSound;      // New clip for a Tie Match

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

        if (timerText != null && timerText.text != lastTextValue)
        {
            string currentText = timerText.text;
            // Simplified check: if countdown contains changes in seconds, play tick
            if (currentState == GameManager.GameState.Playing && currentText != lastTextValue)
            {
                PlayTickSound();
            }
            lastTextValue = currentText;
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
                if (musicSource.isPlaying) musicSource.Stop();
                if (launchSound != null) sfxSource.PlayOneShot(launchSound, sfxVolume);
                PlayLoopingAudio(engineLoop);
                break;

            case GameManager.GameState.Falling:
                if (musicSource.isPlaying) musicSource.Stop();
                break;

            case GameManager.GameState.End:
                if (musicSource.isPlaying) musicSource.Stop();
                
                // Triggers final audio based on game outcomes (Etapa 5 requirement)
                if (GameManager.Instance.scoreTeamA > GameManager.Instance.scoreTeamB)
                {
                    if (victorySound != null) sfxSource.PlayOneShot(victorySound, sfxVolume);
                }
                else if (GameManager.Instance.scoreTeamA < GameManager.Instance.scoreTeamB)
                {
                    if (defeatSound != null) sfxSource.PlayOneShot(defeatSound, sfxVolume);
                }
                else
                {
                    if (tieSound != null) sfxSource.PlayOneShot(tieSound, sfxVolume);
                }
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
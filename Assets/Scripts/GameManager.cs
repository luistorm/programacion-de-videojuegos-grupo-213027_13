using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the core game states, scores, timer, and the overall game loop.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public float gameDuration = 10f;
    private float timer;

    [Header("Team Scores")]
    public int scoreTeamA = 0;
    public int scoreTeamB = 0;
    
    [Header("UI References")]
    public TextMeshProUGUI infoText;
    
    [Header("Tracking Settings")]
    public int rocketsFinished = 0;
    public Transform rocketCenter;

    public enum GameState
    {
        Playing,
        Launching,
        Falling,
        End
    }

    [Header("Current Status")]
    public GameState currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        timer = gameDuration;
        currentState = GameState.Playing;
        Debug.Log("Game timer started.");
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.Playing:
                HandlePlayingState();
                break;

            case GameState.Launching:
                HandleLaunchingState();
                break;

            case GameState.Falling:
                HandleFallingState();
                break;

            case GameState.End:
                HandleEndState();
                break;
        }
    }

    private void HandlePlayingState()
    {
        infoText.text = $"Time: {Mathf.Max(0, Mathf.Ceil(timer))} seconds"; 
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            StartLaunch();
        }
    }

    private void HandleLaunchingState()
    {
        if (rocketCenter != null)
        {
            infoText.text = "Height: " + Mathf.Ceil(rocketCenter.position.y).ToString();
        }

        if (rocketsFinished >= 2)
        {
            StartFalling();
        }
    }

    private void HandleFallingState()
    {
        if (rocketsFinished >= 2)
        {
            EndGame();
        }
    }

    private void HandleEndState()
    {
        // Fulfills the Etapa 5 requirement: Restart mechanism using 'R' key
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void StartLaunch()
    {
        infoText.text = "Time's up!";
        currentState = GameState.Launching;
        rocketsFinished = 0;
        Debug.Log("Rockets launching!");
    }

    private void StartFalling()
    {
        currentState = GameState.Falling;
        rocketsFinished = 0;
        Debug.Log("Rockets falling down!");
    }

    private void EndGame()
    {
        currentState = GameState.End;
        
        // Comprehensive win, loss, and tie state check
        if (scoreTeamA > scoreTeamB)
        {
            infoText.text = "Team A Wins!\nPress 'R' to Restart";
            Debug.Log("Game Over: Team A won.");
        }
        else if (scoreTeamA < scoreTeamB)
        {
            infoText.text = "Team B Wins!\nPress 'R' to Restart";
            Debug.Log("Game Over: Team B won.");
        }
        else 
        {
            infoText.text = "It's a Tie!\nPress 'R' to Restart";
            Debug.Log("Game Over: Match ended in a tie.");
        }
    }

    private void RestartGame()
    {
        Debug.Log("Restarting scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameDuration = 10f;
    private float timer;

    public int scoreTeamA = 0;
    public int scoreTeamB = 0;
    public TextMeshProUGUI timerText;
    public int rocketsFinished = 0;
    public Transform rocketCenter;

    public enum GameState
    {
        Playing,
        Launching,
        Falling,
        End
    }

    public GameState currentState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timer = gameDuration;
        currentState = GameState.Playing;
        Debug.Log("Inicia el tiempo");
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            timerText.text = $"Tiempo: {Mathf.Max(0, Mathf.Ceil(timer))} segundos"; 
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StartLaunch();
            }
        }
        if (currentState == GameState.Launching)
        {
            timerText.text = "Altura: " + Mathf.Ceil(rocketCenter.position.y).ToString();
            if (rocketsFinished >= 2)
            {
                StartFalling();
            }
        }
        if (currentState == GameState.Falling)
        {
            if (rocketsFinished >= 2)
            {
                EndGame();
            }
        }
    }

    void EndGame()
    {
        currentState = GameState.End;
        if (scoreTeamA > scoreTeamB)
            Debug.Log("Gana equipo A");
        else
            Debug.Log("Gana equipo B");
    }

    void StartFalling()
    {
        currentState = GameState.Falling;
        Debug.Log("Falling!");
        rocketsFinished = 0;
    }

    void StartLaunch()
    {
        timerText.text = "¡Tiempo!"; 
        currentState = GameState.Launching;
        Debug.Log("Despegue!");
        rocketsFinished = 0;
    }
}
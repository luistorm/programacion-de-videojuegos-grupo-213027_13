using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameDuration = 10f;
    private float timer;

    public int scoreTeamA = 0;
    public int scoreTeamB = 0;
    public TextMeshProUGUI infoText;
    public int rocketsFinished = 0;
    public Transform rocketCenter;

    public enum GameState
    {
        Menu,
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
        currentState = GameState.Menu;
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            infoText.text = $"Tiempo: {Mathf.Max(0, Mathf.Ceil(timer))} segundos"; 
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StartLaunch();
            }
        }
        if (currentState == GameState.Launching)
        {
            infoText.text = "Altura: " + Mathf.Ceil(rocketCenter.position.y).ToString();
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
            infoText.text = $"¡Gana el equipo A!"; 
        else if (scoreTeamA < scoreTeamB)
            infoText.text = $"¡Gana el equipo B!"; 
        else 
            infoText.text = $"¡Empate!"; 
        StartCoroutine(CloseGame());
    }
    IEnumerator CloseGame()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("GAME CLOSED");

        Application.Quit();
    }

    void StartFalling()
    {
        currentState = GameState.Falling;
        Debug.Log("Falling!");
        rocketsFinished = 0;
    }

    void StartLaunch()
    {
        infoText.text = "¡Tiempo!"; 
        currentState = GameState.Launching;
        Debug.Log("Despegue!");
        rocketsFinished = 0;
    }
}
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameDuration = 10f;
    private float timer;

    public int scoreTeamA = 0;
    public int scoreTeamB = 0;

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
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StartLaunch();
            }
        }
    }

    void StartLaunch()
    {
        currentState = GameState.Launching;
        Debug.Log("Despegue!");
    }
}
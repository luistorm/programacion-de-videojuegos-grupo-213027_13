using UnityEngine;

/// <summary>
/// Controls individual rocket calculations, ascent during launch, and gravity handling during descent.
/// </summary>
public class Rocket : MonoBehaviour
{
    [Header("Team Identity")]
    public int team; // 0 = Team A, 1 = Team B

    [Header("Movement Settings")]
    public float heightMultiplier = 0.5f; 
    public float speed = 5f; 
    public float fallSpeed = 9.8f;
    public float rotationSpeed = 200f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private bool hasLaunched = false;
    private bool hasFinished = false;
    private bool hasLanded = false;
    private bool reportedLanding = false;

    void Start()
    {
        initialPosition = transform.position; 
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        
        var state = GameManager.Instance.currentState;

        // Fulfills the requirement to stop all physics and updates when the game ends
        if (state == GameManager.GameState.End)
        {
            return; 
        }

        if (state == GameManager.GameState.Launching)
        {
            if (!hasLaunched)
                Launch();

            if (!hasFinished)
                MoveUp();
        }
        else if (state == GameManager.GameState.Falling)
        {
            if (!hasLanded)
                Fall();
        }
    }

    void Fall()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Ground detection logic translated to English
        if (transform.position.y <= initialPosition.y) 
        {
            transform.position = initialPosition;
            transform.rotation = Quaternion.identity; // Reset rotation on land
            hasLanded = true;

            if (!reportedLanding)
            {
                reportedLanding = true;
                GameManager.Instance.rocketsFinished++;
            }
        }
    }

    void Launch()
    {
        hasLaunched = true;

        int score = (team == 0) 
            ? GameManager.Instance.scoreTeamA 
            : GameManager.Instance.scoreTeamB;

        float height = score * heightMultiplier;
        targetPosition = initialPosition + Vector3.up * height;
    }

    void MoveUp()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (!hasFinished && transform.position.y >= targetPosition.y - 0.01f)
        {
            transform.position = targetPosition;
            hasFinished = true;

            GameManager.Instance.rocketsFinished++;
        }
    }
}
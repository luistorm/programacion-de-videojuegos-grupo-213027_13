using UnityEngine;

public class Rocket : MonoBehaviour
{
    public int team; // 0 = A, 1 = B

    public float heightMultiplier = 0.5f; 
    public float speed = 5f; 

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private bool hasLaunched = false;
    private bool hasFinished = false;
    public float fallSpeed = 9.8f;
    public float rotationSpeed = 200f;

    private bool isFalling = false;
    private bool hasLanded = false;
    private bool reportedLanding = false;

    void Start()
    {
        initialPosition = transform.position; //M posición 
    }

    void Update()
    {
        var state = GameManager.Instance.currentState;

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

        if (transform.position.y <= initialPosition.y) // detectar suelo 
        {
            transform.position = initialPosition;
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
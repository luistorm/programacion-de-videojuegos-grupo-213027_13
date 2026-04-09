using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    [Header("Team Identity")]
    public bool isTeamA; 
    [Header("Physics & Ground")]
    public float gravity = 15.0f;
    public float groundHeight = 30.5f;

    [Header("Parachute Settings (Winner)")]
    public float parachuteSpeed = 5.0f;
    public float swayAmplitude = 0.8f;
    public float swayFrequency = 2.5f;

    private float currentVelocity = 0f;
    private float targetHeight;

    void Update()
    {
        if (GameManager.Instance == null) return;

        targetHeight = isTeamA ? GameManager.Instance.scoreTeamA : GameManager.Instance.scoreTeamB;

        switch (GameManager.Instance.currentState)
        {
            case GameManager.GameState.Launching:
                PerformLaunch();
                break;

            case GameManager.GameState.Falling:
                PerformDescent();
                break;
        }
    }

    void PerformLaunch()
    {
        if (transform.position.y < targetHeight)
        {
            transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
        }
    }

    void PerformDescent()
    {
        if (transform.position.y > groundHeight)
        {
            if (CheckIfWinner())
            {
                ApplyParachuteEffect();
            }
            else
            {
                ApplyFreeFall();
            }
        }
        else
        {
            currentVelocity = 0f;
            transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
        }
    }

    bool CheckIfWinner()
    {
        if (isTeamA) return GameManager.Instance.scoreTeamA > GameManager.Instance.scoreTeamB;
        else return GameManager.Instance.scoreTeamB > GameManager.Instance.scoreTeamA;
    }

    void ApplyFreeFall()
    {
        currentVelocity += gravity * Time.deltaTime;
        transform.position += Vector3.down * currentVelocity * Time.deltaTime;
    }

    void ApplyParachuteEffect()
    {
        float sway = Mathf.Sin(Time.time * swayFrequency) * swayAmplitude;
        Vector3 descent = (Vector3.down * parachuteSpeed) + (Vector3.right * sway);
        
        transform.position += descent * Time.deltaTime;
    }
}
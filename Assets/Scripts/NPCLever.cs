using UnityEngine;

public class NPCLever : MonoBehaviour
{
    public float interval = 2f;
    public int team; // 0 = A, 1 = B
    private float timer;

    private Vector3 initialPosition;
    private bool isMoving = false;

    public float moveAmount = 1f;
    public float speed = 5f;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0;
            isMoving = true;
            if (team == 0)
                GameManager.Instance.scoreTeamA++;
            else 
                GameManager.Instance.scoreTeamB++;
        }

        Animate();
    }

    void Animate()
    {
        Vector3 target = initialPosition;

        if (isMoving)
            target += Vector3.up * moveAmount;

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            isMoving = false;
        }
    }
}
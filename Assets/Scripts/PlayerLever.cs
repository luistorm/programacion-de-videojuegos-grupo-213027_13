using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLever : MonoBehaviour
{
    public float moveAmount = 1f;
    public float speed = 5f;

    private Vector3 initialPosition;
    private bool isMoving = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isMoving = true;
            GameManager.Instance.scoreTeamA++;
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
            isMoving = false;
    }
}
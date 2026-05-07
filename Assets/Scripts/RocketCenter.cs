using UnityEngine;

public class RocketCenter : MonoBehaviour
{
    public Transform rocketA;
    public Transform rocketB;

    private float maxY;
    private GameManager.GameState lastState;

    void Update()
    {
        var currentState = GameManager.Instance.currentState;

        float centerX = (rocketA.position.x + rocketB.position.x) / 2f;
        float centerZ = (rocketA.position.z + rocketB.position.z) / 2f;

        float highestY = Mathf.Max(
            rocketA.position.y,
            rocketB.position.y
        );

        if (currentState != lastState)
        {
            if (currentState == GameManager.GameState.Falling)
            {
                maxY = highestY;
            }
        }

        if (currentState == GameManager.GameState.Launching)
        {
            if (highestY > maxY)
                maxY = highestY;
        }
        else if (currentState == GameManager.GameState.Falling)
        {
            maxY = highestY;
        }

        transform.position = new Vector3(centerX, maxY, centerZ);

        lastState = currentState;
    }
}
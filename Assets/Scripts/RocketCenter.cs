using UnityEngine;

public class RocketCenter : MonoBehaviour
{
    public Transform rocketA;
    public Transform rocketB;

    private float maxY;

    void Update()
    {
        float centerX = (rocketA.position.x + rocketB.position.x) / 2f;
        float centerZ = (rocketA.position.z + rocketB.position.z) / 2f;

        float highestY = Mathf.Max(
            rocketA.position.y,
            rocketB.position.y
        );

        if (GameManager.Instance.currentState == GameManager.GameState.Launching)
        {
            if (highestY > maxY)
                maxY = highestY;
        }
        else if (GameManager.Instance.currentState == GameManager.GameState.Falling)
        {
            maxY = highestY;
        }

        transform.position = new Vector3(centerX, maxY, centerZ);
    }
}
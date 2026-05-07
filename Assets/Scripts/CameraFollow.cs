using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 3f;

    void LateUpdate()
    {
        var state = GameManager.Instance.currentState;

        if (state != GameManager.GameState.Launching &&
            state != GameManager.GameState.Falling)
            return;

        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            Time.deltaTime * smoothSpeed
        );
    }
}
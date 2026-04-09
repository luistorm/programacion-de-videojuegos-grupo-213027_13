using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Configuration")]
    public Transform rocketA;
    public Transform rocketB;

    [Header("Movement Settings")]
    public float smoothSpeed = 5.0f;
    public float heightOffset = 15.0f; 

    void LateUpdate()
    {
        if (rocketA == null || rocketB == null) return;

        float maxAltitude = Mathf.Max(rocketA.position.y, rocketB.position.y);
        
        Vector3 targetPosition = new Vector3(transform.position.x, maxAltitude + heightOffset, transform.position.z);
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
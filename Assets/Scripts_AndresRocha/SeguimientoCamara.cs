using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    public Transform coheteEquipoA;
    public Transform coheteEquipoB;
    public float suavizado = 5.0f;
    public float offsetAltura = 15.0f; 

    void LateUpdate()
    {
        // Ganador
        float alturaMaxima = Mathf.Max(coheteEquipoA.position.y, coheteEquipoB.position.y);
        
        // eje Y
        Vector3 posicionDeseada = new Vector3(transform.position.x, alturaMaxima + offsetAltura, transform.position.z);
        
        // Smooth
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
    }
}
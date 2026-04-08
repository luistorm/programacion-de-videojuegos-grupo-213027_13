using UnityEngine;

public class LogicaVueloCohete : MonoBehaviour
{
    [Header("Mecánica 2.3 - Velocidad, peso Cohetes y caida")]
    public float alturaSuelo = 30.5f;
    public float limiteAltitud = 400.0f; // limite prueba
    public float gravedad = 15.0f;
    public float velocidadCaidaNormal = 15.0f;

    [Header("Ajustes de Ganador (Paracaídas)")]
    public bool soyGanador = false; 
    public float velocidadConParacaidas = 5.0f;
    public float amplitudBalanceo = 0.8f; 
    public float frecuenciaBalanceo = 2.5f;

    private float velocidadActual = 0f;
    private bool paracaidasAbierto = false;

    void Update()
    {
        // 1. CAIDA 
        if (transform.position.y > alturaSuelo)
        {
            float descenso;

            // Lógica de paracaídas para el ganador
            if (soyGanador && paracaidasAbierto)
            {
                descenso = velocidadConParacaidas;
            }
            else
            {
                // gravedad
                velocidadActual += gravedad * Time.deltaTime;
                descenso = velocidadActual;
            }

            Vector3 movimiento = Vector3.down * descenso * Time.deltaTime;

            // Efecto paracaídas
            if (soyGanador && paracaidasAbierto)
            {
                float balanceo = Mathf.Sin(Time.time * frecuenciaBalanceo) * amplitudBalanceo;
                movimiento += Vector3.right * balanceo * Time.deltaTime;
            }

            transform.Translate(movimiento);
        }
        else
        {
            // Suelo
            velocidadActual = 0f;
            paracaidasAbierto = false;
            transform.position = new Vector3(transform.position.x, alturaSuelo, transform.position.z);
        }

        // 2. LIMITE
        if (transform.position.y > limiteAltitud)
        {
            transform.position = new Vector3(transform.position.x, limiteAltitud, transform.position.z);
            velocidadActual = 0f; // Freno
            paracaidasAbierto = true; // Activa el comportamiento de descenso lento
            Debug.Log(gameObject.name + " LLEGÓ A LA META");
        }

        // PRUEBA
        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position += Vector3.up * 50f;
            velocidadActual = 0f; // Reiniciar velocidad 
            paracaidasAbierto = false;
            Debug.Log("Impulso de prueba: " + transform.position.y + " yardas");
        }
    }
}

using UnityEngine;

public class LogicaVueloCohete : MonoBehaviour
{
    [Header("Configuración de Vuelo")]
    public float velocidadCaida = 1.5f;
    public float limiteAltitud = 400.0f; 
    public float fuerzaImpulso = 2.0f;
    public float alturaSuelo = 0.6f;    

    [Header("Referencia al Personaje")]
    public Animator animadorPersonaje; 

    void Update()
    {
       
        if (transform.position.y > alturaSuelo)
        {
            transform.Translate(Vector3.down * velocidadCaida * Time.deltaTime);
        }

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SubirYAnimar();
        }

        
        if (transform.position.y > limiteAltitud)
        {
            transform.position = new Vector3(transform.position.x, limiteAltitud, transform.position.z);
        }
    }

    void SubirYAnimar()
    {
        
        transform.Translate(Vector3.up * fuerzaImpulso);

        
        if (animadorPersonaje != null)
        {
            animadorPersonaje.SetTrigger("Pump");
        }
    }
}
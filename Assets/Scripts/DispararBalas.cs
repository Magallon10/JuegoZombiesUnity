using UnityEngine;

public class DispararBalas : MonoBehaviour
{
[Header("Referencias")]
    public Camera camaraFPS;              // La cámara principal del jugador
    public Transform puntoDeDisparo;      // Objeto vacío en la boca del cañón

    [Header("Parámetros")]
    public float rangoMaximo = 100f;     // Rango máximo para la puntería
    public float rangoDeDisparo = 150f;  // Rango máximo para el proyectil


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Disparar();
        }
    }

    void Disparar()
    {
        RaycastHit hitPunteria;
        Vector3 objetivo; // El punto final al que debe apuntar el disparo

        // 1. Raycast de Puntería (Desde el centro de la cámara)
        if (Physics.Raycast(camaraFPS.transform.position, camaraFPS.transform.forward, out hitPunteria, rangoMaximo))
        {
            // El jugador está apuntando a un objeto.
            // Establece el objetivo como el punto de impacto.
            objetivo = hitPunteria.point;
        }
        else
        {
            // El jugador no está apuntando a un objeto dentro del rango (al aire).
            // Establece el objetivo como un punto lejano en la dirección de la cámara.
            objetivo = camaraFPS.transform.position + camaraFPS.transform.forward * rangoMaximo;
        }

        // --- Lógica del Disparo Real ---

        RaycastHit hitDisparo;
        Vector3 direccionDisparo = (objetivo - puntoDeDisparo.position).normalized;

        // 2. Raycast de Disparo (Desde la boca del cañón hacia el objetivo)
        // La distancia será la distancia entre el arma y el objetivo, o el rango de disparo, el que sea menor.
        float distancia = Vector3.Distance(puntoDeDisparo.position, objetivo);
        distancia = Mathf.Min(distancia, rangoDeDisparo);


        if (Physics.Raycast(puntoDeDisparo.position, direccionDisparo, out hitDisparo, distancia))
        {
            // El disparo ha impactado con un Collider
            Debug.Log("Disparo impactó en: " + hitDisparo.collider.gameObject.name);
            
            // Puedes usar hitDisparo.point y hitDisparo.normal para efectos y decals.
            // hitDisparo.transform.GetComponent<Daño>().RecibirDaño(25); 
        }
        else
        {
            // El disparo falló o se fue al infinito (aunque el rayo de puntería haya golpeado)
            Debug.Log("El disparo salió del arma pero no golpeó nada.");
        }
    }
}

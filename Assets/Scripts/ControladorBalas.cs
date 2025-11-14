using UnityEngine;

public class ControladorBalas : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float alcance = 100f;         // distancia máxima
    public float daño = 10f;             // daño que causa
    public LayerMask capasImpacto;       // qué capas puede golpear
    public Transform origenDisparo;      // punto desde donde se lanza (por ejemplo, el cañón del arma)


    void Start()
    {
        origenDisparo = gameObject.transform;
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Disparar();
        }
    }

    void Disparar()
    {
        RaycastHit hit;

        if (Physics.Raycast(origenDisparo.position, origenDisparo.forward, out hit, alcance, capasImpacto))
        {
            Debug.Log("Impactó en: " + hit.collider.name);

            // Si el objeto tiene salud, podrías hacer algo como:
            // hit.collider.GetComponent<Enemigo>()?.RecibirDaño(daño);
        }

        // (Opcional) crear un efecto visual de bala o impacto aquí
    }
}

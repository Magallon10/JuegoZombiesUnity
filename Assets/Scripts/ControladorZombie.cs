using UnityEngine;

public class ControladorZombie : MonoBehaviour
{

    public int vida;
    public int ronda;
    public GameObject jugador;
    private Transform jugadorTransform;
    public float rangoAtaque = 1.2f;
    float tiempoEntreAtaques = 2f;
    private float tiempoSiguienteAtaque;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float vidaBase = 100f;     
        float factorCrecimiento = 0.07f;
        float vida = vidaBase * Mathf.Pow(1f + factorCrecimiento, ronda - 1);
        tiempoSiguienteAtaque = Time.time;
        jugadorTransform = jugador.transform;
    }

    // Update is called once per frame
    void Update()
    {

        float distancia = Vector3.Distance(transform.position, jugadorTransform.position);

        if (distancia <= rangoAtaque && Time.time >= tiempoSiguienteAtaque)
        {
            AtacarJugador();
        }
    }

    void AtacarJugador()
    {
        jugador.GetComponent<ControladorPlayer>().HerirPlayer();

        tiempoSiguienteAtaque = Time.time + tiempoEntreAtaques;
    }
    public void HacerDaño(int daño)
    {
        vida -= daño;
        if (vida <= 0)
        {
            Morir();
        }
    }
    
    public void Morir()
    {
        Destroy(gameObject);
        jugador.GetComponent<ControladorPlayer>().ZombieMuerto();
    }
}

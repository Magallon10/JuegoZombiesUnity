using TMPro;
using UnityEngine;

public class ControladorPlayer : MonoBehaviour
{
    [Header("Referencias")]
    public Camera camaraFPS;              // La cámara principal del jugador
    public Transform puntoDeDisparo;      // Objeto vacío en la boca del cañón

    [Header("Parámetros")]
    public float rangoMaximo = 100f;     // Rango máximo para la puntería
    public float rangoDeDisparo = 150f;  // Rango máximo para el proyectil

    private ControladorZombie controladorZombie;



    private CharacterController controller;
    public float jumpSpeed = 8.0F;
    public float speed = 6.0f;
    public float gravity = 20.0F;
    private int vida = 100;
    private int puntos = 0;
    public TMP_Text textoVida;
    public TMP_Text textoPuntos;
    public TMP_Text textoGameOver;
    public Canvas hud;
    public Canvas gameOver;
    private Vector3 moveDirection = Vector3.zero;
    private int contadorBajas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gameOver.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        //Visualizar vida y puntos
        textoVida.text = "Vida: " + vida;
        textoPuntos.text = "Puntos: " + puntos;


        //Disparar si hace click
        if (Input.GetButtonDown("Fire1"))
        {
            Disparar();
        }

        //Esprintar
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 10f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 6f;
        }

        // Movimiento horizontal siempre
        Vector3 horizontalMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        horizontalMove = transform.TransformDirection(horizontalMove);
        horizontalMove *= speed;


        // Solo salto si está en el suelo
        if (controller.isGrounded)
        {
            moveDirection.y = 0; // Reiniciar velocidad vertical cuando está en el suelo

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Aplicar movimiento horizontal al moveDirection manteniendo el valor vertical actual
        moveDirection.x = horizontalMove.x;
        moveDirection.z = horizontalMove.z;

        // Aplicar gravedad
        moveDirection.y -= gravity * Time.deltaTime;

        // Mover el personaje
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void HerirPlayer()
    {
        vida -= 40;

        if (vida < 0)
        {
            GameOver();
        }
    }
            
    public void ZombieMuerto()
    {
        contadorBajas++;
    }
    void GameOver()
    {
        textoGameOver.text = "Zombies matados: " + contadorBajas;
        hud.enabled = false;
        gameOver.enabled = true;
        
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
            GameObject objetoImpactado = hitDisparo.collider.gameObject;
            // El disparo ha impactado con un Collider
            Debug.Log("Disparo impactó en: " + objetoImpactado.name);

             


            // Verificamos si pertenece a un zombie
            if (objetoImpactado.CompareTag("Enemigo"))
            {
                if (objetoImpactado.name == "Bip001")
                {
                  
                    puntos += 300;
                    objetoImpactado.GetComponentInParent<ControladorZombie>().HacerDaño(50);

                }
                else if (objetoImpactado.name == "Body_01_tanktop")
                {
                   
                    puntos += 100;
                    objetoImpactado.GetComponentInParent<ControladorZombie>().HacerDaño(25);

                }
            }

            // Puedes usar hitDisparo.point y hitDisparo.normal para efectos y decals.
        }
        else
        {
            Debug.Log("El disparo salió del arma pero no golpeó nada.");
        }
    }
}

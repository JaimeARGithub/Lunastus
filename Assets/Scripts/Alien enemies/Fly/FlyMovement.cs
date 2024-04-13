using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    private GameObject hunter;
    private Rigidbody2D rb;
    private Fly f;

    // Variables para el movimiento de patrulla y persecución
    private float distance;
    public float rightX;
    public float leftX;
    [SerializeField] private bool movingToEnd;
    private float patrolSpeed = 4f;
    private float chaseSpeed = 2f;
    [SerializeField] private bool mirandoDerecha = false;
    private bool playerDetected = false;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        f = GetComponent<Fly>();

        // Lógica para el movimiento
        movingToEnd = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!f.isDead())
        {
            // Mientras la mosca vive, se evalúa la distancia con el hunter
            distance = Vector2.Distance(transform.position, hunter.transform.position);

            // Si es superior a X, realiza patrol
            // Si no, persigue al hunter
            if (distance > 6)
            {
                // Lógica de patrol
                // Si no se ha detectado al jugador, patrulla normal
                // Si sí, se queda estático (para el caso de que el jugador haya entrado en el
                // rango de detección y después salido de él)

                // Una posibilidad sería que, si se ha iniciado una persecución y el jugador ha salido
                // del rango de detección, el enemigo volviera a su patrol; no obstante, se opta
                // por dejarlo estático en donde se haya quedado, a la espera de una nueva detección
                // del jugador
                if (!playerDetected)
                {
                    if (movingToEnd)
                    {
                        rb.velocity = new Vector2(-1 * patrolSpeed, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(1 * patrolSpeed, rb.velocity.y);
                    }


                    if (transform.position.x < leftX && !mirandoDerecha)
                    {
                        movingToEnd = false;
                        Girar();
                    }
                    if (transform.position.x > rightX && mirandoDerecha)
                    {
                        movingToEnd = true;
                        Girar();
                    }
                } else
                {
                    // Caso para cuando el jugador ya haya sido detectado pero se haya
                    // salido del rango de detección; puesto que hacerlo volver a la patrol
                    // no es viable y también es dudoso que suceda por las velocidades de
                    // persecución, pero por si sucede, que se quede estático
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }


            } else
            {
                // Lógica de persecución
                playerDetected = true;
                Vector2 direction = (hunter.transform.position - transform.position);
                rb.velocity = direction * chaseSpeed;


                if (hunter.transform.position.x <= transform.position.x && mirandoDerecha)
                {
                    Girar();
                } else if (hunter.transform.position.x > transform.position.x && !mirandoDerecha)
                {
                    Girar();
                }
            }
        }
    }


    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        transform.Rotate(0f, 180f, 0f);
    }
}

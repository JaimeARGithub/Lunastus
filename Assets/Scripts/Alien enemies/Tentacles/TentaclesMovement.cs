using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclesMovement : MonoBehaviour
{
    private GameObject hunter;
    private Rigidbody2D rb;
    private Tentacles t;

    // Variables para el movimiento de patrulla y persecución
    // Persecución:
    private bool mirandoDerecha = true;
    private float distance;
    private bool playerDetected = false;
    private float speed = 0.5f;

    // Patrulla:
    public float rightX;
    public float leftX;
    [SerializeField] private bool movingToEnd;



    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        t = GetComponent<Tentacles>();

        movingToEnd = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Lógica del Tentacles: se ejecuta mientras está vivo
        // Si el cazador no ha sido detectado, se ejecuta una patrol de lado a lado
        // En el instante en que se detecta al cazador, el Tentacles se mueve hacia él, pero nunca lo toca;
        // siempre mantiene un mínimo de distancia con el cazador. Una vez el cazador es detectado, es
        // perseguido constantemente.
        if (!t.isDead())
        {
            distance = Vector2.Distance(transform.position, hunter.transform.position);
            if (distance < 6)
            {
                playerDetected = true;
            }


            if (playerDetected)
            {
                // Si el jugador ha sido detectado, aparte de perseguirlo, giramos el sprite
                // dependiendo de dónde esté el jugador y hacia dónde mire el enemigo
                if (hunter.transform.position.x <= transform.position.x && mirandoDerecha)
                {
                    Girar();
                } else if (hunter.transform.position.x > transform.position.x && !mirandoDerecha)
                {
                    Girar();
                }


                // El tentacles se mueve hacia el jugador hasta que la distancia sea de 1.5
                // Si la distancia alcanza 1.5, el tentacles se detiene
                if (distance > 1.5)
                {
                    Vector2 direction = (hunter.transform.position - transform.position);
                    rb.velocity = direction * speed;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }


            } else
            {
                // Si el cazador no ha sido detectado: movimiento de patrulla de lado a lado
                if (movingToEnd)
                {
                    rb.velocity = new Vector2(1 * speed, rb.velocity.y);
                } else
                {
                    rb.velocity = new Vector2(-1 * speed, rb.velocity.y);
                }


                // Si sobrepasa la X izquierda y estaba mirando a la izquierda: se gira e inicia recorrido de vuelta
                // Si sobrepasa la X derecha y estaba mirando a la derecha: se gira e inicia recorrido de ida
                if (transform.position.x < leftX && !mirandoDerecha)
                {
                    movingToEnd = true;
                    Girar();
                }
                if (transform.position.x > rightX && mirandoDerecha)
                {
                    movingToEnd = false;
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

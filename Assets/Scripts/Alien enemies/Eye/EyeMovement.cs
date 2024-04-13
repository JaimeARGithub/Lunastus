using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    private GameObject hunter;
    private Rigidbody2D rb;
    private Eye e;

    // Variables para la persecución
    [SerializeField] private float distance;
    private float chaseSpeed = 0.5f;
    [SerializeField] private bool mirandoDerecha = false;
    private bool playerDetected = false;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        e = GetComponent<Eye>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!e.isDead())
        {
            // Mientras el Eye vive, se evalúa la distancia con el cazador.
            // En el instante en que ésta sea menor de 6, cazador detectado.
            // Si el cazador no ha sido detectado, el Eye permanece estático.
            // Si el cazador ha sido detectado, el Eye lo persigue constantemente, deteniéndose a cierta distancia.
            distance = Vector2.Distance(transform.position, hunter.transform.position);
            if (distance < 6)
            {
                playerDetected = true;
            }

            if (playerDetected)
            {
                if (hunter.transform.position.x <= transform.position.x && mirandoDerecha)
                {
                    Girar();
                }
                else if (hunter.transform.position.x > transform.position.x && !mirandoDerecha)
                {
                    Girar();
                }

                if (distance > 1.5)
                {
                    Vector2 direction = (hunter.transform.position - transform.position);
                    rb.velocity = direction * chaseSpeed;
                } else
                {
                    rb.velocity = Vector2.zero;
                }

            } else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }


    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        transform.Rotate(0f, 180f, 0f);
    }
}

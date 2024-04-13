using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinofishMovement : MonoBehaviour
{
    private GameObject hunter;
    private Rigidbody2D rb;
    private Animator animator;
    private Rinofish f;

    // Variables para la persecución
    [SerializeField] private bool mirandoDerecha = true;
    private bool playerDetected = false;
    private float distance;
    private float chaseSpeed = 12f;

    // Variables para el efecto de frenada durante la persecución
    private float leftTurnInstant = 0f;
    private float rightTurnInstant = 0f;
    private float turningTime = 1f;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        f = GetComponent<Rinofish>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!f.isDead())
        {
            // Mientras el Rinofish vive, se evalúa su distancia con el hunter.
            // Si el jugador no ha sido detectado, el Rinofish no hace nada.
            // En el instante en que es detectado, la persecución no para en ningún momento.
            distance = Vector2.Distance(transform.position, hunter.transform.position);
            if (distance < 6)
            {
                playerDetected = true;
            }

            if (playerDetected)
            {
                // Control de giro del sprite según la posición del jugador con respecto al Rinofish
                animator.SetBool("isRolling", true);

                if (hunter.transform.position.x <= transform.position.x && mirandoDerecha)
                {
                    Girar();
                    leftTurnInstant = Time.time;
                }
                else if (hunter.transform.position.x > transform.position.x && !mirandoDerecha)
                {
                    Girar();
                    rightTurnInstant = Time.time;
                }

                // Control de movimiento; tras girarse por haber llegado a la posición del cazador,
                // aún tiene que permanecer rodando durante unos instantes
                if ((hunter.transform.position.x <= transform.position.x) && Time.time - leftTurnInstant > turningTime)
                {
                    rb.velocity = new Vector2(-1 * chaseSpeed, rb.velocity.y);

                } else if ((hunter.transform.position.x > transform.position.x) && Time.time - rightTurnInstant > turningTime)
                {
                    rb.velocity = new Vector2(1 * chaseSpeed, rb.velocity.y);
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

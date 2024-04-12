using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiceraptorMovement : MonoBehaviour
{
    [SerializeField] private bool mirandoDerecha = false;
    private GameObject hunter;
    private Rigidbody2D rb;
    private Animator animator;
    private Biceraptor b;
    private float speed = 8f;
    [SerializeField] private bool fleeing = false;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        b = GetComponent<Biceraptor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!b.isDead())
        {
            // El biceraptor solamente se mueve cuando ha sido dañado.
            // Si ha sido dañado, se inicia huida en dirección contraria al jugador durante un tiempo.
            // Si no, permanece estático.

            if (b.isDamaged())
            {
                fleeing = true;
                // En el propio script de vida se indica el tiempo que el biceraptor se marca como
                // dañado, y por tanto, el tiempo durante el que debe huir.
                // Mientras esté dañado, huye.
                // Por cada vez que es dañado, huye en dirección contraria al cazador.

                // Parte de animación: mostrarlo como caminando, y girarlo dependiendo de dónde se encuentre el jugador
                animator.SetBool("isWalking", true);
                if (hunter.transform.position.x <= transform.position.x && !mirandoDerecha)
                {
                    Girar();
                } else if (hunter.transform.position.x > transform.position.x && mirandoDerecha)
                {
                    Girar();
                }

                // Parte de física: mientras esté marcado como dañado, huye en dirección contraria al jugador
                if (hunter.transform.position.x <= transform.position.x)
                {
                    rb.velocity = new Vector2(1*speed, rb.velocity.y);
                } else if (hunter.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-1 * speed, rb.velocity.y);
                }

                


            } else
            {
                fleeing = false;
                animator.SetBool("isWalking", false);
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

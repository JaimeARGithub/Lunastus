using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMovement : MonoBehaviour
{
    [SerializeField] private bool mirandoDerecha = false;
    private GameObject hunter;
    private Rigidbody2D rb;
    private Animator animator;
    private Shell s;

    // Variables para el movimiento de patrulla
    public Vector2 initialPosition;
    public Vector2 finalPosition;
    [SerializeField] private bool movingToEnd;
    private float speed = 1f;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        s = GetComponent<Shell>();

        // Lógica para el movimiento
        movingToEnd = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!s.isDead())
        {
            // Mientras el shell no esté muerto, se evalúa la distancia con el hunter.
            // Si es mayor que X, realiza la patrol.
            // Si no, se detiene y se esconde.
            float distance = Vector2.Distance(transform.position, hunter.transform.position);

            if (distance > 5)
            {
                animator.SetBool("isCovering", false);

                if (movingToEnd)
                {
                    rb.velocity = new Vector2(-1*speed, rb.velocity.y);
                } else
                {
                    rb.velocity = new Vector2(1*speed, rb.velocity.y);
                }


                // Para poder usar Vector2 en lugar de Vector3, se mira que coincida la coordenada
                // x de la position del transform con la coordenada x de las posiciones designadas
                // como inicial y final
                if (Vector2.Distance(transform.position, finalPosition) < 0.1f)
                {
                    movingToEnd = false;
                    if (!mirandoDerecha)
                    {
                        Girar();
                    }
                }
                if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
                {
                    movingToEnd = true;
                    if (mirandoDerecha)
                    {
                        Girar();
                    }
                }


            } else
            {
                animator.SetBool("isCovering", true);
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

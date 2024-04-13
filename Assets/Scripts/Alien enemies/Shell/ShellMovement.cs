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
    public float rightX;
    public float leftX;
    [SerializeField] private bool movingToEnd;
    private float speed = 1f;
    private float distance;


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
            distance = Vector2.Distance(transform.position, hunter.transform.position);

            if (distance > 5)
            {
                animator.SetBool("isCovering", false);

                // Dependiendo de la dirección, se aplica fuerza en una dirección u otra.
                if (movingToEnd)
                {
                    rb.velocity = new Vector2(-1*speed, rb.velocity.y);
                } else
                {
                    rb.velocity = new Vector2(1*speed, rb.velocity.y);
                }



                // Para poder usar únicamente coordenadas X en lugar de vectores completos,
                // se van a utilizar la posición del transform y los puntos de referencia izquierda
                // y derecha.
                // En el instante en que el shell supere (xShell < xIzq) la X izquierda, gira y marcha en sentido contrario.
                // En el instante en que el shell supere (xShell > xDrch) la X derecha, gira y marcha en sentido contrario.
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

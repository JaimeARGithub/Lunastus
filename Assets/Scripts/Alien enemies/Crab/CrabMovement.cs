using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMovement : MonoBehaviour
{
    [SerializeField] private bool mirandoDerecha = false;
    private GameObject hunter;
    private Animator animator;
    private Rigidbody2D rb;
    private float speed = 3.75f;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Se mira la distancia entre ambos
        float distance = Vector2.Distance(transform.position, hunter.transform.position);

        // Si es menor que 5: huida
        if (distance < 8)
        {
            animator.SetBool("isWalking", true);

            // Girar el sprite según posición del cazador respecto al cangrejo
            if (hunter.transform.position.x < transform.position.x && !mirandoDerecha)
            {
                Girar();
            } else if (hunter.transform.position.x > transform.position.x && mirandoDerecha)
            {
                Girar();
            }

            // Dar movimiento según posición del cazador respecto al cangrejo
            if (hunter.transform.position.x < transform.position.x)
            {
                rb.velocity = new Vector2(1*speed, rb.velocity.y);
            } else if (hunter.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(-1*speed, rb.velocity.y);
            }

        } else
        {
            // Si es mayor que 5: retorno a la estaticidad, deja de correr, cesa el movimiento y el sprite queda mirando a izquierda

            animator.SetBool("isWalking", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
            if (mirandoDerecha)
            {
                Girar();
            }
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        transform.Rotate(0f, 180f, 0f);
    }
}

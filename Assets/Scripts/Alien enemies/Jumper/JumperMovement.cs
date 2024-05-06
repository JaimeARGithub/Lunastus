using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperMovement : MonoBehaviour
{
    private GameObject hunter;
    private Animator animator;
    private Rigidbody2D rb;
    private Jumper j;

    // Para la l�gica de saltos
    public Collider2D col;
    [SerializeField] private bool isGrounded;
    public LayerMask groundLayer;
    [SerializeField] private bool isOnEnemy;
    public LayerMask enemiesLayer;

    // Para movimiento en general
    private float distance;
    private float horizontalSpeed = 3.75f;
    private float verticalSpeed = 15f;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        j = GetComponent<Jumper>();
    }

    // Update is called once per frame
    void Update()
    {
        // Siempre y cuando el jumper no est� muerto:
        if (!j.isDead())
        {
            // Cada instante se eval�an la distancia entre jumper y cazador y si el jumper est� tocando el suelo
            distance = Vector2.Distance(transform.position, hunter.transform.position);
            isGrounded = Physics2D.IsTouchingLayers(col, groundLayer);
            isOnEnemy = Physics2D.IsTouchingLayers(col, enemiesLayer);



            // Si el jumper est� tocando el suelo y la distancia es menor que 6: salto
            // Salto hacia un lado o a otro dependiendo de la posici�n del hunter
            if ((isGrounded || isOnEnemy) && distance < 4.5)
            {
                if (hunter.transform.position.x <= transform.position.x)
                {
                    rb.velocity = new Vector2(1 * horizontalSpeed, verticalSpeed);
                } else if (hunter.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-1 * horizontalSpeed, verticalSpeed);
                }
            }


            // Si est� saltando: animaci�n de salto
            // Si no: animaci�n idle
            if (!isGrounded && !isOnEnemy)
            {
                animator.SetBool("isJumping", true);
            } else
            {
                animator.SetBool("isJumping", false);
            }
        }
    }
}

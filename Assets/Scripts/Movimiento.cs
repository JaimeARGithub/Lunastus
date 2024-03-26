using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    //Movimiento del jugador
    private float velocidad = 4.5F;
    Rigidbody2D rb2d;
    SpriteRenderer spRd;


    //Salto de jugador
    private bool isGrounded = false;
    private bool quiereSaltar = false;
    private float potenciaSalto = 150F;
    public LayerMask groundLayer;

    // Collider de los pies
    public Collider2D piesCollider;

    // Gravedad personalizada
    private float gravedadPersonalizada = 100f;

    // Variables para el doble salto
    private bool dobleSaltoDesbloqueado = true; // VARIABLE QUE DEPENDE DEL GAME MANAGER, CAMBIARLO PARA QUE LEA EL VALOR DE ÉL
    private bool dobleSalto = true;


    //Para la utilizacion del Animator del jugador
    private Animator animator;
    public bool dir;


    // Para el movimiento; izq-drch
    private float movimientoH;



    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spRd = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movimientoH = Input.GetAxisRaw("Horizontal");
        rb2d.velocity = new Vector2(movimientoH * velocidad, rb2d.velocity.y);


        if (movimientoH > 0)
        {
            dir = false;
            spRd.flipX = false;
        }
        else if (movimientoH < 0)
        {
            dir = true;
            spRd.flipX = true;
        }


        if (movimientoH != 0)
        {
            animator.SetBool("isRunning", true);
        }
        if (movimientoH == 0)
        {
            animator.SetBool("isRunning", false);
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            quiereSaltar = true;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.IsTouchingLayers(piesCollider, groundLayer);


        if (quiereSaltar)
        {
            if (isGrounded || (dobleSalto && dobleSaltoDesbloqueado))
            {

                rb2d.AddForce(Vector2.up * potenciaSalto, (ForceMode2D)ForceMode.VelocityChange);

                if (!isGrounded && dobleSalto)
                {

                    dobleSalto = false;
                }
            }


            quiereSaltar = false;
        }


        if (!isGrounded)
        {
            animator.SetBool("isJumping", true);
            // Aplicar gravedad personalizada si no está saltando ni en el suelo
            rb2d.AddForce(Vector2.down * gravedadPersonalizada * rb2d.gravityScale);
        }
        else
        {
            animator.SetBool("isJumping", false);

            if (!dobleSalto)
            {
                dobleSalto = true;
            }
        }
    }
}

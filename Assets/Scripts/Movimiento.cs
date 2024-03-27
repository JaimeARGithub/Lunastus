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

    // Para el backdash
    private bool puedeBackdash = true;
    private bool isBackdashing;
    private float fuerzaBackdash = 48f;
    private float tiempoBackdash = 0.2f;
    private float cooldownBackdash = 0.5f;
    private bool quiereBackdash = true;

    [SerializeField] private TrailRenderer tr;



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



        if (Input.GetKeyDown(KeyCode.Z))
        {
            quiereBackdash = true;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.IsTouchingLayers(piesCollider, groundLayer);


        if (quiereSaltar)
        {
            if (isGrounded || (dobleSalto && dobleSaltoDesbloqueado))
            {

                if (!isGrounded)
                {
                    rb2d.AddForce(Vector2.up * potenciaSalto, ForceMode2D.Impulse);
                }
                else
                {
                    rb2d.AddForce(Vector2.up * potenciaSalto, (ForceMode2D)ForceMode.VelocityChange);
                }



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


        if (quiereBackdash)
        {
            if (puedeBackdash)
            {
                StartCoroutine(Backdash());
            }

            quiereBackdash = false;
        }
    }


    private IEnumerator Backdash()
    {
        // Se preparan las variables; se ajustan los indicadores de backdash (si puede y si lo está haciendo) y se guarda el valor de gravedad
        puedeBackdash = false;
        isBackdashing = true;
        float gravedadOriginal = rb2d.gravityScale;
        rb2d.gravityScale = 0f;


        // Al rigidbody se le aplica la fuerza del backdash, se accionan la emisión y la animación
        // Se espera a que pase el tiempo del backdash y se inhabilitan emisión y animación
        if (!dir)
        {
            rb2d.velocity = new Vector2(-transform.localScale.x * fuerzaBackdash, 0f);
        } else
        {
            rb2d.velocity = new Vector2(transform.localScale.x * fuerzaBackdash, 0f);
        }

        
        tr.emitting = true;
        animator.SetBool("isBackdashing", true);
        yield return new WaitForSeconds(tiempoBackdash);
        tr.emitting = false;
        animator.SetBool("isBackdashing", false);

        // Y se le devuelve al rigidbody la gravedad original
        rb2d.gravityScale = gravedadOriginal;
        isBackdashing = false;

        // Tras ello, dentro de la corrutina se indica el tiempo de enfriamiento del backdash
        yield return new WaitForSeconds(cooldownBackdash);
        puedeBackdash = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    // Movimiento del jugador
    private float velocidad = 4.5F;
    Rigidbody2D rb2d;


    // Salto de jugador
    public bool isGrounded = false;
    private bool quiereSaltar = false;
    private float potenciaSalto = 150F;
    public LayerMask groundLayer;

    // Para dar un pequeño impulso si pisa un enemigo
    public LayerMask enemiesLayer;
    private bool steppingEnemy;

    // Collider de los pies
    public Collider2D piesCollider;

    // Gravedad personalizada
    private float gravedadPersonalizada = 100f;

    // Variables para el doble salto
    private bool dobleSaltoDesbloqueado = true; // VARIABLE QUE DEPENDE DEL GAME MANAGER
    private bool dobleSalto = true;             // CAMBIARLO PARA QUE LEA EL VALOR DE ÉL EN EL UPDATE


    //Para la utilizacion del Animator del jugador
    private Animator animator;
    [SerializeField] private bool mirandoDerecha = true;


    // Para el movimiento; izq-drch
    private float movimientoH;

    // Para el backdash
    private bool backDashDesbloqueado = true;   // VARIABLE QUE DEPENDE DEL GAME MANAGER
    private bool puedeBackdash = true;          // CAMBIARLO PARA QUE LEA EL VALOR DE ÉL EN EL UPDATE
    private bool isBackdashing;
    private float fuerzaBackdash = 1f;
    private float tiempoBackdash = 0.2f;
    private float cooldownBackdash = 0.5f;
    private bool quiereBackdash = false;

    [SerializeField] private TrailRenderer tr;


    // Para la emisión de sonidos
    public AudioSource sonidoCaminar;
    private float tiempoTranscurridoSonidoCaminar = 0f;
    private float tiempoEsperaSonidoCaminar = 0.3f;
    public AudioSource sonidoSalto;
    public AudioSource sonidoBackdash;



    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector2(movimientoH * velocidad, rb2d.velocity.y);


        isGrounded = Physics2D.IsTouchingLayers(piesCollider, groundLayer);
        if (isGrounded && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            movimientoH = 0f;


            if (Input.GetKey(KeyCode.DownArrow))
            {
                animator.SetBool("isCrouching", true);
                animator.SetBool("isRunning", false);

            } else if (Input.GetKey(KeyCode.UpArrow))
            {
                animator.SetBool("isLookingUp", true);
                animator.SetBool("isRunning", false);

            }


        } else
        {
            movimientoH = Input.GetAxisRaw("Horizontal");
            animator.SetBool("isCrouching", false);
            animator.SetBool("isLookingUp", false);
        }



        if (movimientoH > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (movimientoH < 0 && mirandoDerecha)
        {
            Girar();
        }


        if (movimientoH != 0)
        {
            animator.SetBool("isRunning", true);

            isGrounded = Physics2D.IsTouchingLayers(piesCollider, groundLayer);
            if (!sonidoCaminar.isPlaying && isGrounded && tiempoTranscurridoSonidoCaminar >= tiempoEsperaSonidoCaminar)
            {
                sonidoCaminar.Play();
                tiempoTranscurridoSonidoCaminar = 0f;
            }
        } else
        {
            animator.SetBool("isRunning", false);
            sonidoCaminar.Stop();
        }

        tiempoTranscurridoSonidoCaminar += Time.deltaTime;



        if (Input.GetKeyDown(KeyCode.W))
        {
            quiereSaltar = true;
        }



        if (Input.GetKeyDown(KeyCode.Q))
        {
            quiereBackdash = true;
        }

        steppingEnemy = Physics2D.IsTouchingLayers(piesCollider, enemiesLayer);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.IsTouchingLayers(piesCollider, groundLayer);


        if (quiereSaltar)
        {
            if (isGrounded || (dobleSalto && dobleSaltoDesbloqueado))
            {
                sonidoSalto.Play();

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
            if (isBackdashing)
            {
                animator.SetBool("isBackdashing", true);
                animator.SetBool("isJumping", false);
            } else
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isBackdashing", false);
            }

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
            if (puedeBackdash && backDashDesbloqueado)
            {
                StartCoroutine(Backdash());
            }

            quiereBackdash = false;
        }



        if (steppingEnemy)
        {
            rb2d.AddForce(Vector2.up * potenciaSalto * 0.25f, ForceMode2D.Impulse);
        }
    }


    private void Girar()
    {
        // Cada vez que el jugador se gira, la variable de mirar a la derecha se invierte
        mirandoDerecha = !mirandoDerecha;

        // Y la componente X de la escala local se invierte
        transform.Rotate(0f, 180f, 0f);
    }


    private IEnumerator Backdash()
    {
        // Se preparan las variables; se ajustan los indicadores de backdash (si puede y si lo está haciendo) y se guarda el valor de gravedad
        puedeBackdash = false;
        isBackdashing = true;
        float gravedadOriginal = rb2d.gravityScale;
        rb2d.gravityScale = 0f;


        Vector2 posicionInicial = transform.position;
        Vector2 posicionFinal;
        sonidoBackdash.Play();

        if (mirandoDerecha)
        {
            posicionFinal = posicionInicial - new Vector2(transform.localScale.x * fuerzaBackdash, 0f);
        } else
        {
            posicionFinal = posicionInicial + new Vector2(transform.localScale.x * fuerzaBackdash, 0f);
        }

        float tiempoTranscurrido = 0f;


        tr.emitting = true;
        animator.SetBool("isBackdashing", true);



        while (tiempoTranscurrido < tiempoBackdash)
        {
            float t = tiempoTranscurrido / tiempoBackdash;
            rb2d.MovePosition(Vector2.Lerp(posicionInicial, posicionFinal, t));
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        rb2d.MovePosition(posicionFinal);



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

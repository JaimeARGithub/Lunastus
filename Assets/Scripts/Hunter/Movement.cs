using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private GameManager gameManager;

    // Movimiento del jugador
    private float velocidad = 4.5F;
    Rigidbody2D rb2d;


    // Salto de jugador y movilidad aérea
    public bool grounded;
    private bool quiereSaltar = false;
    private float potenciaSalto = 150F;
    public LayerMask groundLayer;
    private Vector2 boxSize = new Vector2(0.25f, 0.125f);
    private float castDistance = 0.83f;


    // Para dar un pequeño impulso si pisa un enemigo
    public LayerMask enemiesLayer;
    private bool steppingEnemy;

    // Gravedad personalizada
    private float gravedadPersonalizada = 100f;

    // Variables para el doble salto
    //private bool dobleSaltoDesbloqueado = false; // VARIABLE QUE DEPENDE DEL GAME MANAGER
    private bool dobleSalto = true;             // CAMBIARLO PARA QUE LEA EL VALOR DE ÉL EN EL UPDATE


    //Para la utilizacion del Animator del jugador
    private Animator animator;
    [SerializeField] private bool mirandoDerecha = true;


    // Para el movimiento; izq-drch
    private float movimientoH;

    // Para el backdash
    //private bool backDashDesbloqueado = false;   // VARIABLE QUE DEPENDE DEL GAME MANAGER
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
    public AudioSource sonidoRebote;



    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gamePaused)
        {
            // LEER SI EL BACKDASH ESTÁ DESBLOQUEADO
            // LEER SI EL DOBLE SALTO ESTÁ DESBLOQUEADO

            rb2d.velocity = new Vector2(movimientoH * velocidad, rb2d.velocity.y);


            if (isGrounded() && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
            {
                movimientoH = 0f;


                if (Input.GetKey(KeyCode.DownArrow))
                {
                    animator.SetBool("isCrouching", true);
                    animator.SetBool("isRunning", false);

                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    animator.SetBool("isLookingUp", true);
                    animator.SetBool("isRunning", false);

                }


            }
            else
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

                if (!sonidoCaminar.isPlaying && isGrounded() && tiempoTranscurridoSonidoCaminar >= tiempoEsperaSonidoCaminar)
                {
                    sonidoCaminar.Play();
                    tiempoTranscurridoSonidoCaminar = 0f;
                }
            }
            else
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


            steppingEnemy = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, enemiesLayer);
        }
    }

    public void activarBackdash()
    {
        gameManager.SetBackdashUnlocked();
    }

    public void activarDobleSalto()
    {
        gameManager.SetDoublejumpUnlocked();
    }

    private void FixedUpdate()
    {
        if (quiereSaltar)
        {
            if (isGrounded() || (dobleSalto && gameManager.GetDoublejumpUnlocked()))
            {
                sonidoSalto.Play();

                if (!isGrounded())
                {
                    rb2d.AddForce(Vector2.up * potenciaSalto, ForceMode2D.Impulse);
                }
                else
                {
                    rb2d.AddForce(Vector2.up * potenciaSalto, (ForceMode2D)ForceMode.VelocityChange);
                }



                if (!isGrounded() && dobleSalto)
                {

                    dobleSalto = false;
                }
            }


            quiereSaltar = false;
        }


        if (!isGrounded())
        {
            animator.SetBool("isRunning", false);

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
            if (puedeBackdash && gameManager.GetBackdashUnlocked())
            {
                StartCoroutine(Backdash());
            }

            quiereBackdash = false;
        }



        if (steppingEnemy)
        {
            sonidoRebote.Play();
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



    private bool isGrounded()
    {
        // Castear una caja en mi posición, del tamaño indicado, giro de 0 grados,
        // hacia abajo, a la distancia indicada y contra la layer del suelo
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            grounded = true;
        } else
        {
            grounded = false;
        }
        return grounded;
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


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}

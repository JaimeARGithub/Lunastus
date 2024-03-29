using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combate : MonoBehaviour
{
    // Para la ejecución de animaciones
    public Animator animator;

    // Para detectar si se está saltando
    public bool isGrounded;
    public LayerMask groundLayer;
    public Collider2D piesCollider;

    // Para detectar si se está en movimiento
    private float movimientoH;

    // Para los cooldowns
    private bool misilDesbloqueado = true;      // VALOR QUE DEPENDE DEL GAME MANAGER
                                                // CAMBIARLO PARA QUE LEA EL VALOR DE ÉL EN EL UPDATE

    private int misilesRestantes = 5;           // VALOR QUE DEPENDE DEL GAME MANAGER
                                                // CAMBIARLO PARA QUE LEA EL VALOR DE ÉL EN EL UPDATE

    private float tiempoTranscurridoDisparo = 0f;
    private float tiempoEsperaDisparo = 0.2f;
    private float tiempoTranscurridoMisil = 0f;
    private float tiempoEsperaMisil = 2f;

    // Para los sonidos
    public AudioSource sonidoDisparo;
    public AudioSource sonidoMisil;
    public AudioSource sonidoDisparoCargado;
    public AudioSource sonidoCargando;

    // Para los disparos
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public GameObject chargedPrefab;

    // Variables de tiempo para el disparo cargado
    private float tiempoPresion = 0f;
    private float tiempoRequeridoPresion = 0.1f;
    private float tiempoRequeridoPresionDisparar = 2f;
    private float tiempoTranscurridoSonidoCarga = 0f;
    private float tiempoEsperaSonidoCarga = 0.7f;
    private bool disparoCargadoDisponible = false;
    private SpriteRenderer spRd;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spRd = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && tiempoTranscurridoDisparo >= tiempoEsperaDisparo)
        {
            sonidoDisparo.Play();
            AnimarDisparo();
            Disparar();
            tiempoTranscurridoDisparo = 0f;

            // Para el inicio de la carga
            tiempoPresion = Time.time;
        }
        tiempoTranscurridoDisparo += Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.R) && tiempoTranscurridoMisil >= tiempoEsperaMisil && misilDesbloqueado && misilesRestantes > 0)
        {
            sonidoMisil.Play();
            AnimarDisparo();
            DispararMisil();
            tiempoTranscurridoMisil = 0f;
        }
        tiempoTranscurridoMisil += Time.deltaTime;

        
        if (Input.GetKey(KeyCode.E))
        {
            if (Time.time - tiempoPresion >= tiempoRequeridoPresion)
            {
                if (!sonidoCargando.isPlaying && tiempoTranscurridoSonidoCarga >= tiempoEsperaSonidoCarga)
                {
                    sonidoCargando.Play();
                    tiempoTranscurridoSonidoCarga = 0f;
                }
                tiempoTranscurridoSonidoCarga += Time.deltaTime;


                if (Time.time - tiempoPresion >= tiempoRequeridoPresionDisparar)
                {
                    disparoCargadoDisponible = true;

                    if (spRd.color == Color.white)
                    {
                        spRd.color = Color.cyan;
                    } else
                    {
                        spRd.color = Color.white;
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && disparoCargadoDisponible)
        {
            sonidoCargando.Stop();

            spRd.color = Color.white;
            sonidoDisparoCargado.Play();
            AnimarDisparo();
            DispararCargado();

            // Restablecer el tiempo de presión para poder ejecutar un nuevo disparo cargado
            tiempoPresion = 0f;
            disparoCargadoDisponible = false;
        }
    }


    private void Disparar()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }


    private void DispararMisil()
    {
        Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
        misilesRestantes -= 1;      // CAMBIAR ESTA LÍNEA PARA QUE REDUZCA LA
                                    // CANTIDAD DE MISILES RESTANTES DEL GAME MANAGER
    }

    private void DispararCargado()
    {
        Instantiate(chargedPrefab, firePoint.position, firePoint.rotation);
    }

    private void AnimarDisparo()
    {
        movimientoH = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.IsTouchingLayers(piesCollider, groundLayer);

        if (isGrounded)
        {
            if (movimientoH == 0)
            {
                StartCoroutine(ShootIdle());
            }
            else
            {
                StartCoroutine(ShootRunning());
            }
        }
        else
        {
            StartCoroutine(ShootJumping());
        }
    }


    private IEnumerator ShootIdle()
    {
        animator.SetBool("isShootingIdle", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isShootingIdle", false);
    }

    private IEnumerator ShootRunning()
    {
        animator.SetBool("isShootingRunning", true);
        animator.SetBool("isRunning", false);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isShootingRunning", false);
        animator.SetBool("isRunning", true);
    }

    private IEnumerator ShootJumping()
    {
        animator.SetBool("isShootingJumping", true);
        animator.SetBool("isJumping", false);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isShootingJumping", false);
        animator.SetBool("isJumping", true);
    }
}
